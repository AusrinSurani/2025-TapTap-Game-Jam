using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterClickTrigger : MonoBehaviour,IPointerClickHandler
{

    public TextAsset startDialogue;
    public CodeGame_SceneController sceneController;

    public bool BCanInteract;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        //
        if (BCanInteract)
        {
            DialogManager.Instance?.StartDialog(startDialogue);
            DialogManager.Instance.OnDialogueClose.AddListener(OnDialogueEnd);
            BCanInteract = false;
        }

    }

    public void OnDialogueEnd()
    {
        sceneController.BeginEnterGameShow();
        DialogManager.Instance.OnDialogueClose.RemoveListener(OnDialogueEnd);
    }

    private void Update()
    { 
    }
}
