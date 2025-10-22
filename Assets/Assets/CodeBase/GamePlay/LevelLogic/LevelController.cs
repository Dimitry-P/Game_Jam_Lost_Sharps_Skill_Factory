using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        private const string MainMenuSceneName = "main_menu";


        //У LevelController будет 2 события, что уровень завершён.
        public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        //Делаем перезагрузку уровня и загрузку следующего уровня:
        //Наш LevelController должен знать, какой сейчас у него текущий уровень
        //Поэтому делаю ссылку на LevelProperties
        [SerializeField] private LevelProperties m_LevelProperties;
        [SerializeField] private LevelCondition[] m_Condition;

        private bool m_IsLevelCompleted = false;
        private float m_LevelTime;

        public bool HasNextLevel => m_LevelProperties.NextLevel != null;
        //Проверка, есть ли у нас следующий уровень или нет
        public float LevelTime => m_LevelTime;

        private void Start()
        {
            Time.timeScale = 1; 
            m_LevelTime = 0;
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }
           

            if(Player.Instance.NumLives == 0)
            {
                Lose();
            } 
        }

        private void CheckLevelConditions()  // ПРОВЕРКА НА ЗАВЕРШЕНИЕ УРОВНЯ
        {
           

            int numCompleted = 0;
            for (int i = 0; i < m_Condition.Length; i++)
            {
                if (m_Condition[i].IsCompleted == true)
                {
                    numCompleted++;
                }
            }
            if (numCompleted == m_Condition.Length)
            {
                m_IsLevelCompleted = true;

                Pass();
                Debug.Log("m_IsLevelCompleted");
            }
        }
        private void Lose()
        {
            LevelLost?.Invoke();
            Time.timeScale = 0;
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            Time.timeScale = 0;
        }

        public void LoadNextLevel()
        {
            if (HasNextLevel == true) //Если следующий уровень имеется, то:
                SceneManager.LoadScene(m_LevelProperties.NextLevel.SceneName); //загружаем следющий уровень
            //Что делает m_LevelProperties.NextLevel.SceneName?
            //Сначала мы берём объект m_LevelProperties(текущий уровень).
            //Через точку обращаемся к его свойству NextLevel → получаем объект следующего уровня.
            //У этого объекта следующего уровня берём его свойство SceneName → это строка с названием сцены.
            //m_LevelProperties.NextLevel.SceneName — это цепочка обращений
            //И в итоге в LoadScene() мы передаём имя сцены следующего уровня.
            //В C# можно и нужно писать такие цепочки обращений через точки, и это абсолютно нормальная практика.
            //Когда у тебя несколько объектов вложены друг в друга, можно писать цепочкой.
            //Да, но у меня m_NextLevel  и  m_SceneName  не вложены друг в друга. Они равнозначны.
            //Ты прав, что поля m_NextLevel и m_SceneName находятся на одном уровне внутри класса LevelProperties.
            //Но давай разберём, почему всё равно можно писать цепочку:
            //Важный момент: мы не обращаемся напрямую к m_NextLevel и m_SceneName
            //Мы работаем не с полями напрямую, а через свойства!!!
            //m_LevelProperties — это объект LevelProperties (например, текущий уровень).
            //.NextLevel — возвращает другой объект LevelProperties — следующий уровень.
            //.SceneName — уже у этого нового объекта берём название сцены.
            //Почему всё работает?
            //Потому что m_NextLevel — это ссылка на другой объект того же класса,
            //и у него есть собственное поле m_SceneName.
            //Ты прав, что поля m_NextLevel и m_SceneName лежат на одном уровне внутри одного объекта.
            //Но в цепочке m_LevelProperties.NextLevel.SceneName
            //мы сначала берём ссылку на другой объект LevelProperties (NextLevel),
            //а затем у этого объекта читаем SceneName.
            else
                SceneManager.LoadScene(MainMenuSceneName);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}

