using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    public enum State
    {
        Idle,               // �⺻
        Chase,              // ����
        Attack,             // ����(��ų)
        Defend,              // ���(��ȸ��)
        Damage,             // �±�
        Dead                // �ױ�
    }

    public State enemyState;                // ���� ������ ����

    public float maxHp;                     // �ִ� ü��
    float currentHp;                        // ���� ü��
    float atk;                              // ���ݷ�

    NavMeshAgent navMeshAgent;              // �׺�޽�

    Animator anim;                          // �ִϸ�����
    private Transform player;               // �÷��̾� ��ü

    float distance;                         // �÷��̾���� �Ÿ�
    float attackCoolTime;                   // ���� ��Ÿ��
    float currentAttackCoolTime;            // ���� ���� ��Ÿ��

    float attackDistance;                   // ������ �� �Ÿ�

    int skillCount;                         // ��ų�� �������� Ƚ��                              
    int currentSkillCount;                  // ��ų�� �������� ���� Ƚ��  

    float defendCoolTime;                     // ��ų ��Ÿ��
    float currentDefendCoolTime;              // ���� ��ų ��Ÿ��
    float recoveryHpCoolTime;                 // ��ȸ�� ��Ÿ��
    float currentRecoveryHpCoolTime;          // ��ȸ�� ��Ÿ��
    int recoveryHpCount;                      // ��ȸ�� Ƚ��
    int currentRecoveryHpCount;                      // ���� ��ȸ�� Ƚ��

    bool isUseDefend;                         // ��ų ����ߴ��� 
    bool isDefendAble;                         // ��ų ��밡������ 
    bool isAttack;
    public GameObject GameClearPanel;          // ������ ����� �� ������ �ǳ�
    public Slider bossHpBar;                   // ������ Hp Bar

    int damagedCount;
    int currentDamagedCount;
    bool counterAttack;

    void Start()
    {
        // ��ҿ��� ��Ȱ��ȭ �ƴٰ�, ������ �����Ǹ� Ȱ��ȭ
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
        // �÷��̾ ���� �� ��
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
        //// �⺻
        //if (distance <= chaseDistance)
        //{
        //    // �����Ÿ��ȿ� ������ ��(�⺻ -> ����)
        //    enemyState = State.Chase;
        //}
    }
    void Chase()
    {
        //// ����
        //if (distance > chaseDistance)
        //{
        //    // �����Ÿ����� �� ��(���� -> �ȱ�)
        //    enemyState = State.Idle;
        //}
        if (distance <= attackDistance)
        {
            // ���ݰŸ����� ª�� ��(���� -> ����)
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
        // ����
        if (distance > attackDistance)
        {
            // ���ݰŸ��ȿ� �������� ��(���� -> ����)
            currentAttackCoolTime = 0.0f;
            //SetEnemyStateAnimator(State.Chase);
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // ���� ��
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
        // ��� ���°� �ƴ϶�� return.
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

        navMeshAgent.isStopped = true;      // �̵� �ߴ�
        navMeshAgent.ResetPath();           // ��� �ʱ�ȭ
        // �ǰ� 0���� ���� ��
        if (currentHp > 0)
        {
            if (anim.GetBool("isDefend") == true)
                return;
            currentDamagedCount++;
            if(currentDamagedCount >=damagedCount)
            {
                Debug.Log("�ݰ�!!!");

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
            // �׾��� ��
            anim.SetBool("Dead", true);
            //SetEnemyStateAnimator(State.Dead);
            GameClearPanel.SetActive(true);
        }
    }

    void DamagedEnd()
    {
        enemyState = State.Chase;
        //SetEnemyStateAnimator(State.Chase);
        //// �������� �� �޾��� ��, hp�� ���� ������ �� 
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
    //    // ����� ������ �Ѿ
    //    if (enemyState == newState)
    //        return;

    //    enemyState = newState;

    //    anim.SetBool("isWalk", false);

    //    // ���¿� �´� �ִϸ����� �Ķ���� ����
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
