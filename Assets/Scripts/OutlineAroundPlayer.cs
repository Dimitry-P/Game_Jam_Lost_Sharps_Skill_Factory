using UnityEngine;

public class OutlineAroundPlayer : MonoBehaviour
{
    public Transform player;
    public float radius = 5f;
    public LayerMask outlineLayerMask;

    private CustomOutline[] outlinesInRange;

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, radius, outlineLayerMask);

        outlinesInRange = new CustomOutline[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            outlinesInRange[i] = colliders[i].GetComponent<CustomOutline>();
        }

        foreach (var outline in FindObjectsOfType<CustomOutline>())
        {
            if (outline == null) continue;

            bool shouldHighlight = System.Array.Exists(outlinesInRange, o => o == outline);
            outline.SetVisible(shouldHighlight, instant: false);
        }
    }
}
