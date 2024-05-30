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

    [Header("Dangerous Resource Gathering")]
    public bool isDangerousResource; // 위험한 자원 여부

    private Animator animator; // 애니메이터
    private new Camera camera; // 카메라

    private int TreeLayer; // "Tree" 레이어
    private int MushroomLayer; // "Mushroom" 레이어

    private void Awake()
    {
        camera = Camera.main; // 메인 카메라 참조
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 참조
        TreeLayer = LayerMask.NameToLayer("Tree"); // "GreenTree" 레이어 가져오기
        MushroomLayer = LayerMask.NameToLayer("Mushroom");
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

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 화면 중앙에서의 레이 생성
        RaycastHit hit; // 레이캐스트 충돌 정보 저장

        if (Physics.Raycast(ray, out hit, attackDistance)) // 레이캐스트 실행 및 충돌 확인
        {
            if (doesGatherResources && hit.collider.gameObject.layer == TreeLayer && hit.collider.TryGetComponent(out Resource resource))
            {
                // 자원 채집
                resource.Gather(hit.point, hit.normal);
            }

            if (isDangerousResource && hit.collider.gameObject.layer == MushroomLayer && hit.collider.TryGetComponent(out Resource dangerousResource))
            {
                // 위험한 자원에 대한 추가 처리
                dangerousResource.Gather(hit.point, hit.normal); // 위험한 자원 채집
            }

            if((doesGatherResources && hit.collider.gameObject.layer != TreeLayer) || (isDangerousResource && hit.collider.gameObject.layer != MushroomLayer) )
            {
                Debug.Log("적합한 도구를 장착해주세요. 나무->도끼 , 버섯->검");
            }
        }
    }
}
