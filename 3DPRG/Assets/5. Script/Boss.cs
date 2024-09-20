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

    public State enemyState;                // 현재 적의 상태
    public Slider hpBar;                    // 적의 Hp Bar

    public float maxHp;                     // 최대 체력
    float currentHp;                        // 현재 체력
    float atk;                              // 공격력

    Animator anim;                          // 애니메이터
    private Transform player;               // 플레이어 객체

    float distance;                         // 플레이어와의 거리
    float attackCoolTime;                   // 공격 쿨타임
    float currentAttackCoolTime;            // 현재 공격 쿨타임

    float chaseDistance;                    // 추적을 시작 할 거리
    float attackDistance;                   // 공격을 할 거리

    int skillCount;                         // 스킬을 쓰기위한 횟수                              
    int currentSkillCount;                  // 스킬을 쓰기위한 현재 횟수  

    int defendCoolTime;                     // 방어(피회복) 쿨타임
    int currentDefendCoolTime;              // 현재 방어(피회복) 쿨타임





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

        attackCoolTime = 5.0f;
        currentAttackCoolTime = 0.0f;

        chaseDistance = 8.0f;
        attackDistance = 2.0f;
        atk = 10.0f;

        skillCount = 3;
        currentSkillCount = 0;
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
            }
        }
    }

    void Idle()
    {
        // 기본
        if (distance <= chaseDistance)
        {
            // 추적거리안에 들어왔을 때(기본 -> 추적)
            enemyState = State.Chase;
        }
    }
    void Chase()
    {
        // 추적
        if (distance > chaseDistance)
        {
            // 추적거리보다 길 때(추적 -> 걷기)
            enemyState = State.Idle;
        }
        else if (distance <= attackDistance)
        {
            // 공격거리보다 짧을 떄(추적 -> 공격)
            enemyState = State.Attack;
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
        }
        else
        {
            // 공격 중
            currentAttackCoolTime -= Time.deltaTime;

            if (currentAttackCoolTime <= 0.0f)
            {
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


        }
    }

    public void Damaged(int damage)
    {
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;

        hpBar.value = currentHp;

        if (currentHp > 0)
        {
            if (enemyState != State.Attack)
                SetEnemyStateAnimator(State.Damage);
            // anim.SetTrigger("Damage");
            // enemyState = State.Damage;
        }
        else
        {
            SetEnemyStateAnimator(State.Dead);
            //  anim.SetTrigger("Dead");
            //  enemyState = State.Dead;
        }
    }

    void DamagedEnd()
    {
        enemyState = State.Idle;
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

    public void SetEnemyStateAnimator(State newState)
    {
        // 현재랑 같으면 넘어감
        if (enemyState == newState)
            return;

        enemyState = newState;

        anim.SetBool("isWalk", false);

        // 상태에 맞는 애니메이터 파라미터 설정
        switch (newState)
        {
            case State.Idle:
                break;
            case State.Chase:
                anim.SetBool("isWalk", true);
                break;
            case State.Attack:
                anim.SetBool("Attack", true);
                break;
            case State.Defend:
                anim.SetBool("Defend", true);
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
