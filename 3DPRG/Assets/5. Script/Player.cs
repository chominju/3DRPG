using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�

    public float jumpSpeed = 7;                                     // ���� �ӵ�(�Ÿ�)
    public float diveRollSpeed = 12;                                // ������ �ӵ�(�Ÿ�)
    int jumpCurrentCount = 0;                                       // ���� ���� ī��Ʈ
    private Rigidbody rb;                                           // ������ٵ�
    Animator anim;                                                  // �ִϸ�����
    public float raycastDistance = 1.0f;                            // ���ٴڰ��� �Ÿ� ����
    public float doubleDistance = 0.5f;                             // ���������� ������ ���ٴڰ��� �Ÿ�

    GameObject getJoystick;                                         // ���̽�ƽ ������Ʈ
    [SerializeField]
    private Transform character;                                    // �θ� ������Ʈ
    [SerializeField]
    private Transform cameraArm;                                    // ī�޶� ������Ʈ

    public GameObject sprintButton;

    int jumpMaxCount;                                               // ���� �ִ� �� 
    bool isJumpClickFirst;                                          // ������ ���� Ŭ���ߴ°�
    bool isDiveRoll;                                                // ������ ���ΰ�

    float currentHp;                                                // ���� hp
    float maxHp;                                                    // �ִ� hp
    public Slider hpBar;                                            // �÷��̾� hp��

    int damageCount;                                                // ����� ������ �¾Ҵ°�

    bool isDown;                                                    // �Ѿ�������
    bool isInvincibility;                                           // ���������ΰ�
    float invincibilityTimer;                                       // �����ð�
    float invincibilityCurrentTimer;                                // ���� �����ð�

    bool isDamage;
    float damageTimer;
    float currentDamageTimer;

    public enum State
    {
        Idle,           // �⺻
        Walk,           // �ȱ�(�޸���)
        Attack,         // ����(��ų)
        Jump,           // ����(2��)
        DiveRoll,       // ������
        Damage,         // �±�
        Down,           // �Ѿ���
        Dead,           // �ױ�
    }

    State playerState;
    void Start()
    {
        playerState = State.Idle;
        //rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ����
        anim = GetComponent<Animator>();
        getJoystick = GameObject.FindWithTag("Joystick");
        isJumpClickFirst = false;
        isDiveRoll = false;
        jumpMaxCount = 2;
        maxHp = 1000;
        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        damageCount = 0;
        isInvincibility = false;
        invincibilityTimer = 2.0f;
        invincibilityCurrentTimer = 0.0f;
        hpBar.GetComponentInChildren<Text>().text = currentHp.ToString() + " / " + maxHp.ToString();

        isDamage = false;
        damageTimer = 1.0f;
        currentDamageTimer = 0.0f;
    }

    void FixedUpdate()
    {
        if (getJoystick.GetComponent<Joystick>().GetIsInput())
        {
            JoystickMove();
        }
        else
        {
            PlayerIdle();
        }

        if(isInvincibility)
        {
            invincibilityCurrentTimer += Time.deltaTime;
            if(invincibilityCurrentTimer >= invincibilityTimer)
            {
                isInvincibility = false;
                invincibilityCurrentTimer = 0.0f;
            }
        }

        if(isDamage)
        {
            currentDamageTimer -= Time.deltaTime;
            if(currentDamageTimer<=0.0f)
            {
                currentDamageTimer = damageTimer;
                isDamage = false;
            }
        }    

        Jumping();
    }
    void PlayerIdle()
    {
        switch (playerState)
        {
            case State.Attack:
            case State.Damage:
            case State.Dead:
            case State.DiveRoll:
            case State.Down:
            case State.Jump:
                break;
            case State.Walk:
                SetPlayerStateAnimator(State.Idle);
                break;

        }
    }

    public void Sprint()
    {
        if (anim.GetBool("isSprint") == false)
        {
            speed = 8f; // �̵� �ӵ�
            anim.SetBool("isSprint", true);
            sprintButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            speed = 5f; // �̵� �ӵ�
            anim.SetBool("isSprint", false);
            sprintButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void DiveRoll()
    {
        /* �����Ⱑ �Ұ����� ���
         - �̹� ��ư�� �����ų�
         - ����(2������)
         - �±�
         - �Ѿ���
         - ��� */
        if (isDiveRoll || (playerState == State.Jump) || (playerState == State.Attack) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
            return;

        SetPlayerStateAnimator(State.DiveRoll);
        //playerState = PlayerState.DiveRoll;
        isDiveRoll = true;
        Vector3 cameraDirection = gameObject.transform.forward;//camera.GetComponent<Transform>().forward;

        Vector3 movement = cameraDirection.normalized; // ����ȭ�� ����
        Vector3 desiredVelocity = movement * diveRollSpeed; // ���ϴ� �ӵ�

        //rb.velocity = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);
        character.GetComponent<Rigidbody>().velocity = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);

        transform.LookAt(transform.position + movement);
    }

    public void DiveRollEnd()
    {
        isDiveRoll = false;
        SetPlayerStateAnimator(State.Idle);
        //playerState = PlayerState.Idle;
        // anim.SetBool("isDiveRoll", false);
        Debug.Log("DiveRollEnd");
        //rb.velocity = Vector3.zero;
        character.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    public void Jump()
    {
        /* ������ �Ұ����� ���
         - �̹� ��ư�� 3���̻� �����ų�(2������������)
         - ����
         - ������
         - �±�
         - �Ѿ���
         - ��� */
        if ((jumpCurrentCount >= jumpMaxCount) || (playerState == State.Attack) || (playerState == State.DiveRoll) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
            return;

        if (getJoystick.GetComponent<Joystick>().GetIsInput())
            isJumpClickFirst = false;   // �̵�Ű + ����(�̵��ϸ鼭 ����)
        else
            isJumpClickFirst = true;    // ���� + �̵�Ű(���ڸ� ����)

        SetPlayerStateAnimator(State.Jump);


        if (jumpCurrentCount == 1)
        {
            if (IsPlayerDoubleJumpAble())
                return;
            anim.SetTrigger("DoubleJump");
            //anim.SetBool("isDoubleJump", true);
        }
        jumpCurrentCount++;

        character.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        //Debug.Log("Jump velocity Y : " + character.GetComponent<Rigidbody>().velocity.y);
    }

    // �÷��̾� ���������� ������ ��ġ�ΰ�?
    public bool IsPlayerDoubleJumpAble()
    {
        RaycastHit hit;
        return Physics.Raycast(character.position, Vector3.down, out hit, doubleDistance);
    }

    // �÷��̾ �ٴڿ� ����ִ���
    public bool IsPlayerOnGround()
    {
        RaycastHit hit;
        return Physics.Raycast(character.position, Vector3.down, out hit, raycastDistance);
    }

    void Jumping()
    {
        // ���� �� �϶�
        if (playerState == State.Jump)
        {
            // ���� �ö󰡴� ���̸� ����
            if (character.GetComponent<Rigidbody>().velocity.y >= 0.0f)
                return;
            if (IsPlayerOnGround())
            {
                // ���� ��� ��ġ�ΰ�?
                anim.SetBool("isOnGround", true);
            }
            //if (rb.velocity.y >= 0.0f)
            //    return;
            //if(IsPlayerOnGround())
            //{
            //    anim.SetBool("isGround", true);
            //    //anim.SetBool("isJump", false);
            //    jumpCount = 0;
            //}
            //else
            //{
            //    anim.SetBool("isGround", false);
            //}
        }
    }

    public void OnGround()
    {
        Debug.Log("OnGround");
        jumpCurrentCount = 0;
        //isJump = false;
        isJumpClickFirst = false;
        //anim.SetBool("isJump", false);
        anim.SetBool("isOnGround", true);
        character.GetComponent<Rigidbody>().velocity = Vector3.zero;

        SetPlayerStateAnimator(State.Idle);
        //playerState = PlayerState.Idle;
    }


    public void JoystickMove()
    {

        if (!getJoystick.GetComponent<Joystick>().GetIsInput())
            return;

        /* �̵��� �Ұ����� ���
      - ������
      - ������
      - ����
      - �±�
      - �Ѿ���
      - ��� */

        if (isJumpClickFirst || (playerState == State.Attack) || (playerState == State.DiveRoll) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
        {
            //Debug.Log("JoystickMove // PlayerState : " + playerState);
            return;
        }

        Vector2 inputVector = getJoystick.GetComponent<Joystick>().GetinputVector();

        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y); // ����ȭ�� ����
        Vector3 desiredVelocity = movement * speed; // ���ϴ� �ӵ�
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0.0f, cameraArm.forward.z).normalized; // ����ȭ�� ����
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0.0f, cameraArm.right.z).normalized; // ����ȭ�� ����
        Vector3 moveDir = lookForward * inputVector.y + lookRight * inputVector.x;
        transform.forward = moveDir;


        if (playerState != State.Jump)
        {
            SetPlayerStateAnimator(State.Walk);
        }
        //character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //else
        //{
        //}

        Vector3 moveSpeed = moveDir * speed;
        character.GetComponent<Rigidbody>().velocity = new Vector3(moveSpeed.x, character.GetComponent<Rigidbody>().velocity.y, moveSpeed.z);

        //character.GetComponent<Rigidbody>().velocity = moveDir * speed;

        //// �ȱ�(����Űx)
        //if (!isJump)
        //{
        //    SetPlayerStateAnimator(State.Walk);
        //    character.GetComponent<Rigidbody>().velocity = moveDir * speed;
        //}
        //else
        //{
        //    character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
        //�ӵ� 0 / ������ / ���̽�ƽ�� ���� ��
        //if (desiredVelocity == Vector3.zero || anim.GetBool("isAttack") == true || getJoystick.GetComponent<Joystick>().GetIsInput() == false)
        //{
        //    anim.SetBool("isWalk", false);

        //    //rb.velocity = Vector3.zero;
        //    character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    //Character.GetComponent<Rigidbody>().velocity = rb.velocity;

        //    return;
        //}
        //else
        //{
        //    if (anim.GetBool("isJump") == false)
        //    {
        //        //rb.velocity = moveDir * speed;//new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);
        //        character.GetComponent<Rigidbody>().velocity = moveDir * speed;
        //        //Character.GetComponent<Rigidbody>().velocity = rb.velocity;
        //        anim.SetBool("isWalk", true);
        //    }
        //}

    }   

    public void Damaged(float damage)
    {
        if (isInvincibility)
            return;

        Debug.Log("enemy -> player Attack");
        // �������� �޾��� ��
        currentHp -= damage;
        DamageManager.GetInstance().CreateDamage((int)damage, Damage.DamageType.Enemy, transform.position);
        hpBar.value = currentHp;
        hpBar.GetComponentInChildren<Text>().text = currentHp.ToString() + " / " + maxHp.ToString();


        if (currentHp > 0)
        {
            if (isDamage)
                return;
            isDamage = true;

            Debug.Log("damageCount : " + damageCount);
            Debug.Log("isInvincibility : " + isInvincibility);
            if (isDown)
                return;
            damageCount++;
            if (damageCount <= 3)
            {
                if (playerState != State.Attack)
                    SetPlayerStateAnimator(State.Damage);
            }
            else
            {
                SetPlayerStateAnimator(State.Down);
            }

        }
        else
            Dead();

    }

    void Dead()
    {
        // �׾��� ��
        SetPlayerStateAnimator(State.Dead);

    }

    void Down()
    {
        // �������� �޾Ƽ� �Ѿ����� ��
        SetPlayerStateAnimator(State.Down);

    }

    public State GetPlayerState()
    {
        return playerState;
    }

    public void SetPlayerStateAnimator(State newState)
    {
        // ����� ������ �Ѿ
        if (playerState == newState)
            return;

        playerState = newState;

        //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
        //  anim.SetBool("isAttack", false);
        //  anim.SetBool("IsDamage", false);
        //  anim.SetBool("IsDown", false);
        //  anim.SetBool("IsDead", false);

        // ���¿� �´� �ִϸ����� �Ķ���� ����
        switch (newState)
        {
            case State.Idle:
                anim.SetBool("isOnGround", true);
                break;
            case State.Walk:
                anim.SetBool("isWalk", true);
                break;
            case State.Attack:
                anim.SetTrigger("Attack");
                //anim.SetBool("isAttack", true);
                break;
            case State.Jump:
                {
                    anim.SetBool("isOnGround", false);
                    anim.SetTrigger("Jump");
                    break;
                }
            case State.DiveRoll:
                anim.SetTrigger("DiveRoll");
                break;
            case State.Damage:
                anim.SetTrigger("Damage");
                break;
            case State.Down:
                {
                    if(!isInvincibility)
                    {
                        isDown = true;
                        anim.SetTrigger("Down");
                    }
                    //if (anim.GetBool("isDown") == false)
                    //{
                    //    anim.SetTrigger("Down");
                    //    anim.SetBool("isDown", true);
                    //}
                }
                break;
            case State.Dead:
                {
                    StartCoroutine(HideAndShowCoroutine());
                }
                break;
        }
    }

    void DownEnd()
    {
        //anim.SetBool("isDown", false);
        SetPlayerStateAnimator(State.Idle);
        isDown = false;
        isInvincibility = true;
        damageCount = 0;
        // isInvincibility
    }

    void DamageEnd()
    {
        SetPlayerStateAnimator(State.Idle);
    }

    public void EndJoystickEnd()
    {
        if (playerState == State.Walk)
            character.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Hit()
    {
        Debug.Log("Hit!");
    }


    IEnumerator HideAndShowCoroutine()
    {
        // �÷��̾� ������Ʈ�� ����ϴ�.
        gameObject.SetActive(false);

        // 2�ʸ� ��ٸ��ϴ�.
        yield return new WaitForSeconds(2f);

        // �÷��̾� ������Ʈ�� �ٽ� ���̰� �մϴ�.
        gameObject.SetActive(true);
        currentHp = maxHp;
    }

}
