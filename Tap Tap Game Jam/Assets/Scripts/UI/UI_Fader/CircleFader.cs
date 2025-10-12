using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CircleFader : MonoBehaviour
{
    public Image transitionImage;
    private Material transitionMaterial;

    public float transitionDuration; // 收缩动画的持续时间
    public float finalRadius;       // 最终留下的小圆的半径

    void Awake()
    {
        if (transitionImage == null)
        {
            transitionImage = GetComponent<Image>();
        }
        //获取材质实例，这样修改不会影响到项目中的原始材质文件
        transitionMaterial = transitionImage.material;

        transitionMaterial.SetFloat("_Radius", 1.5f);
    }

    public void StartTransition(Vector3 targetWorldPosition)
    {
        StartCoroutine(AnimateTransition(targetWorldPosition));
    }

    private IEnumerator AnimateTransition(Vector3 targetWorldPosition)
    {
        float elapsedTime = 0f;
        float startRadius = 1.5f; 

        while (elapsedTime < transitionDuration)
        {
            Vector2 screenPoint = Camera.main.WorldToViewportPoint(targetWorldPosition);
            transitionMaterial.SetVector("_Center", screenPoint);

            //随时间线性插值计算当前的半径和亮度
            float currentRadius = Mathf.Lerp(startRadius, finalRadius, elapsedTime / transitionDuration);

            //更新 Shader 属性
            transitionMaterial.SetFloat("_Radius", currentRadius);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionMaterial.SetFloat("_Radius", finalRadius);
    }

    [ContextMenu("Test Transition")]
    public void Test()
    {
        StartTransition(Vector3.zero); 
    }
}
