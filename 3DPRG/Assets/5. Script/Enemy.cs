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

    int skillCount;                         // 스킬을 쓰기위한 횟수                              
    int currentSkillCount;                  // 스킬을 쓰기위한 현재 횟수                              


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
        attackCoolTime = 5.0f;
        currentAttackCoolTime = 0.0f;
        wanderCooTime = 3.0f;
        currentWanderCooTime = wanderCooTime; 

        wanderRadius = 5.0f;
        chaseDistance = 8.0f;
        attackDistance = 2.0f;
        atk = 10.0f;

        skillCount = 3;
        currentSkillCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 존재 할 때 상태가 나뉨
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
                case State.Damage:
                    break;
                case State.Dead:
                    break;
            }
        }
        else
        {
            // 플레이어가 존재하지 않는다면 배회한다.
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
        }
        else if (distance <= attackDistance)
        {
            // 공격거리보다 짧을 떄(추적 -> 공격)

           enemyState = State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
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
            currentAttackCoolTime = 0.0f;
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // 공격 중
            currentAttackCoolTime -= Time.deltaTime;

            if (currentAttackCoolTime <= 0.0f)
            {
                transform.LookAt(player.transform.position);
                currentSkillCount++;
                anim.SetTrigger("Attack");
                if(currentSkillCount >= skillCount)
                {
                    anim.SetTrigger("Skill");
                    currentSkillCount = 0;
                }
                //anim.SetBool("isAttack", true);
                currentAttackCoolTime = attackCoolTime;
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
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;
        DamageManager.GetInstance().CreateDamage(damage , Damage.DamageType.Player , transform.position);
        hpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // 이동 중단
        navMeshAgent.ResetPath();           // 경로 초기화

        if(currentHp > 0)
        {
            if (enemyState != State.Attack || enemyState !=State.Damage)
            {
                enemyState = State.Damage;
                anim.SetTrigger("Damage");
            }
        }
        else
        {
            if (enemyState != State.Dead)
            {
                enemyState = State.Dead;
                anim.SetTrigger("Dead");
                EnemyManager.GetInstance().AddEnmeyKillCount();


            }
        }
    }



    void DamagedEnd()
    {
        enemyState = State.Chase;
    }

    void AttackToPlayer()
    {
        distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance)
            return; 
        player.GetComponent<Player>().Damaged(atk);
    }

    void AttackSkillToPlayer()
    {
        Debug.Log("Skill!!!");
        distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk*2);
    }

    void DeadEnd()
    {
        var getPool = EnemyManager.GetInstance().GetEnemyPool();
        getPool.Release(gameObject);
    }

}
    