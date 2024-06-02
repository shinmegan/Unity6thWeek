using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvisibilityOn : MonoBehaviour
{
    public float duration = 10f; // 스킬 지속시간
    public GameObject InvisibilityImage;

    private void Start()
    {
        InvisibilityImage.SetActive(false); // 스킬 이미지 비활성화
    }

    // 무적 효과 이미지 실행 메서드
    public void OnInvisibility()
    {
        InvisibilityImage.SetActive(true);
        // 효과음 재생
        SoundManager.Instance.PlaySpeedUpSound();
        // 10초 후 무적 효과 종료
        StartCoroutine(SpeedContinueTime(duration));
    }

    // 무적 지속시간 코루틴
    private IEnumerator SpeedContinueTime(float duration)
    {
        yield return new WaitForSeconds(duration); // 스킬 지속 시간
        InvisibilityImage.SetActive(false); // 스킬 이미지 비활성화
    }
}
