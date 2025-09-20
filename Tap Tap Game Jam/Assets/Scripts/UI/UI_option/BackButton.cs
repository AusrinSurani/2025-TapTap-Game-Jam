using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("事件广播")]
    public VoidEventSO optionToMenu;

    [Header("Q弹缩放参数")] 
    public float animationDuration = 0.3f;
    public float hoverScaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 0.9f;

    [Tooltip("控制缩放动画的Q弹曲线")]
    public AnimationCurve bounceCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1.2f), // 在70%的时间达到1.2倍大小（超出目标）
        new Keyframe(1f, 1f)      // 在100%的时间回到1倍大小（稳定）
    );
    
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Coroutine currentAnimation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 播放悬停动画
        AnimateScale(originalScale * hoverScaleMultiplier);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(originalScale);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(ClickBounceRoutine());
        
        //切换UI
        optionToMenu?.OnEventRaise();
    }

    /// <summary>
    /// 统一的缩放动画方法
    /// </summary>
    /// <param name="targetScale">目标缩放值</param>
    private void AnimateScale(Vector3 targetScale)
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