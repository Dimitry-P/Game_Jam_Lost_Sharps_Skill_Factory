using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;


namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class LeadPoint_Ship : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }
        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [Range(0.0f, 5.0f)]
        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;
        [SerializeField] private float m_ShootDelay;
        [SerializeField] private float m_EvadeRayLength;

        //три кэшированные переменные:
        [SerializeField] private SpaceShip m_SpaceShip;



        private Vector3 m_MovePosition; // Точка, куда двигается наш корабль


        private Destructible m_SelectedTarget; // у корабля есть некая цель (Какой-то объект)
        // то есть в этих двух строчках мы разделяем точку, куда двигаться и ссылку на объект слежения

        private Timer TestTimer;


        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private int m_CurrentPatrolIndex = 0;



        private void Awake()
        {
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();
            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();          // движемся к текущей цели
            ActionEvadeCollision();
            ActionFindNewAttackTarget();
            ActionFire();
        }

        private void ActionControlShip()
        {
            Vector3 direction = m_MovePosition - transform.position;
            float distance = direction.magnitude;

            float angle = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform);

            //m_NavigationAngular - это поле, доступное в инспекторе, в котором настраивается "скорость" поворота корабля. Если не умножать на это значение, корабль поворачиваться не будет.

            m_SpaceShip.ThrustControl = m_NavigationLinear; // газ в пол

            m_SpaceShip.TorqueControl = angle * m_NavigationAngular;

        }


        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship) // Статичный метод, который возвращает float, то есть некий УГОЛ
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            // переводит позицию в локальные координаты

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            // получаем угол между двумя векторами
            // SignedAngle - это угол со знаком

            if (Mathf.Abs(angle) < 2.0f) // Если угол меньше 2°
                return 0;
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE; // Эта строчка ограничивает наш угол
                                                                           //Если наш угол становится больше 45 градусов, то ОН ПРОСТО 45 градусов.
                                                                           // Т.е. у нас всегда будет ограничение от 0 до 45 градусов.
                                                                           //Посчитанный угол нужно перевести в нормализованные координаты, т.е. разделить на 45.
                                                                           //В итоге, если наша целевая точка находится больше чем на 45 градусов от нас в повороте,
                                                                           //то мы максимальном жмём педаль в пол, чтобы наш корабль повернул. 
                                                                           //Если меньше, то тогда получаем какое-то меньшее значение. 
                                                                           //При маленьких значениях корабль будет плавно совсем чуть-чуть доворачивать до цели.
                                                                           // Чем больше я поворачиваю корабль, тем больше нужно применить силы к вращательному моменту.
                                                                           // Как только угол становится больше чем 45 градусов, я всегда применяю полную силу для вращения
            return -angle;
        }



        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                {
                    if (m_PatrolPoint != null) // Проверяю, в зоне ли патруля мой корабль
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;
                        // m_PatrolPoint.transform.position - transform.position -- Это вычитание двух векторов. Оно создаёт вектор направления от цели к кораблю.
                        //данная строка проверяет, находится ли AI внутри зоны патрулирования радиуса m_PatrolPoint.Radius
                        // Вычитаем позицию текущего объекта (`transform.position`) из позиции точки патрулирования (`m_PatrolPoint.transform.position`)
                        // Результат: получаем вектор направления от текущего объекта к точке патрулирования.
                        //.sqrMagnitude возвращает квадрат длины вектора (без извлечения квадратного корня).  
                        //-Это быстрее, чем.magnitude, потому что не требует вычисления квадратного корня.
                        //Получается, что:
                        //`(A - B).sqrMagnitude` — это быстрый способ проверить, находится ли объект в пределах радиуса, без вычисления квадратного корня.  
                        // Сравниваем с `Radius* Radius`, потому что работаем с квадратами.  



                        if (isInsidePatrolZone == true)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished == true)
                            //используется для регулярного изменения направления движения,
                            //чтобы бот не менял направление каждую миллисекунду, а делал это раз в несколько секунд
                            //То есть:
                            //— Пока таймер не закончился - бот летит в старую точку
                            //— Таймер закончился - бот выбирает новую точку
                            {
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                                //`Random.onUnitSphere` — это команда, которая создаёт случайное направление в 3D-пространстве.
                                // - Random.onUnitSphere возвращает вектор длиной 1, который указывает в случайном направлении в 3D.
                                //-То есть он даёт направление, а не координату точки где-то в пространстве.
                                // назначаю какую-то случайную точку на поверхности сферической зоны патрулирования. Она работает следующим образом:
                                // Берется случайное направление (вектор длиной 1).
                                // Это направление масштабируется до радиуса патрульной зоны.
                                // Полученная точка смещается так, чтобы находиться относительно центра зоны патрулирования.
                                // Дополнительно, условие isInsidePatrolZone проверяет, находится ли объект внутри зоны патрулирования,
                                // но в случае isInsidePatrolZone == true вы генерируете новую точку, чтобы объект оставался внутри зоны.
                                //Если точка выходит за пределы зоны(isInsidePatrolZone == false), 
                                // вы задаете m_MovePosition = m_PatrolPoint.transform.position, что отправляет объект в центр зоны.

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                                //занова запускаем наш код
                            }

                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                }
            }
        }

        private void ActionEvadeCollision()
        {
            var hit = Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength);
            if (hit.collider != null) //Если Raycast с чем-то пересёкся
            {
                m_MovePosition = transform.position + transform.right * 100.0f; //поворачиваю вправо, если вижу впереди какое-то препятствие
            }
        }


        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_ShootDelay);
            }
        }

        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            //potentialTarget = GameObject.FindWithTag("Player");
            //foreach (var v in Destructible.AllDestructbles)
            //{
            //    if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

            //    if (v.TeamId == Destructible.TeamIdNeutral) continue;

            //    if (v.TeamId == m_SpaceShip.TeamId) continue;

            //    float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

            //    if (dist < maxDist)
            //    {
            //        maxDist = dist;
            //        potentialTarget = v;
            //    }
            //}
            return potentialTarget;
        }


        #region Timers

        private void InitTimers()  // Инициализирую все наши таймеры
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            //m_RandomizeDirectionTimer — это переменная типа Timer
            //Она хранит информацию о времени до следующего события.
            //new Timer(m_RandomSelectMovePointTime) — создаётся новый объект таймера
            //m_RandomSelectMovePointTime — это число от 0.0 до 1.0 (настраивается в инспекторе),
            //которое означает интервал времени в секундах между выбором новой точки движения.
            //Например, если там стоит 0.6, то бот будет менять направление каждые 0.6 секунды.

            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        //Где и как таймер обновляется?
        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            //Это очень важная строка. Каждый кадр она уменьшает значение таймера на прошедшее время (обычно 1/60 секунды),
            //чтобы таймер "шёл" во времени. Без этого он бы никогда не закончился.
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

        }

        public void SetPatrolBehavoiur(AIPointPatrol point) // если у меня изменилось поведение или спавнер заспавнил моего бота,
                                                            // то мы должны задать эту точку и задать стартовые параметры
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }

        #endregion  //Здесь инициализируем и обновляем таймеры
    }
}



