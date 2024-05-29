using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform cameraArm;

    GameObject getJoystick;
    private void Start()
    {
        // ���̽�ƽ ��������
        getJoystick = GameObject.FindWithTag("Joystick");
    }


    //private void Update()
    //{


    //    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //    Vector3 camAngle = cameraArm.rotation.eulerAngles;
    //    float x = camAngle.x - mouseDelta.y;

    //    if(x<180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 65f);
    //    }
    //    else
    //    {
    //        x = Mathf.Clamp(x, 340f, 361f);
    //    }

    //    cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    //}
    private void Update()
    {
        // ���콺 �Է� ó��
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            if (mousePosition.x > Screen.width / 2) // ȭ���� ������ ���ݿ����� �Է� ó��(ȸ��)
            {
                RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
            }
        }

        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2) // ȭ���� ������ ���ݿ����� �Է� ó��(ȸ��)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    RotateCamera(touch.deltaPosition);
                }
            }
        }
    }

    private void RotateCamera(Vector2 inputDelta)
    {
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - inputDelta.y;

        // ĳ������ ���� �������� ����
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 65f);
        }
        else
        {
            x = Mathf.Clamp(x, 340f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + inputDelta.x, camAngle.z);
    }
}
