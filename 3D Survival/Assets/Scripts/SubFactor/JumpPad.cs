using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bouncePower = 10f; // 점프대에서 캐릭터가 튀어 오르는 힘

    private void OnCollisionEnter(Collision collision)
    {
        // 점프대에 닿은 객체가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(Vector2.up * bouncePower, ForceMode.Impulse);
        }
    }
}
