using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;

namespace Echoes_At_The_Last_Station
{
    public class LevelController : SingletonBase<LevelController>
    {
        public event UnityAction LevelPassed;
        [SerializeField] private LevelProperties m_LevelProperties;

        [SerializeField] private LevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted;

        public bool HasNextFlashBack => m_LevelProperties.NextLevel != null;

        private void Start()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            Cursor.visible = true;
            CheckLevelConditions();
        }


        private void CheckLevelConditions()
        {
            if (m_IsLevelCompleted == true) return;

            int numCompleted = 0;

            if (m_IsLevelCompleted == true) return;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                {
                    numCompleted++;
                }
            }
            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                Pass();
                Debug.Log("!!!!!!!!!!!!!!");
            }

        }

        private void Pass()
        {
          
            LevelPassed?.Invoke();
            
        }

      
        public void LoadFlashBack()
        {
            if (HasNextFlashBack == true)
                SceneManager.LoadScene(m_LevelProperties.NextLevel.SceneName);
            else
                SceneManager.LoadScene("Main_Menu");
        }
    }
}

