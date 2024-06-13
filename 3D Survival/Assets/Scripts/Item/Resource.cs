using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive; // 제공할 아이템 데이터
    public int quantityPerHit = 1; // 타격당 획득량
    public int capacity = 5; // 자원 용량

    private PlayerCondition condition;  // 플레이어 상태
    private GameObject steak;
    private GameObject banana;

    private void Start()
    {
        condition = CharacterManager.Instance.Player.condition;  // 상태 초기화
    }

    private void Update()
    {
        steak = GameObject.Find("Item_Steak(Clone)");
        banana = GameObject.Find("Item_Banana(Clone)");
        if (steak != null && steak.transform.position.y <= 0)
        {
            steak.transform.position = new Vector3(steak.transform.position.x + 0.2f, steak.transform.position.y + 2f, steak.transform.position.z + 0.2f);
        }
        if (banana != null && banana.transform.position.y <= 0)
        {
            banana.transform.position = new Vector3(banana.transform.position.x + 0.2f, banana.transform.position.y + 2f, banana.transform.position.z + 0.2f);
        }
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            float stanimaDecay = condition.ResouceGatherStaminaDecay;
            //스태미나가 충분하다면, 자원 채집
            if (condition.EnoughStamina(stanimaDecay))
            {
                // 효과음 재생
                SoundManager.Instance.PlayAXSound();
                condition.UseStamina(stanimaDecay);
                if (capacity <= 0) break; // 용량 확인

                capacity -= 1; // 용량 감소
                Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up)); // 아이템 생성
            }
            else
                SoundManager.Instance.PlayWrongSound();
        }

        if (capacity <= 0)
        {
            capacity = 5; // 용량 초기화 (필요에 따라 초기 용량 값으로 설정)
            gameObject.SetActive(false); // 객체 비활성화
        }
    }

    public void Hunt(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break; // 용량 확인
            capacity -= 1; // 용량 감소
        }

        if (capacity <= 0)
        {
            gameObject.SetActive(false); // 객체 비활성화
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up)); // 아이템 생성
            capacity = 6; // 용량 초기화 (필요에 따라 초기 용량 값으로 설정)
        }
    }
}
