using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    public float maxDistance = 5f;
    private CustomOutline currentOutline;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            CustomOutline newOutline = hit.collider.GetComponent<CustomOutline>();

            if (newOutline != null)
            {
                if (currentOutline != null && currentOutline != newOutline)
                    currentOutline.SetVisible(false);

                newOutline.SetVisible(true);
                currentOutline = newOutline;
            }
            else
            {
                if (currentOutline != null)
                {
                    currentOutline.SetVisible(false);
                    currentOutline = null;
                }
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.SetVisible(false);
                currentOutline = null;
            }
        }
    }
}
