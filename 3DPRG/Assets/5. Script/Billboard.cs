using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    GameObject Camera;
    void Start()
    {
        // ���� ī�޶� ã��
        Camera = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
        // �� hp�� �׻� �÷��̾ ���� ���⿡ �ֵ��� ����
        transform.forward = Camera.transform.forward;
    }
}
