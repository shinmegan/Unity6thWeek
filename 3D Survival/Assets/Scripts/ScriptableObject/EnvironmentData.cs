using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "New Environment")]
public class EnvironmentData : ScriptableObject
{
    [Header("Info")]
    public string objectName; // 오브젝트 이름
    public string objectDescription; // 오브젝트 설명
}
