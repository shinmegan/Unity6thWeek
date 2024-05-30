using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive; // 제공할 아이템 데이터
    public int quantityPerHit = 1; // 타격당 획득량
    public int capacity = 5; // 자원 용량

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break; // 용량 확인

            capacity -= 1; // 용량 감소
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up)); // 아이템 생성
        }

        if (capacity <= 0)
        {
            capacity = 5; // 용량 초기화 (필요에 따라 초기 용량 값으로 설정)
            gameObject.SetActive(false); // 객체 비활성화
        }
    }
}
