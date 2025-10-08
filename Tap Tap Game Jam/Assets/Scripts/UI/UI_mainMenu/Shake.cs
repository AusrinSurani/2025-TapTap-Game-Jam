using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Shake : MonoBehaviour,IPointerClickHandler
{
    [Header("晃动效果")]
    [SerializeField] private float duration = 0.8f;

    [SerializeField] private float maxAngle = 25f;
    
    [SerializeField] private float frequency = 25f;
    
    [SerializeField]
    private AnimationCurve amplitudeCurve = new AnimationCurve(
        new Keyframe(0f, 1f, -2f, -2f),
        new Keyframe(0.7f, 1f, -1f, -1f),
        new Keyframe(1f, 0f, -2f, -2f) 
    );

    private Coroutine wobbleCoroutine;
    
    public void TriggerWobble()
    {
        if (wobbleCoroutine != null)
        {
            StopCoroutine(wobbleCoroutine);
        }
        wobbleCoroutine = StartCoroutine(Wobble());
    }

    private IEnumerator Wobble()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float normalizedTime = timer / duration;

            float amplitudeMultiplier = amplitudeCurve.Evaluate(normalizedTime);

            float currentAmplitude = maxAngle * amplitudeMultiplier;

            float angle = Mathf.Sin(timer * frequency) * currentAmplitude;

            transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        wobbleCoroutine = null;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerWobble();
    }
}