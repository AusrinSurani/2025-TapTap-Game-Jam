using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DancerDreamTest : MonoBehaviour
{
    public DanceGamePlay gamePlay;
    public TextMeshProUGUI roundShowText;


    public void BackToMenu()
    {
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.StartMenu,null,false);
    }
}
