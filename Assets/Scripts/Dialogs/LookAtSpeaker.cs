using UnityEngine;

public class LookAtSpeaker : MonoBehaviour
{
    private Transform target;
    public float rotateSpeed = 5f;

    public void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Debug.Log($"LookAtSpeaker: Target set to {newTarget.name}");
    }

    public void ClearTarget()
    {
        target = null;
        Debug.Log("LookAtSpeaker: Target cleared");
    }
}
