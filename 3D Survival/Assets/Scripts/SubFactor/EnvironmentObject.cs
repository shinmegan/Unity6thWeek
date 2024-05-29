using UnityEngine;

public class EnvironmentObject : MonoBehaviour, IInspectable
{
    public EnvironmentData data;
    // 환경 정보 가져와서 표시하기
    public string GetInspectName()
    {
        string str = $"{data.objectName}";
        return str;
    }

    public string GetInspectDescription()
    {
        string str = $"{data.objectDescription}";
        return str;
    }
}
