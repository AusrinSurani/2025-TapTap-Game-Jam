using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : InteractableInDream
{
    public GameObject consultingNote;
    public GameObject newsBulletin;
    
    public void ShowConsultingNote()
    {
        consultingNote.SetActive(true);
    }

    public void HideConsultingNote()
    {
        consultingNote.SetActive(false);
    }

    public void ShowNewsBulletin()
    {
        newsBulletin.SetActive(true);
    }

    public void HideNewsBulletin()
    {
        newsBulletin.SetActive(false);
    }
}
