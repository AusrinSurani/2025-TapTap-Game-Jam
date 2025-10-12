using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI; 
public class AudioSettingsController : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO quitGameEvent;
    
    [Header("音量滑块")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    
    const string AUDIO_SETTING_DATA_FILE = "AudioSettingData.txet";

    private void OnEnable()
    {
        quitGameEvent.OnEventRaise += SaveAudioSettingData;
    }

    private void OnDisable()
    {
        quitGameEvent.OnEventRaise -= SaveAudioSettingData;
    }

    void Start()
    {
        if(File.Exists(Path.Combine(Application.persistentDataPath,AUDIO_SETTING_DATA_FILE)))
        {
            LoadAudioSettingData();
            AudioManager.Instance.BgmVolumn = bgmSlider.value;
            AudioManager.Instance.SfVolumn =  sfxSlider.value;
        }
        else
        {
            //第一次玩没有数据，准备默认的数据
            SetDefaultAudioSettingData();
            AudioManager.Instance.BgmVolumn = bgmSlider.value;
            AudioManager.Instance.SfVolumn =  sfxSlider.value;
        }
        
        if (AudioManager.Instance == null)
        {
            Debug.LogError("无AudioManager单例");
            return;
        }

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
    
    #region 音效设置数据与存储
    [Serializable] class AudioSettingData
    {
        public float BGMVolume;
        public float SFXVolume;
    }

    public void SetDefaultAudioSettingData()
    {
        var data = new AudioSettingData();
        data.BGMVolume = 0.8f;
        data.SFXVolume = 0.8f;
        
        SaveSystem.SaveByJson(AUDIO_SETTING_DATA_FILE, data);
    }
    
    public void SaveAudioSettingData()
    {
        var data = new AudioSettingData();
        data.BGMVolume = bgmSlider.value;
        data.SFXVolume = sfxSlider.value;
        
        SaveSystem.SaveByJson(AUDIO_SETTING_DATA_FILE, data);
    }

    void LoadAudioSettingData()
    {
        var savedData = SaveSystem.LoadFromJson<AudioSettingData>(AUDIO_SETTING_DATA_FILE);
        bgmSlider.value = savedData.BGMVolume;
        sfxSlider.value = savedData.SFXVolume;
    }

    //方便操作
    [UnityEditor.MenuItem("Developer/Delete Audio Setting")]
    public static void DeleteAudioSetting()
    {
        SaveSystem.DeleteSavedFile(AUDIO_SETTING_DATA_FILE);
    }

    #endregion
}