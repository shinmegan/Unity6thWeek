using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource waterAudioSource; // 물소리 오디오 소스
    public AudioSource campfireAudioSource; // 캠프 파이어 오디오 소스
    public AudioSource sunAudioSource; // 태양 오디오 소스
    public AudioSource moonAudioSource; // 달 오디오 소스
    Transform playerTransform { get { return CharacterManager.Instance.Player.controller._transform; } }

    public float maxDistance = 5f; // 최대 거리
    public float waterMaxVolume = 1f; // 물소리 최대 볼륨
    public float campfireMaxVolume = 1f; // 캠프 파이어 소리 최대 볼륨

    private static SoundManager _instance; // 유일한 인스턴스를 저장할 정적 변수
    public static SoundManager Instance // 유일한 인스턴스를 반환하는 정적 프로퍼티
    {
        get
        {   // 인스턴스가 없으면 새 게임 오브젝트를 만들어 인스턴스를 추가(방어코드)
            if (_instance == null)
            {
                _instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {   // 인스턴스 초기화
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        // 물소리 볼륨 조절(waterDistance 값이 작아지면, 오디오 소리 커짐)
        float waterDistance = Vector3.Distance(playerTransform.position, waterAudioSource.transform.position);
        waterAudioSource.volume = Mathf.Clamp(1 - (waterDistance / maxDistance), 0, waterMaxVolume);

        // 캠프 파이어 소리 볼륨 조절
        float campfireDistance = Vector3.Distance(playerTransform.position, campfireAudioSource.transform.position);
        campfireAudioSource.volume = Mathf.Clamp(1 - (campfireDistance / maxDistance), 0, campfireMaxVolume);
    }
}
