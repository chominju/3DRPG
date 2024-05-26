using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum State
    {
        Idle,               // 기본
        Wander,             // 배회
        Chase,              // 추적
        Attack,             // 공격(스킬)
        Damage,             // 맞기
        Dead                // 죽기
    }

    public State enemyState;                // 현재 적의 상태
    public Slider hpBar;                    // 적의 Hp Bar

    public int maxHp;                       // 최대 체력
    int currentHp;                          // 현재 체력

    NavMeshAgent navMeshAgent;              // 네비메쉬
    Animator anim;                          // 애니메이터
    private Transform player;               // 플레이어 객체

    float distance;                         // 플레이어와의 거리
    float attackCoolTime;                   // 공격 쿨타임
    float currentAttackCoolTime;            // 현재 공격 쿨타임
    float wanderCooTime;                    // 배회 쿨타임
    float currentWanderCooTime;             // 현재 배회 쿨타임

    float wanderRadius;                     // 배회 반경
    float chaseDistance;                    // 추적을 시작 할 거리
    float attackDistance;                   // 공격을 할 거리



    // Start is called before the first frame update
    void Start()
    {
        enemyState = State.Idle;
        currentHp = maxHp;
        hpBar.maxValue = maxHp;

        anim = GetComponent<Animator>();
        if (anim == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            return;

        navMeshAgent = GetComponent<NavMeshAgent>();
        attackCoolTime = 1.0f;
        currentAttackCoolTime = 0.0f;
        wanderCooTime = 5.0f;
        currentWanderCooTime = wanderCooTime; 
        //if (navMeshAgent == null)
        //    return;

        wanderRadius = 10.0f;
        chaseDistance = 8.0f;
        attackDistance = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        Debug.Log(enemyState);
        switch(enemyState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Wander:
                Wander();
                break;
            case State.Chase:
                Walk();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        // 기본
        if(distance<= chaseDistance)
        {
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Idle distance<=8");
        }
        else
            enemyState = State.Wander;
    }
    void Walk()
    {
        // 걷기
        if (distance > chaseDistance)
        {
            enemyState = State.Idle;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance>8");
        }
        else if (distance <= attackDistance)
        {
            enemyState = State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance<=2");
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
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Attack distance>2");
        }
        else
        {
            currentAttackCoolTime += Time.deltaTime;

            if (currentAttackCoolTime >= attackCoolTime)
            {
            anim.SetBool("isAttack", true);
                currentAttackCoolTime = 0.0f;
            }

        }
    }

    void Wander()
    {
        currentWanderCooTime += Time.deltaTime;
        // 배회
        if (currentWanderCooTime >= wanderCooTime)
        {
            Vector3 newPos = RandomNavPos();
            navMeshAgent.SetDestination(newPos);
            currentWanderCooTime = 0.0f;
        }

        if (distance <= chaseDistance)
        {
            enemyState = State.Chase;
            navMeshAgent.ResetPath();
        }

    }

    Vector3 RandomNavPos()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        return navHit.position;
    }


    void Damaged(int damage)
    {
        currentHp -= damage;

        hpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // 이동 중단
        navMeshAgent.ResetPath();           // 경로 초기화

        if(currentHp > 0)
        {
            SetEnemyStateAnimator(State.Damage);
            anim.SetTrigger("Damage");
            enemyState = State.Damage;
        }
        else
        {
            SetEnemyStateAnimator(State.Dead);
            anim.SetTrigger("Dead");
            enemyState = State.Dead;
        }
    }

    void DamagedEnd()
    {
        enemyState = State.Idle;
    }

    void RealAttack()
    {
        player.SendMessage("Damaged", 10);
    }

    void AttackAnim()
    {
        if(player != null)
        {
            // 적과 플레이어의 충돌 여부 검사
            if (IsPlayerInAttackRange())
            {
                Debug.Log("Attack!!!");
                //// 플레이어에게 데미지를 입힘
                //PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                //if (playerHealth != null)
                //{
                //    playerHealth.TakeDamage(attackDamage);
                //}
            }
        }
    }

    bool IsPlayerInAttackRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // 임의의 충돌 거리. 적과 플레이어의 거리 기준으로 수정 필요
        return distance < 1.5f;
    }

    public void SetEnemyStateAnimator(State newState)
    {
        // 현재랑 같으면 넘어감
        if (enemyState == newState)
            return;

        enemyState = newState;

        //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);
        //  anim.SetBool("IsDamage", false);
        //  anim.SetBool("IsDown", false);
        //  anim.SetBool("IsDead", false);

        // 상태에 맞는 애니메이터 파라미터 설정
        switch (newState)
        {
            case State.Idle:
                anim.SetBool("isOnGround", true);
                break;
            case State.Chase:
            case State.Wander:
                anim.SetBool("isWalk", true);
                break;
            case State.Attack:
                anim.SetBool("isAttack", true);
                break;
            case State.Damage:
                anim.SetTrigger("Damage");
                break;
            case State.Dead:
                anim.SetBool("isDead", true);
                break;
        }
    }





    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Player"))
    //    {
    //            anim.SetBool("isAttack", true);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Player"))
    //        anim.SetBool("isAttack", false);
    //}

}
