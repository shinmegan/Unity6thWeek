using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uICondition;

    Condition health { get { return uICondition.health;  } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition stamina { get { return uICondition.stamina; } }

    public float noHungerHealthDecay;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime); // 배고픔 상태 업데이트(감소)
        stamina.Add(stamina.passiveValue * Time.deltaTime); // 스테미나 상태 업데이트(증가)
        // 배고픔이 0이 되면, 체력이 감소하기 시작
        if(hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        // 체력이 0이 되면, 캐릭터 사망
        if(health.curValue == 0f)
        {
            Die();
        }
    }

    // 아이템 사용시 체력 회복하는 메서드
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 음식 섭취시 배고픔 회복하는 메서드
    public void Eat(float amount) 
    { 
        hunger.Add(amount); 
    }

    // 체력이 소진되면 사망하는 메서드
    public void Die()
    {
        Debug.Log("체력이 전부 소진되어 사망하였습니다.");
        Time.timeScale = 0f;
    }
}
