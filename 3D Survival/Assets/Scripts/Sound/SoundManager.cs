using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] waterAudioSources; // 물소리 오디오
    public AudioSource campfireAudioSource; // 캠프 파이어 오디오
    public AudioSource sunAudioSource; // 태양 오디오
    public AudioSource moonAudioSource; // 달 오디오
    public AudioSource DamageAudioSource; // 데미지 오디오
    public AudioSource jumpPadAudioSource; // 점프대 오디오
    public AudioSource playerAudioSource; // 플레이어 오디오
    public AudioSource conditionAudioSource; // 체력 20미만 경고 오디오

    public AudioClip[] footstepClips; // 발걸음 소리 배열
    public AudioClip jumpClip; // 점프 소리
    public AudioClip speedUpClip; // 스피드업 소리
    public AudioClip shortClip; // 짧은 소리(버튼)
    public AudioClip getItemClip; // 아이템 줍기
    public AudioClip axClip; // 채집
    public AudioClip invisibilityClip; // 무적 효과 소리
    public AudioClip missClip;
    public AudioClip dropClip;
    public AudioClip wrongClip;
    public AudioClip unequipClip;
    public AudioClip equipClip;
    public AudioClip healingClip;
    public AudioClip faceBearClip;

    Vector3 playerTransform { get { return CharacterManager.Instance.Player.playerPosition; } }

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
                return; // 이걸 추가해서 중복 객체의 초기화를 방지
            }
        }
    }

    private void Update()
    {
        if (!CharacterManager.Instance.Player.condition.isDead)
        {
            // 각 물소리 오디오 소스의 볼륨 조절
            foreach (AudioSource waterAudioSource in waterAudioSources)
            {
                float waterDistance = Vector3.Distance(playerTransform, waterAudioSource.transform.position);
                waterAudioSource.volume = Mathf.Clamp(0.9f - (waterDistance / maxDistance), 0, waterMaxVolume);
            }

            // 캠프 파이어 소리 볼륨 조절
            float campfireDistance = Vector3.Distance(playerTransform, campfireAudioSource.transform.position);
            campfireAudioSource.volume = Mathf.Clamp(0.9f - (campfireDistance / maxDistance), 0, campfireMaxVolume);
        }
    }

    // 발걸음 소리 재생 메서드
    public void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            playerAudioSource.PlayOneShot(clip);
        }
    }

    // 점프 소리 재생 메서드
    public void PlayJumpSound()
    {
        playerAudioSource.PlayOneShot(jumpClip);
    }

    // 스피드 업 소리 재생 메서드
    public void PlaySpeedUpSound()
    {
        playerAudioSource.PlayOneShot(speedUpClip);
    }

    // 무적 소리 재생 메서드
    public void PlayInvisibilitySound()
    {
        playerAudioSource.PlayOneShot(invisibilityClip);
    }

    // 버튼 클릭
    public void PlayShortSound()
    {
        playerAudioSource.PlayOneShot(shortClip);
    }

    // 아이템 줍기
    public void PlayGetSound()
    {
        playerAudioSource.PlayOneShot(getItemClip);
    }

    // 채집
    public void PlayAXSound()
    {
        playerAudioSource.PlayOneShot(axClip, 1f);
    }
    // 허공 휘두르기 소리
    public void PlayMissSound()
    {
        playerAudioSource.PlayOneShot(missClip, 1f);
    }
    // 버리기 소리
    public void PlayDropSound()
    {
        playerAudioSource.PlayOneShot(dropClip, 1f);
    }

    // 잘못된 도구 사용 또는 마나 부족 소리
    public void PlayWrongSound()
    {
        playerAudioSource.PlayOneShot(wrongClip, 1f);
    }

    // 장착 소리
    public void PlayEquipSound()
    {
        playerAudioSource.PlayOneShot(equipClip, 1f);
    }

    // 장착 해제 소리
    public void PlayUnEquipSound()
    {
        playerAudioSource.PlayOneShot(unequipClip, 1f);
    }

    // 회복하는 소리
    public void PlayHealingSound()
    {
        playerAudioSource.PlayOneShot(healingClip, 1f);
    }

    // 곰 마주친 소리
    public void PlayFacingSound()
    {
        playerAudioSource.PlayOneShot(faceBearClip, 1f);
    }
}
