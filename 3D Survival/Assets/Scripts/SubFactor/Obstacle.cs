using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage; // 데미지 크기
    public float damageRate; // 몇 초마다 데미지를 줄 건지
    public float damageDistance = 2f; // 데미지를 입힐 거리

    // IDamagable 객체 저장 리스트
    List<IDamagable> things = new List<IDamagable>();

    // 플레이어의 Transform 저장
    Transform playerTransform;

    void Start()
    {
        // 플레이어 Transform 찾기
        playerTransform = CharacterManager.Instance.Player.transform;
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 거리 조건을 만족할 때만 데미지 적용
        if (distance <= damageDistance)
        {
            for (int i = 0; i < things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }
        }
    }

    // 장애물과 부딪힌 플레이어가 Idamagable 인터페이스를 가지고 있으면, 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable) && !things.Contains(damagable))
        {
            things.Add(damagable);
        }
    }

    // 이미 부딪혔으면, 리스트에서 삭제
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
