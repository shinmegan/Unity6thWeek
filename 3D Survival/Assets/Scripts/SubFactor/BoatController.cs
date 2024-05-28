using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Vector3 startPoint = new Vector3(1.58f, -0.22f, 1.55f); // 시작 지점
    private Vector3 endPoint = new Vector3(-5.5f, -0.22f, 1.55f); // 도착 지점
    public float moveDuration = 5f; // 이동 시간
    public float waitDuration = 5f; // 대기 시간

    private Quaternion startRotation; // 시작 회전(90도)
    private Quaternion endRotation; // 도착 회전(-90도)
    private Rigidbody rb;
    private Transform playerTransform; // 플레이어의 Transform

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startRotation = Quaternion.Euler(0, 90, 0); // 시작 회전 초기화
        endRotation = Quaternion.Euler(0, -90, 0); // 도착 회전 초기화

        transform.position = startPoint; // 보트를 시작 지점으로 이동
        transform.rotation = startRotation; // 보트의 회전을 시작 회전으로 설정

        StartCoroutine(MoveBoat()); // 보트 이동 시작
    }

    private IEnumerator MoveBoat()
    {
        while (true)
        {
            // 도착 지점으로 이동
            yield return StartCoroutine(MoveToPosition(endPoint, endRotation));
            yield return new WaitForSeconds(waitDuration); // 대기 시간

            // 시작 지점으로 이동
            yield return StartCoroutine(MoveToPosition(startPoint, startRotation));
            yield return new WaitForSeconds(waitDuration); // 대기 시간
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 initialPosition = transform.position; // 초기 위치
        float elapsedTime = 0; // 경과 시간

        while (elapsedTime < moveDuration)
        {
            rb.MovePosition(Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration)); // 위치 보간
            elapsedTime += Time.deltaTime; // 경과 시간 갱신
            yield return null; // 다음 프레임까지 대기
        }

        rb.MovePosition(targetPosition); // 최종 위치 설정
        rb.MoveRotation(targetRotation); // 최종 회전 설정
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform; // 플레이어의 Transform 저장
            Debug.Log("탑승 완료");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null; // 플레이어의 Transform 해제
            Debug.Log("탑승 해제");
        }
    }
}
