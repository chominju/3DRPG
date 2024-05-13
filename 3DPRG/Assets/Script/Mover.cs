using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private Rigidbody rb;
    private int jumpCount = 0;
    public float jumpSpeed = 10;
    Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 참조
        anim = GetComponent<Animator>();
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


        //if (Input.GetKeyDown(KeyCode.G)) 
        //{
        //    Debug.Log("Click1");

        //}
        //if(Input.GetMouseButtonDown(1)&&jumpCount<2)
        //{
        //    Debug.Log("Click");
        //    //anim.SetTrigger("Jump");
        //    //Debug.Log("1");
        //    //if(anim.GetBool("isJump") == true && anim.GetBool("isDoubleJump") == false)
        //    //{
        //    //Debug.Log("2");
        //    //    anim.SetBool("isDoubleJump", true);
        //    //}
        //    //else
        //    //{

        //    //Debug.Log("3");
        //    //    anim.SetBool("isJump", true);
        //    //}
        //    //Debug.Log("4");
        //    //jumpCount++;
        //    //rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        //}
    }

    public void tempJ()
    {
        if (jumpCount < 2)
        {
            Debug.Log("Click");
            anim.SetTrigger("Jump");
            if (anim.GetBool("isJump") == true && anim.GetBool("isDoubleJump") == false)
            {
                Debug.Log("더블 점프");
                anim.SetBool("isDoubleJump", true);
            }
            else
            {

                Debug.Log("점프");
                anim.SetBool("isJump", true);
            }
            jumpCount++;
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
            anim.SetBool("isJump", false);
            anim.SetBool("isDoubleJump", false);
        }
    }

    public void JumpEnd()
    {
        Debug.Log("5");
        //anim.SetBool("isJu", false);
        anim.SetBool("isJump", false);
        anim.SetBool("isDoubleJump", false);
    }

    public void JumpStart()
    {
        Debug.Log("6");
        anim.SetBool("isJu", true);
    }

    public void douJumpEnd()
    {
        Debug.Log("DU");
        anim.SetBool("isDoubleJump", false);
    }

    

    public void AirStart()
    {
       // anim.SetBool("isDoubleJump", true);
    }
}



