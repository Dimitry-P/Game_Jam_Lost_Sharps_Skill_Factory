using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        private const string PassedText = "Passed";
        private const string LoseText = "Lose";
        private const string RestartText = "Restart";
        private const string NextText = "Next";
        private const string MainMenuText = "Main menu";
        private const string KillsTextPrefix = "Kills : ";
        private const string ScoreTextPrefix = "Scores : ";
        private const string TimeTextPrefix = "Time : ";

        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;

        private bool m_LevelPassed = false;

        public GameObject root;
        private Transform child;


        //Задача у ResultPanel включаться, когда мы прошли уровень или проиграли.
        //Для этого в Старте подписываюсь на 2 события: 
        private void Start()
        {
            gameObject.SetActive(false);
            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;
            if (root != null)
            {
                child = root.transform.GetChild(3);
                Debug.Log("Первый ребёнок: " + child.name);
            }
            else
            {
                Debug.LogError("Переменная root не назначена!");
            }

        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
           gameObject.SetActive(true);// Нам нужно включить наш объект
            m_LevelPassed = true;
            FillLevelStatistics();

            m_Result.text = PassedText;

            if(LevelController.Instance.HasNextLevel == true)
            {
                m_ButtonNextText.text = NextText;
            }
            else
            {
                m_ButtonNextText.text = MainMenuText;
            }
        }

        private void FillLevelStatistics()
        {
           child.gameObject.SetActive(false);  
           m_Kills.text = KillsTextPrefix + Player.Instance.NumKills.ToString();
           m_Score.text = ScoreTextPrefix + Player.Instance.Score.ToString();
           m_Time.text = TimeTextPrefix + LevelController.Instance.LevelTime.ToString("F0");
        }

        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            FillLevelStatistics();

            m_Result.text = LoseText;
            m_ButtonNextText.text = RestartText;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if(m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }
            else
            {
                LevelController.Instance.RestartLevel();
            }
        }
    }
}

