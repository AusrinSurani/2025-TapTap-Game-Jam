using UnityEngine;

public class WorldParallaxEffect : MonoBehaviour
{
    [Header("图层对象")]
    public Transform[] layers;

    [Header("偏移强度")]
    public float[] parallaxStrength;

    [Header("移动的平滑度")]
    [Range(0.1f, 10f)]
    public float smoothing = 5f;

    [Header("偏移方向")] 
    public bool x;
    public bool y;

    private Vector3[] initialPositions;
    private Vector2 screenCenter;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (layers.Length != parallaxStrength.Length)
        {
            Debug.LogError("layers数组和parallax strengths数组长度不一致.");
            return;
        }
        
        initialPositions = new Vector3[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null)
            {
                initialPositions[i] = layers[i].position;
            }
        }
    }

    void Update()
    {
        //获取当前鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;

        //计算鼠标相对于屏幕中心的归一化偏移量（范围在 -1 到 1 之间）
        float offsetX = (mousePosition.x - screenCenter.x) / screenCenter.x;
        float offsetY = (mousePosition.y - screenCenter.y) / screenCenter.y;

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null)
            {
                //根据强度计算目标偏移量,这个偏移量是相对于初始位置的
                float targetX = initialPositions[i].x + (offsetX * parallaxStrength[i]) * (x ? 1 : 0);
                float targetY = initialPositions[i].y + (offsetY * parallaxStrength[i]) * (y ? 1 : 0);
                
                Vector3 targetPosition = new Vector3(targetX, targetY, initialPositions[i].z);
                
                layers[i].position = Vector3.Lerp(layers[i].position, targetPosition, Time.deltaTime * smoothing);
            }
        }
    }
}