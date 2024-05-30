using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    // 아이템 정보 가져와서 표시하기
    public string GetInteractName()
    {
        string str = $"{data.displayName}";
        return str ;
    }

    public string GetInteractDescription()
    {
        string str = $"{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject); // E(줍기) 버튼을 눌렀을 때, 게임 화면에서 아이템을 사라짐
        // 효과음 재생
        SoundManager.Instance.PlayGetSound();
    }
}
