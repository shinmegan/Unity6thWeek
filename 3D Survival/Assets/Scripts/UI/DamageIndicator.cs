using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed; // 깜빡이는 속도

    private Coroutine coroutine;

    void Start()
    {   // 이벤트에 Flash 메서드 등록
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {   // 코루틴이 이미 실행 중이라면 중지
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true; // 이미지 켜기
        image.color = new Color(1f, 89f / 255f, 89f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.21f; // 이미지의 초기 알파값
        float a = startAlpha;

        while(a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 89f / 255f, 89f / 255f, a);
            yield return null;
        }

        image.enabled = false; // 이미지 끄기
    }

}
