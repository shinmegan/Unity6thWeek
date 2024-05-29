using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지 인터페이스
public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uICondition;

    Condition health { get { return uICondition.health;  } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition stamina { get { return uICondition.stamina; } }
    Condition mana { get { return uICondition.mana; } }

    bool isJumpOn { get { return CharacterManager.Instance.Player.controller.isJumpOn; } } // 점프 키 눌렀을 때 true 반환

    public float onJumpStaminaDecay; // 점프 시 스테미나 감소되는 값
    public float noHungerHealthDecay; // 배고픔 이후 체력 감소되는 값
    public bool isEnoughStamina = false; // 스태미나가 충분한지 확인
    public bool isDead = false; // 플레이어 사망 확인

    public event Action onTakeDamage;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime); // 배고픔 상태 업데이트(감소)
        stamina.Add(stamina.passiveValue * Time.deltaTime); // 스테미나 상태 업데이트(증가)
        EnoughStamina(onJumpStaminaDecay * 0.2f); // 점프 스테미나 충분한지 확인

        // 배고픔이 0이 되면, 체력이 감소하기 시작
        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        // 체력이 0이 되면, 캐릭터 사망
        if(health.curValue == 0f)
        {
            Die();
            isDead = true;
        }
        // 점프시 스태미나가 소모
        if(isJumpOn)
        {
            stamina.Subtract(onJumpStaminaDecay * Time.deltaTime);
        }
        // 스킬 사용시 마나 소모

    }

    // 아이템 사용시 체력 회복하는 메서드
    public void Heal(float amount, float duration)
    {
        StartCoroutine(HealOverTime(amount, duration));
    }

    // 마나 회복 아이템 사용시 마나를 회복하는 메서드
    public void RestoreMana(float amount, float duration)
    {
        StartCoroutine(RestoreManaOverTime(amount, duration));
    }

    // 음식 섭취시 배고픔 회복하는 메서드
    public void Eat(float amount, float duration)
    {
        StartCoroutine(EatOverTime(amount, duration));
    }


    // 스킬 사용 시 마나를 감소하는 메서드
    public void UseSkill(float manaCost)
    {
        if (mana.curValue >= manaCost)
        {
            mana.Subtract(manaCost);
        }
        else
        {
            Debug.Log("마나가 부족합니다.");
        }
    }

    // 체력이 소진되면 사망하는 메서드
    public void Die()
    {
        Debug.Log("체력이 전부 소진되어 사망하였습니다.");
        Time.timeScale = 0f;
    }

    // 데미지 인터페이스
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke(); // 이벤트에 등록된 메서드 호출
    }

    // 스테미나 충분한 지 확인 하는 메서드
    public bool EnoughStamina(float decay)
    {
        return isEnoughStamina = stamina.curValue > decay;
    }

    // 아이템 사용시 체력 회복하는 코루틴
    private IEnumerator HealOverTime(float amount, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            health.Add(amount * (Time.deltaTime / duration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // 마나 회복 아이템 사용시 마나를 회복하는 코루틴
    private IEnumerator RestoreManaOverTime(float amount, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            mana.Add(amount * (Time.deltaTime / duration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // 음식 섭취시 배고픔 회복하는 코루틴
    private IEnumerator EatOverTime(float amount, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            hunger.Add(amount * (Time.deltaTime / duration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
