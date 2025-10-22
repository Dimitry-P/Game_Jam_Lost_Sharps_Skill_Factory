using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = System.Random;
using SpaceShooter;

namespace Common
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То, что может иметь хитпоинты.
    /// </summary>
    public class Destructible : Entity
    {
        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;
        public GameObject smallMeteorPrefab;
        [SerializeField] private int fragmentsCount = 3;
        [SerializeField] private float spreadForce = 0.1f;
        [SerializeField] private Projectile projectile;
        [SerializeField] private AudioClip m_Meteor_Damage_Sound;
        [SerializeField] private float m_ExplosionVolume = 5f;


        #region Properties
        /// <summary>
        /// Объект игнорирует повреждения
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

       

        /// <summary>
        /// Стартовое кол-во хитпоинтов
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;

        /// <summary>
        /// Текущие хитпоинты
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        #endregion

        


        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
            
        }
        #endregion

        #region Public API
        /// <summary>
        /// применение дамага к объекту
        /// </summary>
        /// <param name="damage">Урон наносимый объекту</param>
        public void ApplyDamage(int damage)
        {
            Debug.Log($"ApplyDamage вызван для {gameObject.name}");
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;
            if (m_CurrentHitPoints <= 0)
            {
                
                OnDeath();
                Debug.Log("Корабль уничтожен");
                
            }
        }
        
        #endregion



        /// <summary>
        /// Переопределяемое событие уничтожения объекта, когда хитпоинты ниже нуля
        /// </summary>
        protected virtual void OnDeath()
        {
            m_EventOnDeath?.Invoke();
            

            if (gameObject.CompareTag("Meteor"))
            {
                SpawnFragments(0.4f);
                if (m_Meteor_Damage_Sound != null)
                {
                    AudioSource.PlayClipAtPoint(m_Meteor_Damage_Sound, transform.position, m_ExplosionVolume);
                }
            }
            Destroy(gameObject);
           
        }




                    //БЕЗТЕГОВАЯ КОЛЛЕКЦИЯ DESTRUCTIBLE-ов:

        private static HashSet<Destructible> m_AllDestructibles;//Это лист. То же что и List. У нас есть список, кот. хранит в себе
                                                                //все уничтожаемые объекты, все Destructibles
        // Да, HashSet<Destructible> похож на List<Destructible>, потому что оба хранят коллекцию объектов.
        // НО:
        // HashSet и List — это разные структуры данных с разным поведением, эффективностью и назначением.
        // Почему здесь использован именно HashSet<Destructible>?
        // Потому что:
        // Нам нужно хранить все уникальные объекты Destructible(без повторений).
        // Очень часто мы будем выполнять операции вида:
        // foreach (var d in AllDestructibles) — получить всех.
        // if (m_AllDestructibles.Contains(someDestructible)) — проверить наличие.
        // m_AllDestructibles.Remove(this) — убрать уничтоженный.
        // m_AllDestructibles.Add(this) — добавить новый.
        // Вот где HashSet полезен:
        // Add() не добавит объект второй раз, если он уже есть.
        // Remove() будет очень быстрым.
        // Contains() работает намного быстрее, чем у List.
        // Если это List → каждый вызов .Contains() и .Remove() будет дороже по времени.
        // Если это HashSet → всё работает намного быстрее и безопаснее (никаких дубликатов, меньше багов).
        // HashSet - Это тоже коллекция, но с другим поведением. И здесь она выбрана, потому что лучше подходит для задачи.


        public static IReadOnlyCollection<Destructible> AllDestructbles => m_AllDestructibles;
        //Публичное св-во, такой спец лист, кот. мы можем только прочитать. Оно просто ссылается на нашу коллекцию.

        protected virtual void OnEnable() // Когда у нас появляется объект, то
        {
            if(m_AllDestructibles == null)// смотрим, если наша коллекция не создана, то
                m_AllDestructibles = new HashSet<Destructible>();// мы создаём нашу коллекцию

            m_AllDestructibles.Add(this); //и добавляем свой собственный экземпляр класса туда
        }
        //на первый взгляд может показаться, что метод OnEnable() нигде не вызывается вручную.
        //Но на самом деле — он вызывается автоматически движком Unity.
        //Объяснение: что такое OnEnable()
        //OnEnable() — это специальный метод Unity, называемый жизненным циклом MonoBehaviour. Он вызывается автоматически, когда:
        //Скрипт включается(или активируется объект, на котором он висит).
        //Объект появляется в сцене впервые и включён.
        //Или когда происходит SetActive(true) на объекте, или enabled = true на компоненте.
        //Если у тебя в скрипте написано:
        //protected virtual void OnEnable()
        //{
        //    m_AllDestructibles.Add(this);
        //}
        //То Unity сама вызовет этот метод в тот момент, когда объект, к которому привязан скрипт, становится активным.
        //Именно в этот момент текущий объект (this) и будет добавлен в HashSet m_AllDestructibles.
        //Почему ты не видишь OnEnable() в вызовах?
        //Потому что он не вызывается явно в коде — этим занимается внутренний механизм Unity,
        //и он не всегда виден напрямую при отладке.
        //Ты создаёшь объект "Player", у него есть компонент Destructible, и он активен в сцене.
        //Unity автоматически вызывает OnEnable() → объект добавляется в m_AllDestructibles.







        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this); //Удаляем из основной коллекции, т.е. из m_AllDestructibles
        }

        //Создание команд (Team):
        public const int TeamIdNeutral = 0; // Нейтральная команда

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;


      


        public void SpawnFragments(float customSpreadForce)
        {
            Debug.Log("SpawnFragments вызван — создаём много мелких метеоров");
            Vector3 position = transform.position;
            Collider2D[] colliders = new Collider2D[fragmentsCount];

            float spawnRadius = 3f; // Больше радиус для распределения
            float localSpreadForce = Mathf.Max(spreadForce, 1f); // Минимум 1 для силы

            for (int i = 0; i < fragmentsCount; i++)
            {
                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
                GameObject fragment = Instantiate(smallMeteorPrefab, position + (Vector3)randomOffset, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)));

                Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.mass = 0.1f; // Маленькая масса
                    Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
                    rb.AddForce(randomDir * customSpreadForce, ForceMode2D.Impulse);
                }

                colliders[i] = fragment.GetComponent<Collider2D>();
                Destroy(fragment, 10f);
            }

            // Отключаем столкновения между осколками
            for (int i = 0; i < fragmentsCount; i++)
            {
                for (int j = i + 1; j < fragmentsCount; j++)
                {
                    if (colliders[i] != null && colliders[j] != null)
                        Physics2D.IgnoreCollision(colliders[i], colliders[j]);
                }
            }

            Destroy(gameObject, 0.1f);
        }
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }
}

