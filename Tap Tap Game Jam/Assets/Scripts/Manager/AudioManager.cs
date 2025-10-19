using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class AudioManager : Singleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
        //记录默认音源数量
        if(bgmAudioSources!=null&&bgmAudioSources.Count>0)
            bgmASBaseCount = bgmAudioSources.Count;
        if (soundEffectAudioSources != null && soundEffectAudioSources.Count > 0)
            soundEffectASBaseCount = soundEffectAudioSources.Count;
        //默认配置
        globalVolumn = 1f;
        bgmVolumn = 1f;
        sfVolumn = 1f;
    }

    private void Update()
    { 
        //test
        if(Input.GetKeyDown(KeyCode.T))
        {
            /*AudioLoopPlay(bgmAudioPiece); 
            AudioLoopPlay(bgm2TestAudioPiece);
            AudioLoopPlay(bgm3TestAudioPiece);
            AudioLoopPlay(bgm3TestAudioPiece); */
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //AudioOncePlay(testAudioPiece);
        }

        //endtest

    }


    //一一对应
    public List<AudioPlayData> bgmAudiosPlayData = new List<AudioPlayData>();
    public List<AudioSource> bgmAudioSources=new List<AudioSource>(); 


    //use for search audios in use
    public List<AudioPlayData> soundEffectAudiosPlayData = new List<AudioPlayData>();
    public List<AudioSource> soundEffectAudioSources = new List<AudioSource>(); 

    //音源默认数量，用于GC前将额外轨道清除
    public int bgmASBaseCount;
    public int soundEffectASBaseCount;


    public GameObject audioSourcePrefab;

    public Transform bgmAudioSourceTransform;
    public Transform soundEffectAudioSourceTransform;


    //每次使用音效时更新各音源播放状态
    public void RefreshAudioSourceStatus()
    {
        for (int i = bgmAudiosPlayData.Count-1; i >=0; i--)
        {
            if (bgmAudiosPlayData[i].curStatus == AudioStatus.Playing
                || bgmAudiosPlayData[i].curStatus==AudioStatus.Loop)
            {
                //检查是否播放状态
                if (!bgmAudiosPlayData[i].source.isPlaying)
                {
                    bgmAudiosPlayData[i].curStatus = AudioStatus.End;
                    //重置该组数据 
                    bgmAudiosPlayData[i].source.clip = null;
                }
            }
        }

        for (int i = soundEffectAudiosPlayData.Count - 1; i >= 0; i--)
        {
            if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Playing
                /*|| soundEffectAudiosPlayData[i].curStatus == AudioStatus.Loop*/)
            {
                //检查是否播放状态
                if (!soundEffectAudiosPlayData[i].source.isPlaying)
                {
                    soundEffectAudiosPlayData[i].curStatus = AudioStatus.End;
                    //删除该组数据 
                    soundEffectAudiosPlayData[i].source.clip = null;
                }

            }
        }
    }

    public AudioPlayData GetFreeAudioSourceFromBGMChannel()
    {
        //刷新状态，将已播放完空置的AudioSource刷新
        RefreshAudioSourceStatus();
        //没有空轨道则新增轨道，GC时清除多余轨道
        for (int i=0;i<bgmAudiosPlayData.Count;i++)
        {
            if ( bgmAudiosPlayData[i].curStatus == AudioStatus.None || bgmAudiosPlayData[i].curStatus == AudioStatus.End)
                return bgmAudiosPlayData[i];
        }

        Debug.Log("add one audioSource");
        //无空余，新增一个并记录
        bgmAudioSources.Add(Instantiate(audioSourcePrefab, bgmAudioSourceTransform).GetComponent<AudioSource>());
        AudioPlayData addNewAudioPlayData = new AudioPlayData();
        addNewAudioPlayData.source = bgmAudioSources[bgmAudioSources.Count - 1];
        bgmAudiosPlayData.Add(addNewAudioPlayData); 
        return bgmAudiosPlayData[bgmAudiosPlayData.Count - 1];
    }

    public AudioPlayData GetFreeAudioSourceFromSoundEffectChannel()
    {
        //刷新状态，将已播放完空置的AudioSource刷新
        RefreshAudioSourceStatus();
        //没有空轨道则新增轨道
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.None || soundEffectAudiosPlayData[i].curStatus == AudioStatus.End)
                return soundEffectAudiosPlayData[i];
        }
        //无空余，新增一个并记录
        soundEffectAudioSources.Add(Instantiate(audioSourcePrefab, soundEffectAudioSourceTransform).GetComponent<AudioSource>());
        soundEffectAudiosPlayData.Add(new AudioPlayData()); 
        return soundEffectAudiosPlayData[soundEffectAudiosPlayData.Count - 1];
    }

    private GameObject _objForClear;
    public void ClearExtraSource()
    {
        int clearCount = bgmASBaseCount - bgmAudioSources.Count;
        //清理BGM多余轨道
        if (clearCount>0)
        { 
            for (int i = bgmAudioSources.Count - 1; i >= 0; i--)
            {
                //将不处于播放状态的多余轨道移除
                if (bgmAudiosPlayData[i].curStatus == AudioStatus.None || bgmAudiosPlayData[i].curStatus == AudioStatus.End)
                {
                    if (clearCount-- <= 0)
                        break;
                    _objForClear = bgmAudioSources[i].gameObject;
                    bgmAudioSources.RemoveAt(i);
                    bgmAudiosPlayData.RemoveAt(i); 
                    Destroy(_objForClear);
                }
            }
        }

        //清理SoundEffect音效多余轨道
        clearCount = soundEffectASBaseCount - soundEffectAudioSources.Count;
        if (clearCount > 0)
        {
            for (int i = soundEffectAudioSources.Count - 1; i >= 0; i--)
            {
                //将不处于播放状态的多余轨道移除
                if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.None || soundEffectAudiosPlayData[i].curStatus == AudioStatus.End)
                {

                    if (clearCount-- <= 0)
                        break;
                    _objForClear = soundEffectAudioSources[i].gameObject;
                    soundEffectAudioSources.RemoveAt(i);
                    soundEffectAudiosPlayData.RemoveAt(i); 
                    Destroy(_objForClear);
                }
            }
        }

    }

    #region 音源数据记录（播放状态，播放音乐片段）
    public enum AudioStatus
    {
        None,
        Playing,
        Pause,
        End,
        Loop
    }

    [System.Serializable]
    public class AudioPlayData
    {
        public AudioPlayData()
        {
            source = null;
            curStatus = AudioStatus.None;
            playingAuidoPiece = new AudioPiece();
        }
        public AudioSource source;
        public AudioStatus curStatus;
        public AudioPiece playingAuidoPiece;
    }

    [System.Serializable]
    public struct AudioPiece
    {
        public int audioID;
        public string audioClipDescription;
        public AudioClip sourceClip;
        public float volumnValue;
        //可在不同音源同时播放
        public bool bCanMutPlay;
    }
    #endregion

    /*public void AudioOncePlay(AudioClip audioClip,float volumnValue)
    { 
        //寻找空轨道 
        GetFreeAudioSourceFromSoundEffectChannel().PlayOneShot(audioClip, volumnValue);  
    }*/

    public void AudioOncePlay(AudioClip clip)
    {
        AudioPiece temp = new AudioPiece();
        temp.sourceClip = clip;
        temp.audioID = 1000;//默认临时音片
        temp.volumnValue = 1f;
        AudioOncePlay(temp);
    }

    public void AudioOncePlay(AudioPiece audioPiece)
    { 
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            //已占用播放音源
            if (soundEffectAudiosPlayData[i].playingAuidoPiece.audioID == audioPiece.audioID
                && !soundEffectAudiosPlayData[i].playingAuidoPiece.bCanMutPlay 
                )
            {
                //不可重复音效则覆盖原音轨重放
                soundEffectAudiosPlayData[i].source.Stop();
                soundEffectAudiosPlayData[i].source.volume = globalVolumn * sfVolumn * audioPiece.volumnValue;
                soundEffectAudiosPlayData[i].source.PlayOneShot(audioPiece.sourceClip);
                return;
            }
            //可重复音效则正常获取音轨
            else if (soundEffectAudiosPlayData[i].playingAuidoPiece.audioID == audioPiece.audioID
                && soundEffectAudiosPlayData[i].playingAuidoPiece.bCanMutPlay)
            {
                AudioPlayData tapd = GetFreeAudioSourceFromSoundEffectChannel();
                tapd.source.volume = globalVolumn * sfVolumn * audioPiece.volumnValue;
                tapd.source.PlayOneShot(audioPiece.sourceClip, audioPiece.volumnValue);
                tapd.curStatus = AudioStatus.Playing;
                tapd.playingAuidoPiece = audioPiece; 
                return;
            }
            else
            {
                //donothing
                //continue;
            }
        }
        //没有同id音效，则正常获取空音轨播放
        AudioPlayData newapd = GetFreeAudioSourceFromSoundEffectChannel();
        newapd.source.volume = globalVolumn * sfVolumn * audioPiece.volumnValue;
        newapd.source.PlayOneShot(audioPiece.sourceClip, audioPiece.volumnValue);
        newapd.curStatus = AudioStatus.Playing;
        newapd.playingAuidoPiece = audioPiece; 

    }
    /*
    public void AudioLoopPlay(AudioClip audioClip, float volumnValue)
    {
        AudioSource freeTargetAS = GetFreeAudioSourceFromBGMChannel();
        freeTargetAS.loop = true;
        freeTargetAS.clip = audioClip;
        freeTargetAS.volume = volumnValue;
        freeTargetAS.Play(); 
    }*/

    public void AudioLoopPlay(AudioPiece audioPiece)
    {
        AudioPlayData newapd = GetFreeAudioSourceFromBGMChannel(); 
        newapd.source.loop = true;
        newapd.source.clip = audioPiece.sourceClip;
        newapd.source.volume = globalVolumn*bgmVolumn* audioPiece.volumnValue;
        newapd.source.Play(); 
        newapd.curStatus = AudioStatus.Playing;
        newapd.playingAuidoPiece = audioPiece; 
    }


    public void PauseTargetAudioPiece(AudioPiece targetAP)
    {
        //找到对应音效,若为可同时播放音效，则全部暂停
        for(int i=0;i<bgmAudiosPlayData.Count;i++)
        {
            if (bgmAudiosPlayData[i].playingAuidoPiece.audioID != 0
                && targetAP.audioID == bgmAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (bgmAudiosPlayData[i].curStatus == AudioStatus.Playing
                    || bgmAudiosPlayData[i].curStatus == AudioStatus.Loop)
                {
                    bgmAudiosPlayData[i].source.Pause();
                    bgmAudiosPlayData[i].curStatus = AudioStatus.Pause;
                }
            }
        }
        for (int i=0;i<soundEffectAudiosPlayData.Count;i++)
        {
            if (soundEffectAudiosPlayData[i].playingAuidoPiece.audioID!=0
                && targetAP.audioID == soundEffectAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Playing )
                {
                    soundEffectAudiosPlayData[i].source.Pause();
                    soundEffectAudiosPlayData[i].curStatus = AudioStatus.Pause;
                } 
            }
        }
    }

    public void ClearTargetAudioPiece(AudioPiece targetAP)
    {
        //找到对应音效,若为可同时播放音效，则全部暂停
        for (int i = 0; i < bgmAudiosPlayData.Count; i++)
        {
            if (bgmAudiosPlayData[i].playingAuidoPiece.audioID != 0
                && targetAP.audioID == bgmAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (bgmAudiosPlayData[i].curStatus == AudioStatus.Playing
                    || bgmAudiosPlayData[i].curStatus == AudioStatus.Loop)
                {
                    bgmAudiosPlayData[i].source.Stop();
                    bgmAudiosPlayData[i].curStatus = AudioStatus.None;
                }
            }
        }
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            if (soundEffectAudiosPlayData[i].playingAuidoPiece.audioID != 0
                && targetAP.audioID == soundEffectAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Playing)
                {
                    soundEffectAudiosPlayData[i].source.Stop();
                    soundEffectAudiosPlayData[i].curStatus = AudioStatus.None;
                }
            }
        }
    }

    public void ResumeTargetAudioPiece(AudioPiece targetAP)
    {
        //找到对应音效,若为可同时播放音效，则全部暂停
        for (int i = 0; i < bgmAudiosPlayData.Count; i++)
        {
            if (bgmAudiosPlayData[i].playingAuidoPiece.audioID != 0
                && targetAP.audioID == bgmAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (bgmAudiosPlayData[i].curStatus == AudioStatus.Pause )
                {
                    bgmAudiosPlayData[i].source.Play();
                    bgmAudiosPlayData[i].curStatus = AudioStatus.Loop;
                }
            }
        }
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            if (soundEffectAudiosPlayData[i].playingAuidoPiece.audioID != 0
                && targetAP.audioID == soundEffectAudiosPlayData[i].playingAuidoPiece.audioID)
            {
                if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Pause)
                {
                    soundEffectAudiosPlayData[i].source.Play();
                    soundEffectAudiosPlayData[i].curStatus = AudioStatus.Playing;
                }
            }
        }
    }
    public void PauseAllAudio()
    {
        for(int i=0;i<bgmAudiosPlayData.Count;i++)
        {
            if (bgmAudiosPlayData[i].curStatus==AudioStatus.Playing
                || bgmAudiosPlayData[i].curStatus == AudioStatus.Loop)
            {
                bgmAudiosPlayData[i].source.Pause();
                bgmAudiosPlayData[i].curStatus = AudioStatus.Pause;
            }
        }
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Playing)
            {
                soundEffectAudiosPlayData[i].source.Pause();
                soundEffectAudiosPlayData[i].curStatus = AudioStatus.Pause;
            }
        }
    }

    public void ResumeAllAudio()
    {
        for (int i = 0; i < bgmAudiosPlayData.Count; i++)
        {

            if (bgmAudiosPlayData[i].curStatus == AudioStatus.Pause)
            {
                bgmAudiosPlayData[i].source.Play();
                bgmAudiosPlayData[i].curStatus = AudioStatus.Loop;
            }
        }
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {

            if (soundEffectAudiosPlayData[i].curStatus == AudioStatus.Pause)
            {
                soundEffectAudiosPlayData[i].source.Play();
                soundEffectAudiosPlayData[i].curStatus = AudioStatus.Playing;
            }
        }
    }

    private void RefreshBGMAudioVolumn()
    {
        for(int i=0;i<bgmAudiosPlayData.Count;i++)
        {
            bgmAudiosPlayData[i].source.volume = globalVolumn * bgmVolumn * bgmAudiosPlayData[i].playingAuidoPiece.volumnValue;
        } 
    }

    private void RefreshSFAudioVolumn()
    {
        for (int i = 0; i < soundEffectAudiosPlayData.Count; i++)
        {
            soundEffectAudiosPlayData[i].source.volume = globalVolumn * sfVolumn * soundEffectAudiosPlayData[i].playingAuidoPiece.volumnValue;
        } 
    }

    [Header("Settings")]
    private float globalVolumn;//全局音效音量系数
    private float bgmVolumn;//背景音量系数
    private float sfVolumn;//音效系数


    public float GlobalVolumn
    {
        get => globalVolumn;
        set 
        {
            globalVolumn = Mathf.Clamp(value, 0, 1f);
            RefreshBGMAudioVolumn();
            RefreshSFAudioVolumn();
        }
    }

    public float BgmVolumn
    {
        get => bgmVolumn;
        set
        {
            bgmVolumn = Mathf.Clamp(value, 0, 1f);
            RefreshBGMAudioVolumn();
        }
    }
    public float SfVolumn
    {
        get => sfVolumn;
        set
        {
            sfVolumn = Mathf.Clamp(value, 0, 1f);
            RefreshSFAudioVolumn();
        }
    }

    [Header("AudioClipPieces")]
    public AudioPiece mainMenuBtnAudioPiece;

    public AudioPiece bgmAudioPiece; 

    [Header("DancerDreamAudioPieces")]
    public AudioPiece sceneLightSwitchAudioPiece;
    public AudioPiece spotLightSwitchAudioPiece;

    public AudioPiece dancerBGMAudioPiece;
    [Header("DanceGamePlay")]
    public AudioPiece correctInteractAudioPiece;
    public AudioPiece specialInteractAudioPiece;
    public AudioPiece wrongInteractAudioPiece;
}
