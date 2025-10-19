using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj_DressingRoom : MonoBehaviour
{
    
    public void MoveToDanceDreamScene()
    {
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.DanceDream,null,false);
    }
}
