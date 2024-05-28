using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage; // 데미지 크기
    public float damageRate; // 몇 초마다 데미지를 줄 건지

    // IDamagable 객체 저장 리스트
    List<IDamagable> things = new List<IDamagable>();

    void Start()
    {
        InvokeRepeating("DealDamage", 0 , damageRate);
    }

    void DealDamage()
    {
        for(int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // 캠프파이어와 부딪힌 오브젝트가 Idamagable 인터페이스를 가지고 있으면, 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
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
