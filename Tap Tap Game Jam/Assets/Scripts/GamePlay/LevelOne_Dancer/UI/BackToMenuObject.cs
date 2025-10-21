using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuObject : MonoBehaviour
{
    public float waitSeconds;

    public void BackToConsoleRoom()
    {
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.ConsultationRoom, null, false);
    }

    public void BeginGoBack()
    {
        Invoke(nameof(BackToConsoleRoom), 3f);
    }
}
