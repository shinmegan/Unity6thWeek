using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource waterAudioSource; // 물소리 오디오 소스
    public AudioSource campfireAudioSource; // 캠프 파이어 오디오 소스
    Transform playerTransform { get { return CharacterManager.Instance.Player.controller._transform; } }

    public float maxDistance = 5f; // 최대 거리
    public float waterMaxVolume = 1f; // 물소리 최대 볼륨
    public float campfireMaxVolume = 1f; // 캠프 파이어 소리 최대 볼륨

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
