using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectForTest : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //UIManager.Instance.ShowPanelByPath<TestUIManagerPanel>("UI/TestUIManagerPanel");
            SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.DressingRoom);
        }
    }
}
