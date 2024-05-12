using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ����
    }

    void FixedUpdate()
    {
        // ����Ű �Է��� ���� �̵� ���� ���
        float horizontal = Input.GetAxis("Horizontal"); // �¿� ����
        float vertical = Input.GetAxis("Vertical");     // �յ� ����

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; // ����ȭ�� ����
        Vector3 desiredVelocity = movement * speed; // ���ϴ� �ӵ�

        // �÷��̾��� Rigidbody �ӵ��� ���ϴ� �ӵ��� ����
        rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }
}



