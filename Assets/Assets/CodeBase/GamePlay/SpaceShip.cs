using UnityEngine;
using Common;


namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        [SerializeField] private LeftRocket leftRocket;
        [SerializeField] private RightRocket rightRocket;
       
        private int oneBlastOff;
        private int oneBlastOff_RightRocket;

       

       


        //public GameObject explosionPrefab; // Префаб взрыва

        /// <summary>
        /// ����� ��� �������������� ��������� � ������
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// ��������� ����� ����
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// ��������� ����
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// ������������ �������� ��������
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// ������������ ������������ �������� � ��������/���
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// ����������� ������ �� �����
        /// </summary>
        private Rigidbody2D m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite PreviewImage => m_PreviewImage;

        #region Public API
        /// <summary>
        /// ���������� �������� ����� �� -1.0 �� +1.0
        /// </summary>
        private float thrustControl;
        public float ThrustControl
        {
            get => thrustControl;
            set
            {
                thrustControl = value;
                Debug.Log($"[SpaceShip] ThrustControl set to {value}");
            }
        }

        private float torqueControl;
        public float TorqueControl
        {
            get => torqueControl;
            set
            {
                torqueControl = value;
                Debug.Log($"[SpaceShip] TorqueControl set to {value}");
            }
        }


        #endregion


        private float speedMultiplier = 1.0f;
        public void SetSpeedMultiplier(float value) => speedMultiplier = value;
        public float GetSpeedMultiplier() => speedMultiplier;

        [SerializeField] private GameObject m_halfOfShipSize;
        public static float diameter = 0;


        #region Unity Event

        protected override void Start()
        {
            oneBlastOff = 0;
            oneBlastOff_RightRocket = 0;
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitOfFensive();

            if (diameter == 0)
            {
                var circle = m_halfOfShipSize.GetComponent<CircleCollider2D>();
                if (circle != null)
                {
                    diameter = circle.radius * 2f * transform.lossyScale.y;
                }
            }
        }
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (oneBlastOff == 0 && leftRocket != null)
                {
                    oneBlastOff++;
                    leftRocket.Fire();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (oneBlastOff_RightRocket == 0 && rightRocket != null)
                {
                    oneBlastOff_RightRocket++;
                    rightRocket.Fire();
                }
            }
        }

      


        private void FixedUpdate()
        {
            Debug.Log($"[SpaceShip] Before UpdateRigidBody: ThrustControl: {ThrustControl}, TorqueControl: {TorqueControl}");
            UpdateRigidBody();
            Debug.Log($"[SpaceShip] Velocity: {m_Rigid.velocity}, AngularVelocity: {m_Rigid.angularVelocity}");
            UpdateEnergyRegen();
        }

        #endregion



        /// <summary>
        /// ����� ���������� ��� ������� ��� ��������
        /// </summary>
        private void UpdateRigidBody()
        {
            float m_DragStrength = 50f; // сила торможения — подбери под ощущения

            // 🚀 Ускорение от тяги (с учётом бонуса)
            m_Rigid.AddForce(m_Thrust * speedMultiplier * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            // 🛑 Постоянное торможение, не зависящее от thrust
            m_Rigid.AddForce(-m_Rigid.velocity * m_DragStrength * Time.fixedDeltaTime, ForceMode2D.Force);

            // Поворот и стабилизация вращения
            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        [SerializeField] private Turret[] m_Turrets;
        public void Fire(TurretMode mode)
        {
            Debug.Log("SpaceShip.Fire: " + mode);
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                        m_Turrets[i].Fire();
                }
            }
        }

        

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;
        //переменная, которая определяет скорость восстановления энергии в секунду.
        //Зачем задавать скорость восстановления энергии в редакторе?
        //Поле m_EnergyRegenPerSecond(скорость восстановления энергии в секунду) обычно задается через 
        //инспектор Unity(редактор). Это позволяет разработчику настраивать скорость восстановления 
        // энергии для каждого корабля или объекта в игре.Почему это важно?
        //Гибкость настройки: Разные корабли могут иметь разную скорость восстановления энергии. Например:
        // - Быстрый истребитель может восстанавливать энергию медленно, чтобы ограничить использование мощных 
        // способностей.
        //- Тяжелый крейсер может восстанавливать энергию быстрее, чтобы поддерживать длительные
        // боевые действия.


        private float m_PrimaryEnergy;//Это поле, которое хранит текущее значение основной энергии. 
        private float m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        public void TurnOffEnemy()
        {
            LaserShake laserShake = new LaserShake();
            var ray = laserShake.GetComponent<LaserShake>();
            ray.gameObject.SetActive(false);    
        }

        private void InitOfFensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
        //- Использование Time.fixedDeltaTime гарантирует,
        //что расчеты будут происходить с постоянным шагом времени, независимо от FPS.
        //m_EnergyRegenPerSecond — скорость восстановления энергии в секунду.
        //- Time.fixedDeltaTime — время прошедшее с момента последнего вызова фиксированного обновления.
        //-Умножение этих двух величин дает количество энергии, которое должно быть восстановлено за текущий кадр.
        //Суть формулы: равномерное восстановление энергии.
        // Без использования Time.fixedDeltaTime или аналогичного механизма, энергия могла бы
        // восстанавливаться скачкообразно, что зависит от частоты кадров (FPS). Например:
        //-Если FPS высокий(например, 120), энергия могла бы восстанавливаться слишком быстро.
        //-Если FPS низкий(например, 30), энергия восстанавливалась бы медленнее.
        //Использование Time.fixedDeltaTime гарантирует, что энергия будет восстанавливаться
        //с одинаковой скоростью независимо от FPS. Это делает игровой процесс более
        //предсказуемым и стабильным.


        //Заключение:
        //    1.Равномерное восстановление энергии достигается благодаря использованию 
        //        Time.fixedDeltaTime.Это обеспечивает стабильность и предсказуемость игрового процесса.
        //    2.Автоматическая регенерация энергии — это стандартная механика, которая добавляет 
        //        стратегический элемент в игру.
        //    3.Настройка скорости восстановления через редактор позволяет гибко настраивать параметры 
        //        кораблей, балансировать игру и создавать уникальные игровые механики.



            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }


        public bool DrawAmmo(int count) //отнять патроны
        {
            if (count == 0) return true;

            if(m_SecondaryAmmo >= count) // Проверка, есть ли у нас то кол-во патрон, кот. мы хотим скушать
            {
                m_SecondaryAmmo -= count;// Если патронов больше либо равно сколько мы хотим отнять, то мы 
                //отнимаем эти значения
                return true; // У нас получилось скушать
            } 

            return false;
        }


        public bool DrawEnergy(int count) //отнять патроны
        {
            if (count == 0) return true;

            if (m_PrimaryEnergy >= count) // Проверка, есть ли у нас то кол-во патрон, кот. мы хотим скушать
            {
                m_PrimaryEnergy -= count;// Если патронов больше либо равно сколько мы хотим отнять, то мы 
                //отнимаем эти значения
                return true; // У нас получилось скушать
            }

            return false;
        }

        public void AssignedWeapon(TurretProperties props)
        {
            for(int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignedLoadOut(props);
            }
            //Это "мастер-метод", который получает некие параметры 
            //оружия(TurretProperties props) и распределяет их 
            //всем турелям(m_Turrets).
        }
        public float GetThrust()
        {
            return m_Thrust;
        }

        public void SetThrust(float value)
        {
            m_Thrust = value;
        }

    }
}

