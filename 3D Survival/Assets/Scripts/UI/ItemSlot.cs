using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;             

    public Button button;              
    public Image icon;                 
    public TextMeshProUGUI quantityText;

    public UIInventory inventory;      // 인벤토리 참조

    private Outline outline;           // 슬롯을 강조하는 아웃라인 컴포넌트


    public int index;                  // 인벤토리에서 슬롯의 인덱스
    public bool equipped;              // 아이템이 장착 중인지 여부
    public int quantity;               // 슬롯에 있는 아이템의 수량


    private void Awake()
    {
        outline = GetComponent<Outline>();
    }


    private void OnEnable()
    {
        if (outline != null)
        {
            outline.enabled = equipped; // 장착 시 아웃라인 활성화
        }
    }

    // 아이템 데이터를 사용하여 슬롯을 설정하는 메서드
    public void Set()
    {
        if (item != null)
        {
            icon.gameObject.SetActive(true); // 흰색 배경 이미지
            icon.sprite = item.icon; // 아이콘 이미지
            quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

            if (outline != null)
            {
                outline.enabled = equipped;
            }
        }
    }

    // 슬롯을 초기화하는 메서드
    public void Clear()
    {
        item = null;
        if(icon.gameObject != null)
        {
            icon.gameObject.SetActive(false);
        }
        quantityText.text = string.Empty;
    }

    // 버튼 클릭 이벤트를 처리하는 메서드
    public void OnClickButton()
    {
        if (inventory != null)
        {
            inventory.SelectItem(index);
        }
    }
}
