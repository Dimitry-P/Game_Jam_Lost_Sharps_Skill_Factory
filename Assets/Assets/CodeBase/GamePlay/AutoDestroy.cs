using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {
        float animationLength = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animationLength);
    }
}
