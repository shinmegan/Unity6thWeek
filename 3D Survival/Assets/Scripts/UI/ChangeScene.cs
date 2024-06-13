using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // 시작 버튼이 눌렸을 때 호출될 메소드
    public void StartGame()
    {
        Time.timeScale = 1;
        // "Main"이라는 이름의 씬으로 이동
        SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        // 에디터 모드에서 게임 종료
        CheckTime.Instance.TimeToScore();
        EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서 게임 종료
        CheckTime.Instance.TimeToScore();
        Application.Quit();
#endif
    }
}
