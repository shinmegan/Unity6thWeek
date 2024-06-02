using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class SpeedUpSkill : MonoBehaviour
{
    public float manaCost = 30f; // 스킬 비용
    public float duration = 10f; // 스킬 지속시간
    public bool isSpeedUpSkillOn = false;
    public GameObject SpeedUpImage;

    private PlayerCondition condition;  // 플레이어 상태

    private void Start()
    {
        condition = CharacterManager.Instance.Player.condition;  // 상태 초기화
        SpeedUpImage.SetActive(false); // 스킬 이미지 비활성화
    }

    // Shift 입력을 받아오는 메서드
    public void OnSkill(InputAction.CallbackContext context)
    {   // 키를 눌렀을 때 마나가 충분하면, 스피드 업 + 마나 소모
        if (context.phase == InputActionPhase.Started)
        {
            if (condition.EnoughMana(manaCost))
            {
                isSpeedUpSkillOn = true;
                SpeedUpImage.SetActive(true);
                // 효과음 재생
                SoundManager.Instance.PlaySpeedUpSound();
                // 마나 소모
                condition.UseSkill(manaCost);
                // 10초 후 스피드업 효과 종료
                StartCoroutine(SpeedContinueTime(duration));
            }
            else
                Debug.Log("마나가 부족합니다.");

        }
    }

    // 스피드 지속시간 코루틴
    private IEnumerator SpeedContinueTime(float duration)
    {
        yield return new WaitForSeconds(duration); // 스킬 지속 시간
        SpeedUpImage.SetActive(false); // 스킬 이미지 비활성화
        isSpeedUpSkillOn = false;
    }
}
