using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool grayscale = false;
    private Material material;
    public Shader shader;

    void Start() { material = new Material(shader); }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(grayscale) Graphics.Blit(source, destination, material); else Graphics.Blit(source, destination);
    }
}
