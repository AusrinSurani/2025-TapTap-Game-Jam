using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPanel : MonoBehaviour
{


    public string PanelPath;
    public BaseUIPanel ParentPanel; 


    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnPause()
    {  
    }

    public virtual void OnResume()
    {

    }
}
