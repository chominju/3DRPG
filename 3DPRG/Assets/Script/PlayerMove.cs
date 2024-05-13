using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    
    public float jumpSpeed = 20;
    int jumpCount = 0;
    private Rigidbody rb;
    Animator anim;
    public float raycastDistance = 1.0f;

    GameObject getJoystick;
    bool getIsInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 참조
        anim = GetComponent<Animator>();
        getJoystick = GameObject.FindWithTag("Joystick");
        if (getJoystick != null)
            getIsInput = getJoystick.GetComponent<Joystick>().GetIsInput();
    }

    void FixedUpdate()
    {
        if (getJoystick.GetComponent<Joystick>().GetIsInput())
        {
            Vector2 getinputVector = getJoystick.GetComponent<Joystick>().GetinputVector();
            JoystickMove(getinputVector);
        }
        //Move();
        JumpAfter();
    }

    //private void Move()
    //{
    //    if (anim.GetBool("isAttack") == true)
    //        return;
    //    // 방향키 입력을 통해 이동 벡터 계산
    //    float horizontal = Input.GetAxis("Horizontal"); // 좌우 방향
    //    float vertical = Input.GetAxis("Vertical");     // 앞뒤 방향

    //    Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; // 정규화된 벡터
    //    Vector3 desiredVelocity = movement * speed; // 원하는 속도

    //    if(desiredVelocity != Vector3.zero)
    //    {
    //        anim.SetBool("isWalk", true);
    //        Debug.Log("movement : " + movement.x + " /// " + movement.y + " /// " + movement.z);
    //        Debug.Log("desiredVelocity : " + desiredVelocity.x + " /// " + desiredVelocity.y + " /// " + desiredVelocity.z);


    //    }
    //    else
    //        anim.SetBool("isWalk", false);

    //    // 플레이어의 Rigidbody 속도를 원하는 속도로 설정
    //    rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

    //    transform.LookAt(transform.position + movement);
    //}

    public void Sprint()
    {
        if (anim.GetBool("isSprint") == false)
        {
            speed = 8f; // 이동 속도
            anim.SetBool("isSprint", true);
        }
        else
        {
            speed = 5f; // 이동 속도
            anim.SetBool("isSprint", false);
        }
    }

    public void Jump()
    {
        if (jumpCount >= 2)
            return;

        if (anim.GetBool("isWalk") == true)
            anim.SetBool("isWalk", false);

        if (jumpCount == 0)
        {
            anim.SetBool("isJump", true);
        }
        else if (jumpCount == 1)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
                return;
            //anim.SetTrigger("DoubleJump");
            anim.SetBool("isDoubleJump",true);
        }
        jumpCount++;
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

   void JumpAfter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            anim.SetBool("isGround", true);
        }
        else
        {
            anim.SetBool("isGround", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
            anim.SetBool("isJump", false);
        }
    }

    public void JumpEnd()
    {
        anim.SetBool("isJump", false);
        anim.SetBool("isDoubleJump", false);
    }
    public void DoubleJumpEnd()
    {
        anim.SetBool("isDoubleJump", false);
    }

    public void JoystickMove(Vector2 inputVector)
    {
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y); // 정규화된 벡터
        Vector3 desiredVelocity = movement * speed; // 원하는 속도

        Debug.Log("movement : " + movement.x + " /// " + movement.y + " /// " + movement.z);
        Debug.Log("desiredVelocity : " + desiredVelocity.x + " /// " + desiredVelocity.y + " /// " + desiredVelocity.z);

        if (desiredVelocity != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
            anim.SetBool("isWalk", false);

        // 플레이어의 Rigidbody 속도를 원하는 속도로 설정
         rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }

    public void StopWalk()
    {
        anim.SetBool("isWalk", false);
    }
}
