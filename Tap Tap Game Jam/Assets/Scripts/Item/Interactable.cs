using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable:MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(DialogManager.Instance.dialogBox.activeSelf == true)
            return;
        
        Debug.Log(itemData.itemName);
        DialogManager.Instance.ShowMessage(itemData.description);
    }
}