using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 참조
    }

    void FixedUpdate()
    {
        // 방향키 입력을 통해 이동 벡터 계산
        float horizontal = Input.GetAxis("Horizontal"); // 좌우 방향
        float vertical = Input.GetAxis("Vertical");     // 앞뒤 방향

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; // 정규화된 벡터
        Vector3 desiredVelocity = movement * speed; // 원하는 속도

        // 플레이어의 Rigidbody 속도를 원하는 속도로 설정
        rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }
}



