using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public float fadeDuration = 1F;
    public FadeType currentFadeType;

    private int fadeAmount = Shader.PropertyToID("_FadeAmount");
    
    private int useShutters = Shader.PropertyToID("_UseShutter");
    
    private int useRadiaWipe = Shader.PropertyToID("_UseWipe");
    
    private int usePlainBlack = Shader.PropertyToID("_UsePlainBlack");
    
    private int useGoop = Shader.PropertyToID("_UseGoop");

    private int? lastEffect;

    private Image image;
    private Material material;

    public enum FadeType
    {
        Shutter,
        RadiaWipe,
        PlainBlack,
        Goop
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        
        Material mat = image.material;
        image.material = new Material(mat);
        material = image.material;

        lastEffect = useShutters;
    }

    private void Update()
    { 
        //test
       if (Input.GetKeyDown(KeyCode.Q))
        {  
            Debug.Log("FadeIn");
            FadeIn(currentFadeType);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("FadeOut");
            FadeOut(currentFadeType);
        }
    }

    public void FadeOut(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeOut();
    }

    public void FadeIn(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeIn();
    }

    private void ChangeFadeEffect(FadeType fadeType)
    {
        if (lastEffect.HasValue)
        {
            material.SetFloat(lastEffect.Value, 0F);
        }

        switch (fadeType)
        {
            case FadeType.Shutter:
                SwitchEffect(useShutters);
                break;
            case FadeType.RadiaWipe:
                SwitchEffect(useRadiaWipe);
                break;
            case FadeType.PlainBlack:
                SwitchEffect(usePlainBlack);
                break;
            case FadeType.Goop:
                SwitchEffect(useGoop);
                break;
        }
    }
    
    
    private void SwitchEffect(int effectToTurnOn)
    {
        material.SetFloat(effectToTurnOn, 1F);
        lastEffect = effectToTurnOn;
    }
    
    private void StartFadeOut()
    {
        material.SetFloat(fadeAmount, 0F);

        StartCoroutine(HandleFade(1F, 0F));
    }

    private void StartFadeIn()
    {
        material.SetFloat(fadeAmount, 1F);
        StartCoroutine(HandleFade(0F, 1F));
    }
    
    private IEnumerator HandleFade(float targetAmount, float startAmount)
    { 
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float lerpAmount = Mathf.Lerp(startAmount, targetAmount, elapsedTime / fadeDuration);
            material.SetFloat(fadeAmount, lerpAmount);
            
            yield return null;
        }
        
        material.SetFloat(fadeAmount, targetAmount); 
    }
}
