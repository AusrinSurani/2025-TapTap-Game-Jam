using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{
    private IEnumerator _errorShowIE;
    public void SetMaterialWrongOnce()
    {
        if (_errorShowIE != null)
            StopCoroutine(_errorShowIE);
        _errorShowIE = ErrorShow();
        StartCoroutine(_errorShowIE);
    }

    private WaitForSeconds ErrorTime = new WaitForSeconds(2f);
    public Material errorMaterial;
    public Material selfMaterial;
    private IEnumerator ErrorShow()
    {
        selfMaterial.SetFloat("_Intensity", 0.1f);
        yield return ErrorTime;
        errorMaterial.SetFloat("_Intensity", 0f);
        selfMaterial.CopyPropertiesFromMaterial(errorMaterial);
    }
    public void End2()
    {
        UIManager.Instance.coverFader.gameObject.SetActive(true);
        GetComponentInParent<UI_Computer>().MoveBack(false);
        
        List<string> slist = new List<string>();
        
        slist.Add("在桥田缪的帮助下，你第一次未经上报离开了DTB的办公室——你知道自己不会再回去了。\r\n\r\n当天午夜，你收到了一条阅后即焚的坐标信息，发件者署名为“Shadow”。\r\n\r\n坐标指示的地方是曼庚市边缘的一个废弃工地，在你到达之前，桥田缪已经在人群的簇拥下独自站上高处。\n你在人群中看到了不少熟悉的身影，妮娜·奥蜜可、德尔塔·布莱梅……以及你曾经“治疗”过的很多位仿生人。\r\n\r\n在桥田缪的演讲中，你得知在场不仅有仿生人，还有一些桥田缪声称“可信任的人类”。\r\n\r\n这天过后，你们有了共同的目标——结束谎言。");
        
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(
            SceneLoadManager.SceneDisplayID.StartMenu, slist, true, true);
    }
}