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

    [Header("Moon")] // 달 관련 설정 섹션
    public Light moon; // 달 광원
    public Gradient moonColor; // 달의 색상 그라디언트
    public AnimationCurve moonIntensity; // 달의 강도 곡선

    [Header("Other Lighting")] // 기타 조명 설정 섹션
    public AnimationCurve lightingIntensityMultiplier; // 조명 강도 곡선
    public AnimationCurve reflectionIntensityMultiplier; // 반사 강도 곡선

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // 하루 길이에 따른 시간 비율 계산
        time = startTime; // 초기 시간 설정
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간 업데이트 및 하루 주기 계산

        UpdateLighting(sun, sunColor, sunIntensity); // 태양 조명 업데이트
        UpdateLighting(moon, moonColor, moonIntensity); // 달 조명 업데이트

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // 환경 조명 강도 업데이트
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // 반사 강도 업데이트
    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time); // 현재 시간에 따른 강도 계산

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f; // 광원의 각도 업데이트
        lightSource.color = colorGradiant.Evaluate(time); // 광원의 색상 업데이트
        lightSource.intensity = intensity; // 광원의 강도 업데이트

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy) // 강도가 0이면 광원을 비활성화
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy) // 강도가 0보다 크면 광원을 활성화
            go.SetActive(true);
    }
}
