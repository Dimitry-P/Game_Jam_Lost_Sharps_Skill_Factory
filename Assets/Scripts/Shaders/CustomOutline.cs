using UnityEngine;

public class CustomOutline : MonoBehaviour
{
    public Color outlineColor = Color.cyan;
    public float outlineWidth = 0.03f;
    public float fadeDuration = 0.5f;

    private Material outlineMaterial;
    private Material[] originalMaterials;
    private Renderer rend;

    private bool targetVisible = false;
    private bool isVisible = false;
    private float alpha = 0f;
    private float fadeSpeed;

    void Start()
    {
        rend = GetComponent<Renderer>();
        outlineMaterial = new Material(Shader.Find("Custom/OutlineTransparent"));
        outlineMaterial.SetColor("_OutlineColor", new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0f));
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);

        if (rend != null)
        {
            originalMaterials = rend.materials;
            SetVisible(false, instant: true);
        }

        fadeSpeed = 1f / fadeDuration;
    }

    void Update()
    {
        if (targetVisible && alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (!isVisible) EnableOutline();
        }
        else if (!targetVisible && alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
        }

        alpha = Mathf.Clamp01(alpha);
        SetAlpha(alpha);

        if (alpha == 0f && isVisible)
        {
            DisableOutline();
        }
        else if (alpha == 1f && !isVisible)
        {
            isVisible = true;
        }
    }

    public void SetVisible(bool visible, bool instant = false)
    {
        targetVisible = visible;

        if (instant)
        {
            if (visible)
            {
                alpha = 1f;
                EnableOutline();
                SetAlpha(1f);
            }
            else
            {
                alpha = 0f;
                DisableOutline();
                SetAlpha(0f);
            }
        }
    }

    private void EnableOutline()
    {
        if (rend == null || outlineMaterial == null) return;

        var materialsWithOutline = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(materialsWithOutline, 0);
        materialsWithOutline[materialsWithOutline.Length - 1] = outlineMaterial;
        rend.materials = materialsWithOutline;
        isVisible = false; // будет переключено на true при достижении alpha=1
    }

    private void DisableOutline()
    {
        if (rend != null && originalMaterials != null)
            rend.materials = originalMaterials;
        isVisible = false;
    }

    private void SetAlpha(float a)
    {
        if (outlineMaterial != null)
        {
            Color c = outlineMaterial.GetColor("_OutlineColor");
            c.a = a;
            outlineMaterial.SetColor("_OutlineColor", c);
        }
    }

    public void SetColor(Color color)
    {
        outlineColor = color;
        if (outlineMaterial != null)
        {
            Color c = outlineMaterial.GetColor("_OutlineColor");
            c.r = color.r;
            c.g = color.g;
            c.b = color.b;
            outlineMaterial.SetColor("_OutlineColor", c);
        }
    }
}
