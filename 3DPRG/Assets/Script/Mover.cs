using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    private Rigidbody rb;
    private int jumpCount = 0;
    public float jumpSpeed = 10;
    Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ����
        anim = GetComponent<Animator>();
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
                Debug.Log("���� ����");
                anim.SetBool("isDoubleJump", true);
            }
            else
            {

                Debug.Log("����");
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



