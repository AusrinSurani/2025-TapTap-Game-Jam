using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Q弹参数")] 
    public float animationDuration = 0.3f;
    public float hoverScaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 0.9f;
    
    [Header("悬浮变化")]
    public Sprite hoverSprite;
    public Sprite defaultSprite;

    [Tooltip("Q弹曲线")]
    //默认参数
    public AnimationCurve bounceCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1.2f), 
        new Keyframe(1f, 1f)      
    );
    
    public Vector3 originalScale;
    private Coroutine currentAnimation;

    public virtual void Awake()
    {
        originalScale = transform.localScale;
        
        defaultSprite = GetDefaultSprite();
    }

    public virtual Sprite GetDefaultSprite()
    {
        return GetComponent<SpriteRenderer>()?.sprite;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(originalScale * hoverScaleMultiplier);
        if(hoverSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = hoverSprite;
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(originalScale);
        if (hoverSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(ClickBounceRoutine());
        if(defaultSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
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
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            
            float curveValue = bounceCurve.Evaluate(t);
            transform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, curveValue);

            yield return null;
        }
        
        transform.localScale = targetScale;
        currentAnimation = null;
    }
    
    private IEnumerator ClickBounceRoutine()
    {
        Vector3 pressDownScale = originalScale * clickScaleMultiplier;
        Vector3 hoverScale = originalScale * hoverScaleMultiplier;

        yield return StartCoroutine(ScaleRoutine(pressDownScale));
        yield return StartCoroutine(ScaleRoutine(hoverScale));
    }

    public void SetDefaultSprite(Sprite sprite)
    {
        defaultSprite = sprite;
    }
}