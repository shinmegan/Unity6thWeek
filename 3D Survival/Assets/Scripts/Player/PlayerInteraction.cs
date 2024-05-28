using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f; // 레이캐스트 거리
    public LayerMask interactLayer; // 인터랙션 가능한 레이어 설정
    public TextMeshProUGUI objectNameText; // 오브젝트 이름을 표시할 UI 텍스트
    public TextMeshProUGUI objectDescriptionText; // 오브젝트 설명을 표시할 UI 텍스트

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main; // 메인 카메라 가져오기
    }

    private void Update()
    {
        // 카메라 기준으로 정면으로 레이 발사
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        // 레이캐스트로 충돌 감지
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 충돌한 오브젝트가 InspectableObject 스크립트를 가지고 있는지 확인
            InspectableObject inspectableObject = hit.collider.GetComponent<InspectableObject>();

            if (inspectableObject != null) // InspectableObject가 존재할 경우
            {
                // 오브젝트 정보 표시
                DisplayObjectInfo(inspectableObject);
            }
            else // InspectableObject가 존재하지 않을 경우
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

    // 오브젝트 정보를 텍스트 UI에 표시하는 메서드
    private void DisplayObjectInfo(InspectableObject obj)
    {
        objectNameText.text = obj.objectName; // 오브젝트 이름 표시
        objectDescriptionText.text = obj.objectDescription; // 오브젝트 설명 표시
    }

    // 텍스트 UI 초기화 메서드
    private void ClearObjectInfo()
    {
        objectNameText.text = ""; // 오브젝트 이름 텍스트 초기화
        objectDescriptionText.text = ""; // 오브젝트 설명 텍스트 초기화
    }
}
