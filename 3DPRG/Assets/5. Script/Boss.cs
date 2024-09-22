using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    public enum State
    {
        Idle,               // 기본
        Chase,              // 추적
        Attack,             // 공격(스킬)
        Defend,              // 방어(피회복)
        Damage,             // 맞기
        Dead                // 죽기
    }

    public State enemyState;                // 현재 보스의 상태

    public float maxHp;                     // 최대 체력
    float currentHp;                        // 현재 체력
    float atk;                              // 공격력

    NavMeshAgent navMeshAgent;              // 네비메쉬

    Animator anim;                          // 애니메이터
    private Transform player;               // 플레이어 객체

    float distance;                         // 플레이어와의 거리
    float attackCoolTime;                   // 공격 쿨타임
    float currentAttackCoolTime;            // 현재 공격 쿨타임

    float attackDistance;                   // 공격을 할 거리

    int skillCount;                         // 스킬을 쓰기위한 횟수                              
    int currentSkillCount;                  // 스킬을 쓰기위한 현재 횟수  

    float defendCoolTime;                     // 방어스킬 쿨타임
    float currentDefendCoolTime;              // 현재 방어스킬 쿨타임
    float recoveryHpCoolTime;                 // 피회복 쿨타임
    float currentRecoveryHpCoolTime;          // 피회복 쿨타임
    int recoveryHpCount;                      // 피회복 횟수
    int currentRecoveryHpCount;                      // 현재 피회복 횟수

    bool isUseDefend;                         // 방어스킬 사용했는지 
    bool isDefendAble;                         // 방어스킬 사용가능한지 
    bool isAttack;
    public GameObject GameClearPanel;          // 보스를 잡았을 때 나오는 판넬
    public Slider bossHpBar;                   // 보스의 Hp Bar

    int damagedCount;
    int currentDamagedCount;
    bool counterAttack;

    void Start()
    {
        // 평소에는 비활성화 됐다가, 보스가 생성되면 활성화
        bossHpBar.gameObject.SetActive(true);
        enemyState = State.Idle;
        currentHp = maxHp;
        bossHpBar.maxValue = maxHp;
        bossHpBar.value = currentHp;

        anim = GetComponent<Animator>();
        if (anim == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            return;

        navMeshAgent = GetComponent<NavMeshAgent>();

        attackCoolTime = 5.0f;
        currentAttackCoolTime = 0.0f;

        attackDistance = 3.0f;
        atk = 10.0f;

        skillCount = 3;
        currentSkillCount = 0;

        defendCoolTime = 30.0f;
        currentDefendCoolTime = 0.0f;
        recoveryHpCoolTime = 1.0f;
        currentRecoveryHpCoolTime = 0.0f;
        isUseDefend = false;

        recoveryHpCount = 10;
        currentRecoveryHpCount =0;
        isDefendAble = true;
        isAttack = false;

        damagedCount= 3;
        currentDamagedCount = 0;
        counterAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 존재 할 때
        if (player.gameObject.activeSelf)
        {
            distance = Vector3.Distance(transform.position, player.position);
            switch (enemyState)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Defend:
                    Defend();
                    break;
            }
        }

        if (!isUseDefend)
        {
            currentDefendCoolTime -= Time.deltaTime;
            if (currentDefendCoolTime <= 0.0f)
                isDefendAble = true;
        }
    }

    void Idle()
    {
        //Debug.Log("Boss Idle");
        //// 기본
        //if (distance <= chaseDistance)
        //{
        //    // 추적거리안에 들어왔을 때(기본 -> 추적)
        //    enemyState = State.Chase;
        //}
    }
    void Chase()
    {
        //// 추적
        //if (distance > chaseDistance)
        //{
        //    // 추적거리보다 길 때(추적 -> 걷기)
        //    enemyState = State.Idle;
        //}
        if (distance <= attackDistance)
        {
            // 공격거리보다 짧을 떄(추적 -> 공격)
            //SetEnemyStateAnimator(State.Attack);
            enemyState =  State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
        else
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        // 공격
        if (distance > attackDistance)
        {
            // 공격거리안에 못들어왔을 때(공격 -> 추적)
            currentAttackCoolTime = 0.0f;
            //SetEnemyStateAnimator(State.Chase);
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // 공격 중
            currentAttackCoolTime -= Time.deltaTime;
            transform.LookAt(player.transform.position);
            if (currentAttackCoolTime <= 0.0f || counterAttack)
            {
                isAttack = true;
                currentSkillCount++;
                anim.SetTrigger("Attack");
                if (currentSkillCount >= skillCount)
                {
                    anim.SetTrigger("Skill");
                    currentSkillCount = 0;
                }
                //anim.SetBool("isAttack", true);
                currentAttackCoolTime = attackCoolTime;
            }
            else
                isAttack = false;


        }
    }

    void Defend()
    {
        Debug.Log("Defend!!!");
        // 방어 상태가 아니라면 return.
        if (enemyState != State.Defend)
            return;
        if (!isUseDefend)
        {
            anim.SetTrigger("Defend");
            anim.SetBool("isDefend", true);
        }

        isDefendAble = false;
        isUseDefend = true;

        Debug.Log("currentRecoveryHpCoolTime : " + currentRecoveryHpCoolTime);
        currentRecoveryHpCoolTime -= Time.deltaTime;
        if (currentRecoveryHpCoolTime <= 0.0f)
        {
            Debug.Log("RecoveryHp!!!");
            currentRecoveryHpCoolTime = recoveryHpCoolTime;
            currentHp += 20;
            if (currentHp >= maxHp)
                currentHp = maxHp;
            bossHpBar.value = currentHp;
            currentRecoveryHpCount++;

            if(currentRecoveryHpCount >= recoveryHpCount)
            {
                isUseDefend = false;
                currentDefendCoolTime = defendCoolTime;
                currentRecoveryHpCount = 0;
                anim.SetBool("isDefend", false);
                enemyState = State.Attack;
                //SetEnemyStateAnimator(State.Attack);
            }
        }
    }

    public void Damaged(int damage)
    {
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;

        bossHpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // 이동 중단
        navMeshAgent.ResetPath();           // 경로 초기화
        // 피가 0보다 많을 때
        if (currentHp > 0)
        {
            if (anim.GetBool("isDefend") == true)
                return;
            currentDamagedCount++;
            if(currentDamagedCount >=damagedCount)
            {
                Debug.Log("반격!!!");

                counterAttack = true;
                Attack();
                counterAttack = false;
                currentDamagedCount = 0;
                return;
            }
            if (!isAttack)
                anim.SetTrigger("Damage");
                // SetEnemyStateAnimator(State.Damage);

            if (currentHp <= maxHp / 2)
            {
                Debug.Log("Hp 1/2");
                if (isDefendAble)
                    enemyState = State.Defend;
                //SetEnemyStateAnimator(State.Defend);
            }
            else
                enemyState = State.Chase;
            //SetEnemyStateAnimator(State.Chase);
        }
        else
        {
            // 죽었을 때
            anim.SetBool("Dead", true);
            //SetEnemyStateAnimator(State.Dead);
            GameClearPanel.SetActive(true);
        }
    }

    void DamagedEnd()
    {
        enemyState = State.Chase;
        //SetEnemyStateAnimator(State.Chase);
        //// 데미지를 다 받았을 때, hp가 절반 남았을 떄 
        //if(currentHp < maxHp / 2)
        //{
        //    enemyState = State.Defend;
        //    return;
        //}

        //enemyState = State.Idle;
    }

    void AttackToPlayer()
    {
        distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance)
            return;
        //Debug.Log("AttackToPlayer!!!");
        player.GetComponent<Player>().Damaged(atk);
    }

    void AttackSkillToPlayer()
    {
        Debug.Log("Skill!!!");
        distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk * 2);
    }

    void DeadEnd()
    {
        gameObject.SetActive(false);
    }

    void AttackEnd()
    {
        enemyState = State.Chase;
        //SetEnemyStateAnimator(State.Chase);
    }

    //public void SetEnemyStateAnimator(State newState)
    //{
    //    // 현재랑 같으면 넘어감
    //    if (enemyState == newState)
    //        return;

    //    enemyState = newState;

    //    anim.SetBool("isWalk", false);

    //    // 상태에 맞는 애니메이터 파라미터 설정
    //    switch (newState)
    //    {
    //        case State.Idle:
    //            break;
    //        case State.Chase:
    //            anim.SetBool("isWalk", true);
    //            break;
    //        case State.Attack:
    //            anim.SetBool("Attack", true);
    //            break;
    //        case State.Defend:
    //            {
    //                anim.SetTrigger("Defend");
    //                anim.SetBool("isDefend", true);
    //            }
    //            break;
    //        case State.Damage:
    //            anim.SetTrigger("Damage");
    //            break;
    //        case State.Dead:
    //            anim.SetBool("Dead", true);
    //            break;
    //    }
    //}
}
