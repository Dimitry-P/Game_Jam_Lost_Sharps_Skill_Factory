using UnityEngine;

[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    public Material material;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
            Graphics.Blit(src, dest, material);
        else
            Graphics.Blit(src, dest);
    }
}
