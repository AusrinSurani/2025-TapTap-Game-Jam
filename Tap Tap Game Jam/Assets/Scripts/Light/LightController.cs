using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public List<Light2D>  lightsOfRoom;
    public List<Light2D>  lightsOfHotel;
    public Rigidbody2D playerRb;

    private bool haveTriggerDialog = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRb.velocity.x > 0)
            {
                if (!haveTriggerDialog)
                {
                    haveTriggerDialog = true;
                    playerRb.GetComponent<PlayerController>().initialDialogHaveDone = false;
                    AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.hotelBGM);
                }
                
                foreach (Light2D light2D in lightsOfRoom)
                {
                    light2D.intensity /= 10f;
                }

                foreach (Light2D light2D in lightsOfHotel)
                {
                    light2D.intensity *= 10f;
                }
            }
            else
            {
                foreach (Light2D light2D in lightsOfRoom)
                {
                    light2D.intensity *= 10f;
                }

                foreach (Light2D light2D in lightsOfHotel)
                {
                    light2D.intensity /= 10f;
                }
            }
        }
    }
}
