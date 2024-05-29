using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem; // addItem 델리게이트

    private void Awake()
    {   // 싱글톤 인스턴스에 Player 인스턴스를 설정(CharacterManager를 통해 접근 가능)
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
