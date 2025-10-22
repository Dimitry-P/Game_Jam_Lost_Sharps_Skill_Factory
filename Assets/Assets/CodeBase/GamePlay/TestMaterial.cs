using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class TestMaterial : MonoBehaviour
{
    public MeshRenderer Renderer;
    public Material m_mat;
    public float t;

    private void Start()
    {
        m_mat = Renderer.material;
    }

    private void Update()
    {
        m_mat.mainTextureOffset += new Vector2(t, t);
    }

}
