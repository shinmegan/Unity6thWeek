using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface IInteractable
{
    public string GetInteractName();        // 아이템 이름
    public string GetInteractDescription(); // 아이템 설명
    public void OnInteract();               // E 누르면 아이템 줍기
}

public interface IInspectable
{
    public string GetInspectName();         // 환경 요소 이름
    public string GetInspectDescription();  // 환경 요소 설명
}

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f; // 레이캐스트 거리
    public LayerMask layerMask; // 인터랙션 가능한 레이어 설정
    public TextMeshProUGUI objectNameText; // 오브젝트 이름을 표시할 UI 텍스트
    public TextMeshProUGUI objectDescriptionText; // 오브젝트 설명을 표시할 UI 텍스트
    public Image objectImg; // UI background 이미지

    private IInteractable curInteractable;
    private IInspectable Inspectable;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main; // 메인 카메라 가져오기
    }

    private void Update()
    {
        // 카메라 기준으로 정면으로 레이 발사
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 레이캐스트로 충돌 감지
        if (Physics.Raycast(ray, out hit, interactDistance, layerMask))
        {
            // 충돌한 오브젝트가 인스펙터 인터페이스를 가지고 있는지 확인
            Inspectable = hit.collider.GetComponent<IInspectable>();

            // 충돌한 오브젝트가 상호작용 인터페이스를 가지고 있는지 확인
            curInteractable = hit.collider.GetComponent<IInteractable>();

            if (Inspectable != null || curInteractable != null) // 둘 중 하나가 존재할 경우
            {
                if (Inspectable != null)
                    DisplayObjectInfo(Inspectable); // 환경 정보 표시
                else if(curInteractable != null)
                    SetItemInfo(curInteractable); // 아이템 정보 표시

            }
            else // 둘 다 존재하지 않을 경우
            {
                // 오브젝트 정보 초기화
                ClearObjectInfo();
            }
            
        }
        else // 레이가 아무 오브젝트와 충돌하지 않을 경우
        {
            // 오브젝트 정보 초기화
            ClearObjectInfo();
        }
    }

    // 텍스트 UI 초기화 메서드
    private void ClearObjectInfo()
    {
        objectNameText.text = ""; // 오브젝트 이름 텍스트 초기화
        objectDescriptionText.text = ""; // 오브젝트 설명 텍스트 초기화
        objectImg.gameObject.SetActive(false); // 배경이미지 비활성화
    }

    // 오브젝트 정보를 텍스트 UI에 표시하는 메서드 (환경요소)
    private void DisplayObjectInfo(IInspectable obj)
    {
        objectImg.gameObject.SetActive(true);
        objectNameText.text = obj.GetInspectName(); // 오브젝트 이름 표시
        objectDescriptionText.text = obj.GetInspectDescription(); // 오브젝트 설명 표시
    }

    // 아이템 정보를 UI에 표시하는 메서드 (아이템)
    private void SetItemInfo(IInteractable curInteractable)
    {
        objectImg.gameObject.SetActive(true); // 배경 이미지 활성화
        objectNameText.text = curInteractable.GetInteractName();
        objectDescriptionText.text = curInteractable.GetInteractDescription();  
    }

    // E 버튼 클릭 시, 인벤토리에 추가하는 메서드 실행
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract(); // 아이템 줍기
            curInteractable = null; // 상호작용 초기화
            ClearObjectInfo(); // 오브젝트 정보 초기화
        } 
    }
}
