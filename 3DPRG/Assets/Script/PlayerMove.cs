using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    
    public float jumpSpeed = 20;
    public float diveRollSpeed = 12;
    int jumpCount = 0;
    private Rigidbody rb;
    Animator anim;
    public float raycastDistance = 1.0f;

    GameObject getJoystick;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 참조
        anim = GetComponent<Animator>();
        getJoystick = GameObject.FindWithTag("Joystick");
    }

    void FixedUpdate()
    {
        if (getJoystick.GetComponent<Joystick>().GetIsInput() && anim.GetBool("isDiveRoll")==false)
        {
            Vector2 getinputVector = getJoystick.GetComponent<Joystick>().GetinputVector();
            JoystickMove(getinputVector);
        }
        else
            anim.SetBool("isWalk", false);
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

    //    if (desiredVelocity != Vector3.zero)
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

    public void DiveRoll()
    {
        if (anim.GetBool("isJump") == true)
            return;

        anim.SetBool("isDiveRoll", true);

        Vector3 cameraDirection = gameObject.transform.forward;//camera.GetComponent<Transform>().forward;

        Vector3 movement = cameraDirection.normalized; // 정규화된 벡터
        Vector3 desiredVelocity = movement * diveRollSpeed; // 원하는 속도

        rb.velocity = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }

    public void DiveRollEnd()
    {
        anim.SetBool("isDiveRoll", false);
        rb.velocity = Vector3.zero;
    }


    public void Jump()
    {
        Debug.Log("Jump Count : " + jumpCount);
        if (jumpCount >= 2|| IsPlayerOnGround())
            return;

        if (anim.GetBool("isWalk") == true)
            anim.SetBool("isWalk", false);

        anim.SetBool("isJump", true);
        if (jumpCount == 1)
        {
            if (IsPlayerOnGround())
                return;
            anim.SetBool("isDoubleJump",true);
        }
        jumpCount++;
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    public bool IsPlayerOnGround()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);
    }

   void JumpAfter()
    {
        // 점프 중 일때
        if (anim.GetBool("isJump") == true)
        {
            if (rb.velocity.y >= 0.0f)
                return;
            if(IsPlayerOnGround())
            {
                anim.SetBool("isGround", true);
                //anim.SetBool("isJump", false);
                jumpCount = 0;
            }
            else
            {
                anim.SetBool("isGround", false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
            anim.SetBool("isJump", false);
            anim.SetBool("isGround", false);
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

        if (desiredVelocity == Vector3.zero || anim.GetBool("isAttack")==true || getJoystick.GetComponent<Joystick>().GetIsInput()==false)
        {
            anim.SetBool("isWalk", false);
            return;
        }
        else
            anim.SetBool("isWalk", true);

        // 플레이어의 Rigidbody 속도를 원하는 속도로 설정
         rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }

    public void StopWalk()
    {
        anim.SetBool("isWalk", false);
        rb.velocity = Vector3.zero;
    }

    public void Hit()
    {
        Debug.Log("Hit!");
    }
}
