using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echoes_At_The_Last_Station
{
    public class LevelCompletionScore : LevelCondition
    {
        [SerializeField] private int m_Score;
        public override bool IsCompleted
        {
            get 
            {


                if (AddScore.Instance.Score == m_Score)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

