using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uICondition;
    public PlayerController controller;

    Condition health { get { return uICondition.health;  } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition stamina { get { return uICondition.stamina; } }
    
    bool isJumpOn { get { return controller.isJumpOn; } } // 점프 키 눌렀을 때 true 반환

    public float onJumpStaminaDecay; // 점프 시 스테미나 감소되는 값
    public float noHungerHealthDecay; //배고픔 이후 체력 감소되는 값

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
        // 점프시 스태미나 소모
        if(isJumpOn && stamina.curValue > 0)
        {
            stamina.Subtract(onJumpStaminaDecay * Time.deltaTime); //Time.deltaTime = 0.2
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
