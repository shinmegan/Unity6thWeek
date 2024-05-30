using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equip;
    public SpeedUpSkill skill;

    public ItemData itemData;
    public Action addItem; // addItem 델리게이트

    public Transform dropPosition; // 드롭 위치(버리기)

    public Vector3 playerPosition;

    private void Awake()
    {   // 싱글톤 인스턴스에 Player 인스턴스를 설정(CharacterManager를 통해 접근 가능)
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
        skill = GetComponent<SpeedUpSkill>();
    }

    private void Update()
    {
        playerPosition = gameObject.transform.position;
    }
}
