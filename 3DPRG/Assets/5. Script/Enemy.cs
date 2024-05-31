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

    public float maxHp;                     // 최대 체력
    float currentHp;                        // 현재 체력
    float atk;                              // 공격력

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
        hpBar.value = currentHp;

        anim = GetComponent<Animator>();
        if (anim == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            return;

        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.stoppingDistance 
        attackCoolTime = 1.0f;
        currentAttackCoolTime = 0.0f;
        wanderCooTime = 5.0f;
        currentWanderCooTime = wanderCooTime; 
        //if (navMeshAgent == null)
        //    return;

        wanderRadius = 10.0f;
        chaseDistance = 8.0f;
        attackDistance = 2.0f;
        atk = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.gameObject.activeSelf)
        {
            distance = Vector3.Distance(transform.position, player.position);
            switch (enemyState)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.Wander:
                    Wander();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
            }
        }
        else
        {
            enemyState = State.Wander;
            Wander();
        }
    }

    void Idle()
    {
        // 기본
        if(distance<= chaseDistance)
        {
            // 추적거리안에 들어왔을 때(기본 -> 추적)
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Idle distance<=8");
        }
        else
        {
            // 추적거리 밖일 때(기본 -> 배회)
            enemyState = State.Wander;
        }
    }
    void Chase()
    {
        // 추적
        if (distance > chaseDistance)
        {
            // 추적거리보다 길 때(추적 -> 걷기)
            enemyState = State.Idle;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance>8");
        }
        else if (distance <= attackDistance)
        {
            // 공격거리보다 짧을 떄(추적 -> 공격)
            enemyState = State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance<=2");
        }
        else
        {
            // 추적거리안에 들어왔을 때(추적)
             navMeshAgent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        // 공격
        if (distance > attackDistance)
        {
            // 공격거리안에 못들어왔을 때(공격 -> 추적)
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Attack distance>2");
        }
        else
        {
            // 공격 중
            currentAttackCoolTime += Time.deltaTime;

            if (currentAttackCoolTime >= attackCoolTime)
            {
                anim.SetTrigger("Attack");
            //anim.SetBool("isAttack", true);
                currentAttackCoolTime = 0.0f;
            }

        }
    }

    void Wander()
    {
        // 배회
        currentWanderCooTime += Time.deltaTime;
        if (currentWanderCooTime >= wanderCooTime)
        {
            Vector3 newPos = RandomNavPos();
            navMeshAgent.SetDestination(newPos);
            currentWanderCooTime = 0.0f;
        }

        if (distance <= chaseDistance)
        {
            // 추적거리보다 짧을 때 (배회 -> 추적)
            enemyState = State.Chase;
            navMeshAgent.ResetPath();
        }

    }

    Vector3 RandomNavPos()
    {
        // 구 내부의 임의의 점 반환. (3D 공간에서 무작위 위치 생성 가능)
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;


        // SamplePosition 로 주위의 NavMesh 상에서 가까운 점을 찾음
        NavMeshHit navHit;  // 유효한 위치 반환
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        return navHit.position;
    }

    public void Damaged(int damage)
    {
        Debug.Log("enmey Damage!!!!!");
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

    void AttackToPlayer()
    {
        //distance = Vector3.Distance(transform.position, player.position);
        //if (distance > attackDistance)
        //    return;
        ////Debug.Log("AttackToPlayer!!!");
        //player.SendMessage("Damaged", atk);
    }

    void DeadEnd()
    {
        gameObject.SetActive(false);
    }

    public void SetEnemyStateAnimator(State newState)
    {
        // 현재랑 같으면 넘어감
        if (enemyState == newState)
            return;

        enemyState = newState;

        //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
        //anim.SetBool("isAttack", false);
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
                anim.SetBool("Attack", true);
                break;
            case State.Damage:
                anim.SetTrigger("Damage");
                break;
            case State.Dead:
                anim.SetBool("Dead", true);
                break;
        }
    }

}
