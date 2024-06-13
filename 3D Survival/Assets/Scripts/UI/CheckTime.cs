using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckTime : MonoBehaviour
{
    // TextMeshProUGUI 오브젝트, 시간 텍스트와 최고 점수 텍스트
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI bestScore;

    // 타이머 변수
    public int timer = 0;

    // 최고 점수를 저장할 PlayerPrefs 키
    private const string BestScoreKey = "BestScore";

    private static CheckTime _instance; // 유일한 인스턴스를 저장할 정적 변수

    // 게임 시작 시 타이머 코루틴 시작
    private void Start()
    {
        // 최고 점수가 존재하는지 확인하고, 없으면 0으로 설정
        int bestScoreValue = PlayerPrefs.HasKey(BestScoreKey) ? PlayerPrefs.GetInt(BestScoreKey) : 0;

        // 최고 점수를 텍스트로 표시
        bestScore.text = "최고점: " + bestScoreValue.ToString("D6");

        // 타이머 코루틴 시작
        StartCoroutine(TimerCoroutine());
    }


    public static CheckTime Instance // 유일한 인스턴스를 반환하는 정적 프로퍼티
    {
        get
        {   // 인스턴스가 없으면 새 게임 오브젝트를 만들어 인스턴스를 추가(방어코드)
            if (_instance == null)
            {
                _instance = new GameObject("CheckTime").AddComponent<CheckTime>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {   // 인스턴스 초기화
            _instance = this;
        }
        else
        {
            if (_instance == this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
                return; // 이걸 추가해서 중복 객체의 초기화를 방지
            }
        }
    }

    // 타이머 코루틴, 1초마다 경과 시간을 갱신
    IEnumerator TimerCoroutine()
    {
        // 1초마다 타이머 증가
        timer += 1;

        // 타이머를 시:분:초 형식으로 변환하여 표시
        timeTxt.text = (timer / 3600).ToString("D2") + ":" + (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 타이머 코루틴 재시작
        StartCoroutine(TimerCoroutine());
    }

    // 게임 종료 시 호출, 경과 시간을 점수로 변환하고 최고 점수 갱신
    public void TimeToScore()
    {
        // 경과 시간을 시:분:초 형식으로 변환
        string timeString = timeTxt.text;

        // 시간, 분, 초를 나눔
        string[] timeParts = timeString.Split(':');
        if (timeParts.Length != 3)
        {
            Debug.LogError("시간 형식이 잘못되었습니다.");
            return;
        }

        // 시간, 분, 초를 변환
        if (!int.TryParse(timeParts[0], out int hours))
        {
            hours = 0; // 기본값 설정
        }
        if (!int.TryParse(timeParts[1], out int minutes))
        {
            minutes = 0; // 기본값 설정
        }
        if (!int.TryParse(timeParts[2], out int seconds))
        {
            seconds = 0; // 기본값 설정
        }

        // 시간, 분, 초를 하나의 숫자로 변환하여 점수로 사용
        int score = hours * 10000 + minutes * 100 + seconds;

        // 최고 점수를 저장할 PlayerPrefs 키
        string bestScoreKey = "BestScore";

        // 기존 최고 점수를 불러옴
        int bestScoreValue = PlayerPrefs.GetInt(BestScoreKey, 0);

        // 현재 점수가 기존 최고 점수보다 높은 경우
        if (score > bestScoreValue)
        {
            // 현재 점수를 최고 점수로 저장
            PlayerPrefs.SetInt(bestScoreKey, score);
            bestScoreValue = score;
        }

        // 최고 점수를 텍스트로 표시
        bestScore.text = "최고점: " + bestScoreValue.ToString("D6");
    }
}
