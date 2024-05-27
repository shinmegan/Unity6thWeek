
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; // 현재 값
    public float startValue;
    public float maxValue;
    public float passiveValue; // 시간에 따라 주기적으로 변하는 값(Add)
    public Image uiBar;

    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // UI업데이트
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {   // Mathf.Min(A, B) A, B 중 더 작은 값을 반환
        curValue = Mathf.Min(curValue + value, maxValue);  
    }

    public void Subtract(float value)
    {   // Mathf.Max(A, B) A, B 중 더 큰 값을 반환
        curValue = Mathf.Max(curValue - value, maxValue);
    }
}
