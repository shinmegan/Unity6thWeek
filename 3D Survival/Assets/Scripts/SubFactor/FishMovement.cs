using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float waitDuration = 1f; // 대기 시간
    private float moveDuration = 50f; // 이동 시간

    private Vector3 startPoint; // 시작 지점
    private Vector3 endPoint; // 도착 지점
    private Quaternion startRotation; // 시작 회전(90도)
    private Quaternion endRotation; // 도착 회전(-90도)

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moveDuration = Random.Range(30f, 50f);
        // 시작 지점과 도착 지점의 z 값을 -5.2와 3.2 사이에서 랜덤하게 설정
        float startX = Random.Range(-3.92f, -1.34f);
        float endX = Random.Range(-3.92f, -1.34f);

        startPoint = new Vector3(startX, -0.302f, 14.78f); // 시작 지점 초기화
        endPoint = new Vector3(startX, -0.302f, -3.89f); // 도착 지점 초기화

        startRotation = Quaternion.Euler(0, 90, 0); // 시작 회전 초기화
        endRotation = Quaternion.Euler(0, -90, 0); // 도착 회전 초기화

        transform.position = startPoint; // 물고기를 시작 지점으로 이동
        transform.rotation = startRotation; // 물고기의 회전을 시작 회전으로 설정

        StartCoroutine(MoveFish()); // 물고기 이동 시작
    }

    private IEnumerator MoveFish()
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
}
