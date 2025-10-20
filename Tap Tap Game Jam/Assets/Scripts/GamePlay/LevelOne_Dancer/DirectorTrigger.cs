using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorTrigger : MonoBehaviour
{
    public PlayableDirector danceDirector;
   /* public bool BDirectorHaveStart;
    private void Update()
    {
        if(Input.GetAxis("Horizontal")!=0&& !BDirectorHaveStart)
        {
            BDirectorHaveStart = true;
            danceDirector.Play();
            
        }
    }*/

    private void Start()
    {
        Invoke(nameof(StartDirector), 2f);
    }

    private void StartDirector()
    {
        danceDirector.Play();
    }
}
