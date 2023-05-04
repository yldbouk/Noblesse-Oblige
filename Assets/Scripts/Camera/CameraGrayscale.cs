using UnityEngine;

public class CameraGrayscale : MonoBehaviour
{
    private Material material;
    [SerializeField] private Shader shader;

    void Start() { material = new Material(shader); }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) { Graphics.Blit(source, destination, material); }
}
