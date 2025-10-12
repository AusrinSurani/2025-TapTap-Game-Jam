using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable:MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(itemData.itemName);
        DialogManager.Instance.ShowMessage(itemData.description);
    }
}