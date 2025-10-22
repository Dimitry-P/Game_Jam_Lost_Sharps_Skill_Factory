using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Echoes_At_The_Last_Station
{
    public class ResultPanel : MonoBehaviour
    {
       
        private const string NextText = "Next";
        [SerializeField] private Text m_Result;


        [SerializeField] private Text m_ButtonNextText;

        private bool m_LevelPassed = false;

      


        //Задача у ResultPanel включаться, когда мы прошли уровень или проиграли.
        //Для этого в Старте подписываюсь на 2 события: 
        private void Start()
        {
            gameObject.SetActive(false);
           
            LevelController.Instance.LevelPassed += OnLevelPassed;
          

        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
           gameObject.SetActive(true);// Нам нужно включить наш объект
            Time.timeScale = 0f;


            m_LevelPassed = true;
          

            

            if(LevelController.Instance.HasNextFlashBack == true)
            {
                m_ButtonNextText.text = "Next";
            }
            else
            {
                m_ButtonNextText.text = "Main";
            }
        }

      

       

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if(m_LevelPassed == true)
            {
                LevelController.Instance.LoadFlashBack();
            }
           
        }
    }
}

