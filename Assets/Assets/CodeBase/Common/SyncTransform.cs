using UnityEngine;


namespace Common
{
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void Update()
        {
            transform.position = new Vector3(m_Target.position.x, m_Target.position.y, transform.position.z);
        }
    }
}

