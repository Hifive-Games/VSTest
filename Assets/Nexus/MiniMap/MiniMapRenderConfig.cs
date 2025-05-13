using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class MiniMapRenderConfig : MonoBehaviour
{
    [Header("Match your UI RawImage size here")]
    public int resolution = 300;
    public int depthBuffer = 16;
    public int antiAliasing = 1;

    public RawImage rawImage;
    public Texture2D texture;

    void Start()
    {
        var cam = GetComponent<Camera>();
        cam.allowMSAA = false;                  // ensure camera MSAA = 1
        var rt = new RenderTexture(resolution, resolution, depthBuffer)
        {
            antiAliasing = antiAliasing,
            name = "MinimapRT"
        };
        cam.targetTexture = rt;

        texture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false)
        {
            name = "MinimapTex"
        };

        rawImage.texture = texture;
        rawImage.rectTransform.sizeDelta = new Vector2(resolution, resolution);
        rawImage.gameObject.SetActive(true);
        cam.gameObject.SetActive(true);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.clear;
        
    }
}