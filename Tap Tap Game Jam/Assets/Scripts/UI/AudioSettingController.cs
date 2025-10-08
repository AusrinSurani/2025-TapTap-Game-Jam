using UnityEngine;
using UnityEngine.UI; 
public class AudioSettingsController : MonoBehaviour
{
    [Header("音量滑块")]
    [SerializeField] private Slider bgmSlider; 

    [SerializeField] private Slider sfxSlider; 

    void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("无AudioManager");
            return;
        }
        
        bgmSlider.value = AudioManager.Instance.BgmVolumn;
        sfxSlider.value = AudioManager.Instance.SfVolumn;

        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    /// <summary>
    /// 当BGM滑块的值改变时，此方法被调用
    /// </summary>
    /// <param name="value">滑块的新值 (介于0.0和1.0之间)</param>
    public void OnBgmSliderChanged(float value)
    {
        AudioManager.Instance.BgmVolumn = value;
    }

    /// <summary>
    /// 当SFX滑块的值改变时，此方法被调用
    /// </summary>
    /// <param name="value">滑块的新值 (介于0.0和1.0之间)</param>
    public void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SfVolumn = value;
    }
    
    private void OnDestroy()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
        }
    }
}