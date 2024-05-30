using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip; // 현재 장착된 장비
    private Transform equipPosition; // 장비 객체 위치

    private PlayerController controller; // 플레이어 컨트롤러
    private PlayerCondition condition; // 플레이어 상태

    // 초기화 함수
    void Start()
    {
        equipPosition = gameObject.transform;
        controller = CharacterManager.Instance.Player.controller; // 플레이어 컨트롤러 참조
        condition = CharacterManager.Instance.Player.condition; // 플레이어 상태 참조
    }

    // 새로운 장비 장착 함수
    public void EquipNew(ItemData data)
    {
        UnEquip(); // 기존 장비 해제
        curEquip = Instantiate(data.equipPrefab, equipPosition).GetComponent<Equip>(); // 새로운 장비 인스턴스 생성 및 장착
    }

    // 장비 해제 함수
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject); // 현재 장착된 장비 파괴
            curEquip = null; // 현재 장착된 장비 null로 설정
        }
    }

    // 공격 입력 처리 함수
    public void OnAttackInput(InputAction.CallbackContext context)
    {   // 마우스 왼쪽 버튼 클릭 + 인벤토리창 비활성화
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput(); // 장비의 공격 입력 처리 호출
        }
    }
}
