using System;
using UnityEngine;

public enum ItemType
{
    Equipable, // 장착
    Consumable, // 소비
    Resource // 자원
}

public enum ConsumableType
{
    Health, // 체력
    Hunger, // 배고픔
    Mana    // 마나
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // 회복량
    public float duration; // 회복 시간(기본값: 1이면 즉시 회복)
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType type; // 아이템 타입
    public Sprite icon; // 아이콘 이미지
    public GameObject dropPrefab; // 프리팹 정보

    [Header("Stacking")]
    public bool canStack; // 여러개 소지 가능 여부
    public int maxStackAmount; // 소지 가능 개수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}
