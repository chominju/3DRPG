using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    
    public float jumpSpeed = 20;
    int jumpCount = 0;
    private Rigidbody rb;
    Animator anim;
    public float raycastDistance = 1.0f;

    GameObject getJoystick;
    bool getIsInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ����
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
    //    // ����Ű �Է��� ���� �̵� ���� ���
    //    float horizontal = Input.GetAxis("Horizontal"); // �¿� ����
    //    float vertical = Input.GetAxis("Vertical");     // �յ� ����

    //    Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; // ����ȭ�� ����
    //    Vector3 desiredVelocity = movement * speed; // ���ϴ� �ӵ�

    //    if(desiredVelocity != Vector3.zero)
    //    {
    //        anim.SetBool("isWalk", true);
    //        Debug.Log("movement : " + movement.x + " /// " + movement.y + " /// " + movement.z);
    //        Debug.Log("desiredVelocity : " + desiredVelocity.x + " /// " + desiredVelocity.y + " /// " + desiredVelocity.z);


    //    }
    //    else
    //        anim.SetBool("isWalk", false);

    //    // �÷��̾��� Rigidbody �ӵ��� ���ϴ� �ӵ��� ����
    //    rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

    //    transform.LookAt(transform.position + movement);
    //}

    public void Sprint()
    {
        if (anim.GetBool("isSprint") == false)
        {
            speed = 8f; // �̵� �ӵ�
            anim.SetBool("isSprint", true);
        }
        else
        {
            speed = 5f; // �̵� �ӵ�
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
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y); // ����ȭ�� ����
        Vector3 desiredVelocity = movement * speed; // ���ϴ� �ӵ�

        Debug.Log("movement : " + movement.x + " /// " + movement.y + " /// " + movement.z);
        Debug.Log("desiredVelocity : " + desiredVelocity.x + " /// " + desiredVelocity.y + " /// " + desiredVelocity.z);

        if (desiredVelocity != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
            anim.SetBool("isWalk", false);

        // �÷��̾��� Rigidbody �ӵ��� ���ϴ� �ӵ��� ����
         rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }

    public void StopWalk()
    {
        anim.SetBool("isWalk", false);
    }
}
