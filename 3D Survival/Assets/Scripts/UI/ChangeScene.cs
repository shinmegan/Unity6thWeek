using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // 시작 버튼이 눌렸을 때 호출될 메소드
    public void StartGame()
    {
        // "Main"이라는 이름의 씬으로 이동
        SceneManager.LoadScene("MainScene");
    }
}
