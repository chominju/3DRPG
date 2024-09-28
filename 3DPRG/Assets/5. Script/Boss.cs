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
    int recoveryHp;                           // 피 회복 수치
    int recoveryHpCount;                      // 피회복 횟수
    int currentRecoveryHpCount;               // 현재 피회복 횟수

    bool isUseDefendSkill;                     // 방어스킬 사용했는지 
    bool isDefendSkillAble;                    // 방어스킬 사용가능한지 
    bool isAttack;                             // 공격중인지
    public GameObject GameClearPanel;          // 보스를 잡았을 때 나오는 판넬
    public Slider bossHpBar;                   // 보스의 Hp Bar

   
    int damagedCount;                           // 맞는 횟수
    int currentDamagedCount;                    // 지금 맞은 횟수
    bool isCounterAttackAble;                   // 카운터 공격이 가능한지

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
        recoveryHp = 20;
        currentRecoveryHpCoolTime = 0.0f;
        isUseDefendSkill = false;

        recoveryHpCount = 10;
        currentRecoveryHpCount =0;
        isDefendSkillAble = true;
        isAttack = false;

        damagedCount= 3;
        currentDamagedCount = 0;
        isCounterAttackAble = false;
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
                case State.Dead:
                    Dead();
                    break;
            }
        }

        if (isUseDefendSkill)
        {
            // 방어스킬를 사용했다면, 쿨타임 돌아감
            currentDefendCoolTime -= Time.deltaTime;
            if (currentDefendCoolTime <= 0.0f)
            {
                // 쿨타임이 끝났을 때
                isDefendSkillAble = true;
                isUseDefendSkill = false;
            }
        }
    }

    void Idle()
    {
        // 노피격시 가만히 서있음
    }
    void Chase()
    {
        //// 추적(공격 받았을 때 추적 or 공격)
        if (distance <= attackDistance)
        {
            // 공격가능 범위일때(추적->공격)
            enemyState =  State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
        else
        {
            // 추적중
            navMeshAgent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        // 공격
        if (distance > attackDistance)
        {
            // 공격불가능 범위(공격 -> 추적)
            currentAttackCoolTime = 0.0f;
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // 공격 중
            currentAttackCoolTime -= Time.deltaTime;
            transform.LookAt(player.transform.position);
            if (currentAttackCoolTime <= 0.0f || isCounterAttackAble)
            {
                isAttack = true;
                isCounterAttackAble = false;
                // 일반 공격시 스킬
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

    void Defend()
    {
        // 방어 상태가 아니라면 return.
        if (enemyState != State.Defend)
            return;

        //  방어상태를 사용하지 않았다면
        if (!isUseDefendSkill)
        {
            anim.SetTrigger("Defend");
            anim.SetBool("isDefend", true);
            isUseDefendSkill = true;
            isDefendSkillAble = false;
        }

        // 일정시간마다 회복
        currentRecoveryHpCoolTime -= Time.deltaTime;
        if (currentRecoveryHpCoolTime <= 0.0f)
        {
            // 일정시간마다 회복하는 쿨타임 다시 복구.
            currentRecoveryHpCoolTime = recoveryHpCoolTime;
            // 지금 회복 
            currentHp += recoveryHp;
            DamageManager.GetInstance().CreateDamage(20, Damage.DamageType.Recovery, transform.position); // 회복 폰트 출격

            // 회복된 hp가 최대hp를 넘었다면 최대hp로 고정
            if (currentHp >= maxHp)
                currentHp = maxHp;

            // hp바 조정
            bossHpBar.value = currentHp;
            currentRecoveryHpCount++;

            // 현재 회복횟수 >= 회복하는 횟수
            if(currentRecoveryHpCount >= recoveryHpCount)
            {
                currentDefendCoolTime = defendCoolTime;
                currentRecoveryHpCount = 0;
                anim.SetBool("isDefend", false);

                // 회복을 다 했으면 공격으로 바뀜
                enemyState = State.Attack;
            }
        }
    }

    public void Damaged(int damage)
    {
        // 플레이어한테 맞았을 때
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;
        DamageManager.GetInstance().CreateDamage(damage, Damage.DamageType.Player, transform.position);

        bossHpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // 이동 중단
        navMeshAgent.ResetPath();           // 경로 초기화

        // 피가 0보다 많을 때
        if (currentHp > 0)
        {
            // 방어상태이면 return
            if (anim.GetBool("isDefend") == true)
                return;

            // 데미지 카운트 증가
            currentDamagedCount++;

            // 맞은 횟수가 넘어가면 카운터 가능
            if(currentDamagedCount >=damagedCount)
            {
                // 카운터 어택이 가능함
                isCounterAttackAble = true;
                Attack();
                currentDamagedCount = 0;
                return;
            }

            // 공격중이 아니라면 맞는 모션
            if (!isAttack)
                anim.SetTrigger("Damage");

            // 현재 hp가 절반 밑으로 떨어졌을 때
            if (currentHp <= maxHp / 2)
            {
                // 방어모드 사용이 가능한지
                if (isDefendSkillAble)
                {
                    // 방어모드
                    enemyState = State.Defend;
                    return;
                }
                else
                    Debug.Log("DefendSkill is coolTime");
            }
        }
        else
        {
            // 상태 : 죽음
            enemyState = State.Dead;

            // 클리어 판넬 출력
            GameClearPanel.SetActive(true);
        }
    }

    void DamagedEnd()
    {
        // 데미지 받는게 끝났다면 추적모드
        enemyState = State.Chase;
    }

    void AttackToPlayer()
    {
        // 플레이어한테 공격
        distance = Vector3.Distance(transform.position, player.position);

        // 공격거리보다 멀어졌을때는 데미지가 안들어감(공격모션은 함)
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk);
    }

    void AttackSkillToPlayer()
    {
        // 플레이어한테 공격
        distance = Vector3.Distance(transform.position, player.position);

        // 공격거리보다 멀어졌을때는 데미지가 안들어감(공격모션은 함)
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk * 2);
    }

    void Dead()
    {
        // 죽음
        anim.SetBool("Dead", true);
    }

    void DeadEnd()
    {
        // 죽는 애니메이션이 끝나면 사라짐.
        gameObject.SetActive(false);
    }
        
    void AttackEnd()
    {
        // 공격 애니메이션이 끝났을 때
        isAttack = false;
        isCounterAttackAble = false;
    }
}
