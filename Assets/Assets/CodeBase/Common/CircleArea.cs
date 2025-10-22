using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Common
{
    public class CircleArea : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        public Vector2 GetRandomInsideZone()
        {
            return (Vector2)transform.position + (Vector2) UnityEngine.Random.insideUnitSphere * m_Radius;
        }


#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
//        Ётот метод вызываетс€ только в редакторе, когда ты выдел€ешь объект с этим скриптом.
//        –исует плоский зелЄный круг в сцене дл€ нагл€дности.
//        transform.forward Ч нормаль к плоскости круга (в 2D обычно это z-ось).


#endif
    }
}

