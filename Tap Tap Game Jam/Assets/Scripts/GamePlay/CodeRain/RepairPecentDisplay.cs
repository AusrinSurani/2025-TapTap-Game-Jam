using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairPecentDisplay : MonoBehaviour
{
    public CodeGamePlay parentGameplay;
    public TextMeshProUGUI repairPercentText;

    private void Update()
    {
        if(parentGameplay!=null)
        {
            repairPercentText.text = (parentGameplay.GetRepairPercent()*100f).ToString("F0")+"%";
        }
    }

}
