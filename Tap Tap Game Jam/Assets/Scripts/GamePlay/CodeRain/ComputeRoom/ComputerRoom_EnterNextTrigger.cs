using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerRoom_EnterNextTrigger : MonoBehaviour
{

    //Show EnterGamePanel ?

    public void OnInteractWithComputerEnd()
    {
        //TODO:提示UI，确认以进入天台场景
        SceneLoadManager.Instance?.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.CoderDream,null,false);
    }
}
