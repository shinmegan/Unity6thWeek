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

    private void Update()
    {   // 체력 15미만 경고음
        CheckHealthAndPlaySound();
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
        // 데미지 입는 효과음 재생
        SoundManager.Instance.DamageAudioSource.Play();
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

    // 체력이 1에서 20 사이면 효과음을 재생하는 메서드
    private void CheckHealthAndPlaySound()
    {
        AudioSource conditionAudioSource = SoundManager.Instance.conditionAudioSource;
        float currentHealth = CharacterManager.Instance.Player.condition.CurrentHealth();
        
        if (currentHealth >= 1 && currentHealth <= 15 && !conditionAudioSource.isPlaying)
        {
            conditionAudioSource.Play();
        }
        else if (currentHealth < 1 || currentHealth > 15)
            conditionAudioSource.Stop();
    }

}
