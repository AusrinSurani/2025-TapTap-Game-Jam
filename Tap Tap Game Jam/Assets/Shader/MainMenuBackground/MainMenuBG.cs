using UnityEngine;

public class MainMenuBG : MonoBehaviour
{
    [Header("Color Palette (6 Colors)")]
    public Color[] colors = new Color[6]; // 颜色调色板

    [Header("Effect Controls")]
    [Range(0.01f, 10.0f)]
    public float speed = 1.0f; // 动画速度

    [Range(1.0f, 20.0f)]
    public float patternScale = 5.0f; // 图案缩放

    [Range(0.01f, 10.0f)]
    public float edgeBlur = 1.0f; // 边缘模糊/卷曲强度

    [Range(0.0f, 4.0f)]
    public float brightness = 1.0f; // 亮度

    [Range(0.0f, 2.0f)]
    public float darkness = 0.0f; // 暗部强度

    [Range(0.0f, 0.4f)]
    public float noiseFactor = 0.05f; // 噪点/颗粒感强度

    // 私有变量来存储材质实例
    private Material _materialInstance;

    void Awake()
    {
        // 获取物体上的Renderer组件
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // 使用 .material 会创建一个材质的实例，这样修改就不会影响到项目中的原始材质文件
            _materialInstance = renderer.material;
        }
        else
        {
            // 如果是UI RawImage, 获取其组件
            var rawImage = GetComponent<UnityEngine.UI.RawImage>();
            if (rawImage != null)
            {
                 // 同样，实例化材质
                _materialInstance = new Material(rawImage.material);
                rawImage.material = _materialInstance;
            }
        }
        
        if (_materialInstance == null)
        {
            Debug.LogError("No Renderer or RawImage found on this GameObject. Cannot apply animated material.");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (_materialInstance == null) return;

        // 每一帧都更新材质的属性
        _materialInstance.SetFloat("_Speed", speed);
        _materialInstance.SetFloat("_PatternScale", patternScale);
        _materialInstance.SetFloat("_CurlScale", edgeBlur);
        _materialInstance.SetFloat("_Brightness", brightness);
        _materialInstance.SetFloat("_Darkness", darkness);
        _materialInstance.SetFloat("_NoiseFactor", noiseFactor);

        if (colors.Length >= 6)
        {
            _materialInstance.SetColor("_Color0", colors[0]);
            _materialInstance.SetColor("_Color1", colors[1]);
            _materialInstance.SetColor("_Color2", colors[2]);
            _materialInstance.SetColor("_Color3", colors[3]);
            _materialInstance.SetColor("_Color4", colors[4]);
            _materialInstance.SetColor("_Color5", colors[5]);
        }
    }
}