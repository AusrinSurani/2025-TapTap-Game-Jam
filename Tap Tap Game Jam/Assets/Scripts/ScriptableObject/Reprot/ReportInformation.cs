using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Report Data", menuName = "Data/Report Data")]
public class ReportInformation : ScriptableObject
{
    [TextArea(10,5)]
    public string information;
    public List<string> options;
}