using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CharacterObjectMove : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform cameraArm;

    GameObject getJoystick;


    private Rigidbody rb;


    private void Start()
    {
        getJoystick = GameObject.FindWithTag("Joystick");


        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ����
    }

    private void Update()
    {
        //JoystickDrag();
        CameraTouch();
    }

    //void JoystickDrag()
    //{
    //    bool tem = getJoystick.GetComponent<Joystick>().GetIsInput();
    //    bool temm = player.GetComponent<Animator>().GetBool("isDiveRoll");
    //    if (tem && temm == false)
    //    {
    //        Vector2 inputVector = getJoystick.GetComponent<Joystick>().GetinputVector();
    //        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0.0f, cameraArm.forward.z).normalized; // ����ȭ�� ����
    //        Vector3 lookRight = new Vector3(cameraArm.right.x, 0.0f, cameraArm.right.z).normalized; // ����ȭ�� ����
    //        Vector3 moveDir = lookForward * inputVector.y + lookRight * inputVector.x;


    //        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y); // ����ȭ�� ����
    //        Vector3 desiredVelocity = movement * 5; // ���ϴ� �ӵ�

    //        player.forward = moveDir;
    //    }
    //    if (tem == false)
    //    {
    //        rb.velocity = Vector3.zero;
    //        //player.GetComponent<Rigidbody>().velocity = rb.velocity;
    //    }
    //    else
    //    {
    //       // rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);
    //        //player.GetComponent<Rigidbody>().velocity = rb.velocity;
    //    }
    //    //transform.position += moveDir * Time.deltaTime * 5.0f;
    //}

    void CameraTouch()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    Vector2 mousePosition = Input.mousePosition;
        //    if (mousePosition.x > Screen.width / 2) // ȭ���� ������ ���ݿ����� �Է� ó��
        //    {
        //        RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        //    }
        //}

        // ��ġ �Է� ó��
        foreach (Touch touch in Input.touches)
        {

            //Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("UI???");
            }
            else
            {
                if (touch.position.x > Screen.width / 2) // ȭ���� ������ ���ݿ����� �Է� ó��
                {


                    if (touch.phase == TouchPhase.Moved)
                    {
                        RotateCamera(touch.deltaPosition);
                    }
                }
            }
        }
    }

    private void RotateCamera(Vector2 inputDelta)
    {
        inputDelta = inputDelta / 2;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - inputDelta.y;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.GetComponent<Player>().OnGround();
        }

    }

}
