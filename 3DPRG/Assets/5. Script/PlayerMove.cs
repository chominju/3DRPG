using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    
    public float jumpSpeed = 7;
    public float diveRollSpeed = 12;
    int jumpCurrentCount = 0;
    private Rigidbody rb;
    Animator anim;
    public float raycastDistance = 1.0f;
    public float doubleDistance = 0.5f;

    GameObject getJoystick;
    [SerializeField]
    private Transform character;
    [SerializeField]
    private Transform cameraArm;

    int jumpMaxCount;
    bool isJumpClickFirst;
    bool isDiveRoll;
    bool isJump;
    bool isDoubleJump;

    float currentHp;
    float maxHp;
    public Slider hpBar;


    public enum State
    {
        Idle,           // 기본
        Walk,           // 걷기(달리기)
        Attack,         // 공격(스킬)
        Jump,           // 점프(2단)
        DiveRoll,       // 구르기
        Damage,         // 맞기
        Down,           // 넘어짐
        Dead,           // 죽기
    }

    State playerState;
    void Start()
    {
        playerState = State.Idle;
        //rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 참조
        anim = GetComponent<Animator>();
        getJoystick = GameObject.FindWithTag("Joystick");
        isJumpClickFirst = false;
        isDiveRoll = false;
        isDoubleJump = false;
        jumpMaxCount = 2;
        maxHp = 100;
        currentHp = maxHp;
        hpBar.maxValue = maxHp;
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

        Jumping();
    }
    void PlayerIdle()
    {
        switch(playerState)
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
        /* 구르기가 불가능한 경우
         - 이미 버튼을 눌렀거나
         - 점프(2단점프)
         - 맞기
         - 넘어짐
         - 사망 */
        if (isDiveRoll || (playerState == State.Jump)|| (playerState == State.Attack) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
            return;

        SetPlayerStateAnimator(State.DiveRoll);
        //playerState = PlayerState.DiveRoll;
        isDiveRoll = true;
        Vector3 cameraDirection = gameObject.transform.forward;//camera.GetComponent<Transform>().forward;

        Vector3 movement = cameraDirection.normalized; // 정규화된 벡터
        Vector3 desiredVelocity = movement * diveRollSpeed; // 원하는 속도

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
        /* 점프가 불가능한 경우
         - 이미 버튼을 3번이상 눌렀거나(2단점프까지임)
         - 공격
         - 구르기
         - 맞기
         - 넘어짐
         - 사망 */
        if ((jumpCurrentCount >= jumpMaxCount)||(playerState == State.Attack) || (playerState == State.DiveRoll) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
            return;

        if (getJoystick.GetComponent<Joystick>().GetIsInput())
            isJumpClickFirst = false;   // 이동키 + 점프(이동하면서 점프)
        else
            isJumpClickFirst = true;    // 점프 + 이동키(제자리 점프)

        SetPlayerStateAnimator(State.Jump);


        if (jumpCurrentCount == 1)
        {
            if (IsPlayerDoubleJumpAble())
                return;
            isDoubleJump = true;
            anim.SetTrigger("DoubleJump");
            //anim.SetBool("isDoubleJump", true);
        }
        jumpCurrentCount++;

        character.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        //Debug.Log("Jump velocity Y : " + character.GetComponent<Rigidbody>().velocity.y);
    }

    // 플레이어 더블점프가 가능한 위치인가?
    public bool IsPlayerDoubleJumpAble()
    {
        RaycastHit hit;
        return Physics.Raycast(character.position, Vector3.down, out hit, doubleDistance);
    }

    // 플레이어가 바닥에 닿고있는지
    public bool IsPlayerOnGround()
    {
        RaycastHit hit;
        return Physics.Raycast(character.position, Vector3.down, out hit, raycastDistance);
    }

   void Jumping()
    {
        // 점프 중 일때
        if(playerState == State.Jump)
        {
            // 위로 올라가는 중이면 제외
            if (character.GetComponent<Rigidbody>().velocity.y >= 0.0f)
                return;
            if (IsPlayerOnGround())
            {
                // 땅에 닿는 위치인가?
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
        /* 이동이 불가능한 경우
      - 선점프
      - 구르기
      - 공격
      - 맞기
      - 넘어짐
      - 사망 */
        if (isJumpClickFirst ||(playerState == State.Attack) || (playerState == State.DiveRoll) || (playerState == State.Damage) || (playerState == State.Down) || (playerState == State.Dead))
        {
            //Debug.Log("JoystickMove // PlayerState : " + playerState);
            return;
        }

        Vector2 inputVector = getJoystick.GetComponent<Joystick>().GetinputVector();

        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y); // 정규화된 벡터
        Vector3 desiredVelocity = movement * speed; // 원하는 속도
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0.0f, cameraArm.forward.z).normalized; // 정규화된 벡터
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0.0f, cameraArm.right.z).normalized; // 정규화된 벡터
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

        //// 걷기(점프키x)
        //if (!isJump)
        //{
        //    SetPlayerStateAnimator(State.Walk);
        //    character.GetComponent<Rigidbody>().velocity = moveDir * speed;
        //}
        //else
        //{
        //    character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
        //속도 0 / 공격중 / 조이스틱을 땟을 때
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

    void Damaged(float damage)
    {
        // 데미지를 받았을 떄
        currentHp -= damage;
        hpBar.value = currentHp;

        if (currentHp > 0)
            SetPlayerStateAnimator(State.Damage);
        else
            Dead();

    }

    void Dead()
    {
        // 죽었을 때
        SetPlayerStateAnimator(State.Dead);

    }

    void Down()
    {
        // 데미지를 받아서 넘어졌을 때
        SetPlayerStateAnimator(State.Down);

    }

    public State GetPlayerState()
    {
        return playerState;
    }

    public void SetPlayerStateAnimator(State newState)
    {
        // 현재랑 같으면 넘어감
        if (playerState == newState) 
            return;

        playerState = newState;

      //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
      //  anim.SetBool("isAttack", false);
      //  anim.SetBool("IsDamage", false);
      //  anim.SetBool("IsDown", false);
      //  anim.SetBool("IsDead", false);

        // 상태에 맞는 애니메이터 파라미터 설정
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
                anim.SetBool("isDown", true);
                break;
            case State.Dead:
                anim.SetBool("isDead", true);
                break;
        }
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
}
    