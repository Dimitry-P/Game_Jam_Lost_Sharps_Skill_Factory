using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echoes_At_The_Last_Station
{
    [CreateAssetMenu]
    public class LevelProperties : ScriptableObject
    {
        [SerializeField] private string m_Title; //Название уровня
        [SerializeField] private string m_SceneName;
        [SerializeField] private Sprite m_PreviewImage;  //Спрайт у уровня
        [SerializeField] private LevelProperties m_NextLevel;  //Ссылка на следующий уровень

        public string Title => m_Title;
        public string SceneName => m_SceneName;
        public Sprite PreviewImage => m_PreviewImage;
        public LevelProperties NextLevel => m_NextLevel;
    }
}

