using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)] // time 변수의 범위를 0.0에서 1.0으로 설정
    public float time; // 현재 시간 비율 변수
    public float fullDayLength; // 하루의 길이 변수
    public float startTime = 0.4f; // 초기 시작 시간 (0.5f가 정오)
    private float timeRate; // 시간 비율 변수
    public Vector3 noon; // 정오의 각도 (90, 0, 0)

    [Header("Sun")] // 태양 관련 설정 섹션
    public Light sun; // 태양 광원
    public Gradient sunColor; // 태양의 색상 그라디언트
    public AnimationCurve sunIntensity; // 태양의 강도 곡선
    AudioSource sunAudioSource { get { return SoundManager.Instance.sunAudioSource; } } // 태양 오디오 소스

    [Header("Moon")] // 달 관련 설정 섹션
    public Light moon; // 달 광원
    public Gradient moonColor; // 달의 색상 그라디언트
    public AnimationCurve moonIntensity; // 달의 강도 곡선
    AudioSource moonAudioSource { get { return SoundManager.Instance.moonAudioSource; } } // 달 오디오 소스

    [Header("Other Lighting")] // 기타 조명 설정 섹션
    public AnimationCurve lightingIntensityMultiplier; // 조명 강도 곡선
    public AnimationCurve reflectionIntensityMultiplier; // 반사 강도 곡선

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // 하루 길이에 따른 시간 비율 계산
        time = startTime; // 초기 시간 설정
        sunAudioSource.Play(); // 태양 오디오 소스는 처음부터 활성화
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간 업데이트 및 하루 주기 계산

        UpdateLighting(sun, sunColor, sunIntensity, sunAudioSource); // 태양 조명 및 오디오 업데이트
        UpdateLighting(moon, moonColor, moonIntensity, moonAudioSource); // 달 조명 및 오디오 업데이트

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // 환경 조명 강도 업데이트
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // 반사 강도 업데이트
    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve, AudioSource audioSource)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
            
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
            
    }
}
