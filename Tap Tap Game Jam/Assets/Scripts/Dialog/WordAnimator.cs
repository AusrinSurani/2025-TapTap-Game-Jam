using UnityEngine;
using TMPro;

public class WordAnimator : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float rotateSpeed = 90f;
    public float fadeSpeed = 1f;
    public float lifetime = 1.5f;

    private TextMeshProUGUI textMesh;
    private RectTransform rectTransform;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        
        rectTransform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        
        Color currentColor = textMesh.color;
        currentColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = currentColor;
    }
}