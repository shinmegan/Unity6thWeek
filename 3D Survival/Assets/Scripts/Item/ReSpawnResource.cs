using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class ReSpawnResource : MonoBehaviour
{
    public GameObject spawnableResource;
    public float reSpawnTime = 5f; // 재생성 대기시간

    // Update is called once per frame
    void Update()
    {
        if (!spawnableResource.gameObject.activeSelf)
        {
            StartCoroutine(ReSpawn()); // 재생성 코루틴 시작
        }

    }

    // 재생성 메서드
    private IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(reSpawnTime); // 재생성 대기 시간
        spawnableResource.SetActive(true); // 객체 활성화
    }
}
