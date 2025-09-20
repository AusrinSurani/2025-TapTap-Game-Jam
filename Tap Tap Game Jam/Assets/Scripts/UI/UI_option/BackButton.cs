using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("�¼��㲥")]
    public VoidEventSO optionToMenu;

    [Header("Q�����Ų���")] 
    public float animationDuration = 0.3f;
    public float hoverScaleMultiplier = 1.1f;
    public float clickScaleMultiplier = 0.9f;

    [Tooltip("�������Ŷ�����Q������")]
    public AnimationCurve bounceCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1.2f), // ��70%��ʱ��ﵽ1.2����С������Ŀ�꣩
        new Keyframe(1f, 1f)      // ��100%��ʱ��ص�1����С���ȶ���
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
        // ������ͣ����
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
        
        //�л�UI
        optionToMenu?.OnEventRaise();
    }

    /// <summary>
    /// ͳһ�����Ŷ�������
    /// </summary>
    /// <param name="targetScale">Ŀ������ֵ</param>
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