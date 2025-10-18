using UnityEngine;

[CreateAssetMenu(fileName = "New Config Data", menuName = "Data/ Dream System Config")]
public class DreamSystemConfig:ScriptableObject
{
    public SceneLoadManager.SceneDisplayID idOfSceneToLoad;
    public string nameOfPatient;
    public string keyWords;
    public string initialDiagnosis;
    public string advice;
}