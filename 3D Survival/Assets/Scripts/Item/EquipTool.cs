using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate; // 공격 주기
    private bool attacking; // 공격 중 여부
    public float attackDistance; // 공격 거리

    [Header("Resource Gathering")]
    public bool doesGatherResources; // 자원 채집 여부

    [Header("Combat")]
    public bool doesDealDamage; // 피해 입히기 여부
    public int damage; // 피해량

    private Animator animator; // 애니메이터
    private new Camera camera; // 카메라


    private void Awake()
    {
        camera = Camera.main; // 메인 카메라 참조
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 참조
    }

    // 공격 입력 처리 함수(애니메이션 동작)
    public override void OnAttackInput()
    {
        if (!attacking) // 공격 중이 아닐 때
        {
            attacking = true; // 공격 중으로 설정
            animator.SetTrigger("Attack"); // 공격 애니메이션 트리거 설정
            Invoke("OnCanAttack", attackRate); // 일정 시간 후 다시 공격 가능하게 설정
        }
    }

    // 공격 가능 상태로 변경하는 함수
    void OnCanAttack()
    {
        attacking = false; // 공격 중이 아님으로 설정
    }

}
