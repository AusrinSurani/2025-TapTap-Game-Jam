using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionCtr : MonoBehaviour
{
    public GuideMask guideMask;
    private void Awake()
    {
        guideMask.Init();
    }

    public RectTransform introRect_1;
    public RectTransform introRect_2; 

    public void DoIntroMask(int targetIndex)
    {
        if(targetIndex==1)
        {
            introRect_1.gameObject.SetActive(true);
            //展示第一个引导
            guideMask.Play(introRect_1);
        }
        else if(targetIndex==2)
        {
            introRect_2.gameObject.SetActive(true);
            //展示第二个引导
            guideMask.Play(introRect_2);
        }
    }

    public void HideIntro_1()
    {
        //隐藏GuidMask，继续DanceGamePlay，同时不在冒引导提示
        guideMask.Close();
        introRect_1.gameObject.SetActive(false);

    }
    public void HideIntro_2()
    {
        //隐藏GuidMask，继续DanceGamePlay，同时不在冒引导提示
        guideMask.Close();
        introRect_2.gameObject.SetActive(false);
    }
}
