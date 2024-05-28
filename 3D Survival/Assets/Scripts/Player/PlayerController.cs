using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    bool isEnoughStamina { get { return CharacterManager.Instance.Player.condition.isEnoughStamina; } } // 스태미나가 충분하면 true 반환
    public Transform _transform;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Jump")]
    public float jumpPower;
    public bool isJumpOn = false;

    [Header("Look")]
    public Transform cameraContainer; // 회전할 오브젝트
    public float minXLook; // 회전 최소값
    public float maxXLook; // 회전 최대값
    private float camCurXRot; // 현재 카메라 X축 회전 값
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    [Header("Settings")]
    public GameObject settingsMenu; // 설정창 GameObject

    private bool isSettingsOpen = false; // 설정창 활성화 상태를 관리하는 변수

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {   // 게임 시작 시 마우스 커서를 숨기고 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        _transform = GetComponent<Transform>(); 
        if (!isSettingsOpen) // 설정창이 열려있지 않을 때만 이동 처리
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (!isSettingsOpen) // 설정창이 열려있지 않을 때만 카메라 회전 처리
        {
            CameraLook();
        }
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

    // Space 입력을 받아오는 메서드
    public void OnJump(InputAction.CallbackContext context)
    {
        // 스페이스 입력 + 지면 + 스테미나 충분
        if(context.phase == InputActionPhase.Started && IsGrounded() && isEnoughStamina)
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            // 점프 가능
            isJumpOn = true;
        }
        else if (context.phase == InputActionPhase.Canceled
                || context.phase == InputActionPhase.Waiting
                || (context.phase == InputActionPhase.Performed && !IsGrounded())
                || (context.phase == InputActionPhase.Started && !IsGrounded()))
        {
            isJumpOn = false;
        }
    }

    // 땅에 붙어있는지 확인하는 메서드
    bool IsGrounded()
    {   // 플레이어의 네 귀퉁이에서 아래로 향하는 4개의 레이캐스트 생성
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.7f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    // 키보드 1 입력을 받아오는 메서드
    public void OnSetting(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && !isSettingsOpen) // 키를 눌렀을 때
        {
            // 설정창 띄우기
            // 마우스 커서가 보이고 카메라 회전 막기
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isSettingsOpen = true;
        }
        else if (context.phase == InputActionPhase.Started && isSettingsOpen ) // 키를 한 번 더 눌렀을 때
        {
            // 설정창 감추기
            // 마우스 커서 감추고 카메라 회전 시작
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isSettingsOpen = false;
        }
    }
}
