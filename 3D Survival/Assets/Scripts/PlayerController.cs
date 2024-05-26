using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {   // 게임 시작 시 마우스 커서를 숨기고 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    // 플레이어 이동 처리 메서드
    void Move()
    {   // 방향설정: W(0,1)와 S(0,-1) + A(-1,0)와 D(1,0)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // 현재 y 속도 유지(점프하거나 중력에 의해 떨어지는 경우를 처리)

        _rigidbody.velocity = dir; // 새로운 속도 설정
    }

    // 카메라 회전 처리 메서드(카메라만 회전)
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // WSAD 입력을 받아오는 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed) // 키를 누르고 있을 때
        {
            curMovementInput = context.ReadValue<Vector2>(); // 이동 입력값 설정
        }
        else if(context.phase == InputActionPhase.Canceled) // 키를 뗐을 때
        {
            curMovementInput = Vector2.zero; // 이동 입력값 초기화
        }
    }

    // 마우스 정보를 받아오는 메서드
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
