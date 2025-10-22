using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLoopBackground : MonoBehaviour
{
    [Header("颜色列表")]
    public List<Color> colors = new List<Color>() {
        new Color(0.286f, 0.412f, 0.431f), // #49696e
        new Color(0.117f, 0.345f, 0.502f), // #1E5880
        new Color(0.063f, 0.243f, 0.384f), // #103E62
        new Color(0.0f, 0.157f, 0.282f),   // #002848
        new Color(0.004f, 0.008f, 0.016f), // #010204
        new Color(0.129f, 0.463f, 0.525f)  // #217683
    };

    [Header("循环切换颜色")]
    public bool loop = true;

    [Header("是否随机切换颜色")]
    public bool randomize = true;

    [Header("颜色切换速度")]
    public float transitionSpeed = 0.8f;

    private int currentIndex = 0;
    private Color targetColor;
    private Image uiImage;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 尝试自动获取背景组件
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (colors.Count > 0)
            targetColor = colors[0];

        StartCoroutine(ColorChangeLoop());
    }

    IEnumerator ColorChangeLoop()
    {
        while (loop)
        {
            // 选择下一个颜色
            int nextIndex = randomize ? Random.Range(0, colors.Count) : (currentIndex + 1) % colors.Count;
            targetColor = colors[nextIndex];

            // 平滑过渡
            float t = 0f;
            Color startColor = GetCurrentColor();

            while (t < 1f)
            {
                t += Time.deltaTime * transitionSpeed;
                Color newColor = Color.Lerp(startColor, targetColor, t);
                SetColor(newColor);
                yield return null;
            }

            currentIndex = nextIndex;
        }
    }

    Color GetCurrentColor()
    {
        if (uiImage != null) return uiImage.color;
        if (spriteRenderer != null) return spriteRenderer.color;
        return Color.black;
    }

    void SetColor(Color color)
    {
        if (uiImage != null) uiImage.color = color;
        if (spriteRenderer != null) spriteRenderer.color = color;
    }
}