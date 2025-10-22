using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;


namespace SpaceShooter
{
    public class AIPointPatrolPath : MonoBehaviour
    {
        [SerializeField] public Transform[] m_PatrolPoints;

        public Transform[] PatrolPoints => m_PatrolPoints;


        public Transform GetPoint(int index)
        {
            if (m_PatrolPoints == null || m_PatrolPoints.Length == 0) return null;
            return m_PatrolPoints[index % m_PatrolPoints.Length];
        }

        public int Length => m_PatrolPoints.Length;
    }
}

