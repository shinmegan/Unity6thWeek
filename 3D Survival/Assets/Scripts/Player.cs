using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {   // 싱글톤 인스턴스에 Player 인스턴스를 설정(CharacterManager를 통해 접근 가능)
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    }
}
