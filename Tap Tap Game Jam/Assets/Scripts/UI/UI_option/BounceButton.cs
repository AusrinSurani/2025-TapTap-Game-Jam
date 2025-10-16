using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BounceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Q弹参数")] 
    public float animationDuration = 0.3f;
    public float hoverScaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 0.9f;

    [Tooltip("Q弹曲线")]
    //默认参数
    public AnimationCurve bounceCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1.2f), 
        new Keyframe(1f, 1f)      
    );
    
    public RectTransform rectTransform;
    public Vector3 originalScale;
    private Coroutine currentAnimation;

    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
        }
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(originalScale * hoverScaleMultiplier);
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(originalScale);
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(ClickBounceRoutine());
    }
    
    protected void AnimateScale(Vector3 targetScale)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(ScaleRoutine(targetScale));
    }
    
    private IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = rectTransform.localScale;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            
            float curveValue = bounceCurve.Evaluate(t);
            rectTransform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, curveValue);

            yield return null;
        }
        
        rectTransform.localScale = targetScale;
        currentAnimation = null;
    }
    
    private IEnumerator ClickBounceRoutine()
    {
        Vector3 pressDownScale = originalScale * clickScaleMultiplier;
        Vector3 hoverScale = originalScale * hoverScaleMultiplier;

        yield return StartCoroutine(ScaleRoutine(pressDownScale));
        yield return StartCoroutine(ScaleRoutine(hoverScale));
    }
}