using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        // ���� ī�޶� ã��
        Camera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // �� hp�� �׻� �÷��̾ ���� ���⿡ �ֵ��� ����
        transform.forward = Camera.transform.forward;
    }
}
