using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        // 메인 카메라 찾기
        Camera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // 적 hp들 항상 플레이어가 보는 방향에 있도록 세팅
        transform.forward = Camera.transform.forward;
    }
}
