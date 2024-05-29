using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLocation : MonoBehaviour
{
    public float targetHeight; // 목표 높이
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 물고기에게 초기 힘을 가해 물 위로 올리기
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        // 현재 위치와 목표 높이의 차이 계산
        float heightDifference = targetHeight - transform.position.y;

        // 높이 차이에 비례하여 힘 가하기
        float buoyancyForce = heightDifference * 20f;
        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.VelocityChange);
    }
}
