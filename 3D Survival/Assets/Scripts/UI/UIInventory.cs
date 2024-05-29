using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;  // 슬롯 배열

    public GameObject inventoryWindow;  // 인벤토리 창
    public Transform slotPanel;  // 슬롯 패널

    private ItemSlot selectedItem;  // 선택된 아이템 슬롯
    private int selectedItemIndex;  // 선택된 아이템 인덱스

    public TextMeshProUGUI selectedItemName;  // 선택된 아이템 이름
    public TextMeshProUGUI selectedItemDescription;  // 선택된 아이템 설명
    public TextMeshProUGUI selectedItemStatName;  // 선택된 아이템 스탯 이름
    public TextMeshProUGUI selectedItemStatValue;  // 선택된 아이템 스탯 값
    public GameObject useButton;  // 사용 버튼
    public GameObject equipButton;  // 장착 버튼
    public GameObject unequipButton;  // 장착 해제 버튼
    public GameObject dropButton;  // 드롭 버튼

    private int curEquipIndex;  // 현재 장착 인덱스

    private PlayerController controller;  // 플레이어 컨트롤러
    private PlayerCondition condition;  // 플레이어 상태
    private Transform dropPosition;  // 아이템 드롭 위치

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;  // 컨트롤러 초기화
        condition = CharacterManager.Instance.Player.condition;  // 상태 초기화
        dropPosition = CharacterManager.Instance.Player.dropPosition;  // 드롭 위치 초기화

        controller.inventory += Toggle;  // 인벤토리 토글 이벤트
        CharacterManager.Instance.Player.addItem += AddItem;  // 아이템 이벤트 등록

        inventoryWindow.SetActive(false);  // 인벤토리 창 비활성화
        slots = new ItemSlot[slotPanel.childCount];  // 슬롯 배열 초기화

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();  // 슬롯 컴포넌트 설정
            slots[i].index = i;  // 슬롯 인덱스 설정
            slots[i].inventory = this;  // 인벤토리 참조 설정
            slots[i].Clear();  // 슬롯 초기화
        }

        ClearSelectedItemWindow();  // 선택된 아이템 창 초기화
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false );
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;  // 인벤토리 창 활성화 여부
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;  // 아이템 데이터 가져오기

        // 아이템이 중복 가능한지 canStack 체크
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);  // 쌓을 수 있는 아이템 슬롯 가져오기
            if (slot != null) // 슬롯이 null이 아니라면,
            {
                slot.quantity++;  // 아이템 수량 증가
                UpdateUI();  // UI 업데이트
                CharacterManager.Instance.Player.itemData = null;  // 아이템 데이터 초기화
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();  // 빈 슬롯 가져오기

        if (emptySlot != null)
        {
            emptySlot.item = data;  // 아이템 설정
            emptySlot.quantity = 1;  // 아이템 수량 설정
            UpdateUI();  // UI 업데이트
            CharacterManager.Instance.Player.itemData = null;  // 아이템 데이터 초기화
            return;
        }
        // 빈 슬롯이 없다면,
        ThrowItem(data);  // 아이템 버리기
        CharacterManager.Instance.Player.itemData = null;  // 아이템 데이터 초기화
    }


    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {   // 슬롯에 데이터가 있다면
            if (slots[i].item != null)
            {
                slots[i].Set();  // 슬롯 설정
            }
            else
            {
                slots[i].Clear();  // 슬롯 초기화
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {   // 넣으려는 데이터가 같고, 최대 수량보다 적다면
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];  // 쌓을 수 있는 슬롯 반환
            }
        }
        return null;
    }
    
    // 비어있는 슬롯 가져오는 메서드
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];  // 빈 슬롯 반환
            }
        }
        return null;
    }
    // 아이템 버리는 메서드
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));  // 아이템 인스턴스화
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return; // 인덱스 아이템이 null이면 return

        selectedItem = slots[index];  // 선택된 아이템 설정
        selectedItemIndex = index;  // 선택된 아이템 인덱스 설정

        selectedItemName.text = selectedItem.item.displayName;  // 선택된 아이템 이름 설정
        selectedItemDescription.text = selectedItem.item.description;  // 선택된 아이템 설명 설정

        selectedItemStatName.text = string.Empty;  // 선택된 아이템 스탯 이름 초기화
        selectedItemStatValue.text = string.Empty;  // 선택된 아이템 스탯 값 초기화

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";  // 스탯 이름 추가
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";  // 스탯 값 추가
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);  // 사용 버튼 활성화
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);  // 장착 버튼 활성화
        unequipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);  // 장착 해제 버튼 활성화
        dropButton.SetActive(true);  // 드롭 버튼 활성화
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;  // 선택된 아이템 초기화

        selectedItemName.text = string.Empty;  // 선택된 아이템 이름 초기화
        selectedItemDescription.text = string.Empty;  // 선택된 아이템 설명 초기화
        selectedItemStatName.text = string.Empty;  // 선택된 아이템 스탯 이름 초기화
        selectedItemStatValue.text = string.Empty;  // 선택된 아이템 스탯 값 초기화

        useButton.SetActive(false);  // 사용 버튼 비활성화
        equipButton.SetActive(false);  // 장착 버튼 비활성화
        unequipButton.SetActive(false);  // 장착 해제 버튼 비활성화
        dropButton.SetActive(false);  // 드롭 버튼 비활성화
    }
    // 사용하기 버튼 메서드
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                ItemDataConsumable consumable = selectedItem.item.consumables[i];
                switch (consumable.type)
                {
                    case ConsumableType.Health:
                        condition.Heal(consumable.value, consumable.duration); break; // 일정 시간 체력 회복
                    case ConsumableType.Hunger:
                        condition.Eat(consumable.value, consumable.duration); break;  // 일정 시간 배고픔 회복
                    case ConsumableType.Mana:
                        condition.RestoreMana(consumable.value, consumable.duration); break;  // 일정 시간 마나 회복
                }
            }
            RemoveSelectedItem();  // 선택된 아이템 제거
        }
    }

    // 버리기 버튼 메서드
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);  // 아이템 버리기
        RemoveSelectedItem();  // 선택된 아이템 제거
    }

    // 버린 아이템 제거 메서드
    void RemoveSelectedItem()
    {
        selectedItem.quantity--;  // 선택된 아이템 수량 감소

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);  // 장착 해제
            }

            selectedItem.item = null;  // 아이템 초기화
            ClearSelectedItemWindow();  // 선택된 아이템 창 초기화
        }

        UpdateUI();  // UI 업데이트
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;  // 아이템 보유 여부
    }
}
