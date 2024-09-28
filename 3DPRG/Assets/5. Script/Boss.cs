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
    int recoveryHp;                           // �� ȸ�� ��ġ
    int recoveryHpCount;                      // ��ȸ�� Ƚ��
    int currentRecoveryHpCount;               // ���� ��ȸ�� Ƚ��

    bool isUseDefendSkill;                     // ��ų ����ߴ��� 
    bool isDefendSkillAble;                    // ��ų ��밡������ 
    bool isAttack;                             // ����������
    public GameObject GameClearPanel;          // ������ ����� �� ������ �ǳ�
    public Slider bossHpBar;                   // ������ Hp Bar

   
    int damagedCount;                           // �´� Ƚ��
    int currentDamagedCount;                    // ���� ���� Ƚ��
    bool isCounterAttackAble;                   // ī���� ������ ��������

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
                case State.Dead:
                    Dead();
                    break;
            }
        }

        if (isUseDefendSkill)
        {
            // ��ų�� ����ߴٸ�, ��Ÿ�� ���ư�
            currentDefendCoolTime -= Time.deltaTime;
            if (currentDefendCoolTime <= 0.0f)
            {
                // ��Ÿ���� ������ ��
                isDefendSkillAble = true;
                isUseDefendSkill = false;
            }
        }
    }

    void Idle()
    {
        // ���ǰݽ� ������ ������
    }
    void Chase()
    {
        //// ����(���� �޾��� �� ���� or ����)
        if (distance <= attackDistance)
        {
            // ���ݰ��� �����϶�(����->����)
            enemyState =  State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
        else
        {
            // ������
            navMeshAgent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        // ����
        if (distance > attackDistance)
        {
            // ���ݺҰ��� ����(���� -> ����)
            currentAttackCoolTime = 0.0f;
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // ���� ��
            currentAttackCoolTime -= Time.deltaTime;
            transform.LookAt(player.transform.position);
            if (currentAttackCoolTime <= 0.0f || isCounterAttackAble)
            {
                isAttack = true;
                isCounterAttackAble = false;
                // �Ϲ� ���ݽ� ��ų
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
        // ��� ���°� �ƴ϶�� return.
        if (enemyState != State.Defend)
            return;

        //  �����¸� ������� �ʾҴٸ�
        if (!isUseDefendSkill)
        {
            anim.SetTrigger("Defend");
            anim.SetBool("isDefend", true);
            isUseDefendSkill = true;
            isDefendSkillAble = false;
        }

        // �����ð����� ȸ��
        currentRecoveryHpCoolTime -= Time.deltaTime;
        if (currentRecoveryHpCoolTime <= 0.0f)
        {
            // �����ð����� ȸ���ϴ� ��Ÿ�� �ٽ� ����.
            currentRecoveryHpCoolTime = recoveryHpCoolTime;
            // ���� ȸ�� 
            currentHp += recoveryHp;
            DamageManager.GetInstance().CreateDamage(20, Damage.DamageType.Recovery, transform.position); // ȸ�� ��Ʈ ���

            // ȸ���� hp�� �ִ�hp�� �Ѿ��ٸ� �ִ�hp�� ����
            if (currentHp >= maxHp)
                currentHp = maxHp;

            // hp�� ����
            bossHpBar.value = currentHp;
            currentRecoveryHpCount++;

            // ���� ȸ��Ƚ�� >= ȸ���ϴ� Ƚ��
            if(currentRecoveryHpCount >= recoveryHpCount)
            {
                currentDefendCoolTime = defendCoolTime;
                currentRecoveryHpCount = 0;
                anim.SetBool("isDefend", false);

                // ȸ���� �� ������ �������� �ٲ�
                enemyState = State.Attack;
            }
        }
    }

    public void Damaged(int damage)
    {
        // �÷��̾����� �¾��� ��
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;
        DamageManager.GetInstance().CreateDamage(damage, Damage.DamageType.Player, transform.position);

        bossHpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // �̵� �ߴ�
        navMeshAgent.ResetPath();           // ��� �ʱ�ȭ

        // �ǰ� 0���� ���� ��
        if (currentHp > 0)
        {
            // �������̸� return
            if (anim.GetBool("isDefend") == true)
                return;

            // ������ ī��Ʈ ����
            currentDamagedCount++;

            // ���� Ƚ���� �Ѿ�� ī���� ����
            if(currentDamagedCount >=damagedCount)
            {
                // ī���� ������ ������
                isCounterAttackAble = true;
                Attack();
                currentDamagedCount = 0;
                return;
            }

            // �������� �ƴ϶�� �´� ���
            if (!isAttack)
                anim.SetTrigger("Damage");

            // ���� hp�� ���� ������ �������� ��
            if (currentHp <= maxHp / 2)
            {
                // ����� ����� ��������
                if (isDefendSkillAble)
                {
                    // �����
                    enemyState = State.Defend;
                    return;
                }
                else
                    Debug.Log("DefendSkill is coolTime");
            }
        }
        else
        {
            // ���� : ����
            enemyState = State.Dead;

            // Ŭ���� �ǳ� ���
            GameClearPanel.SetActive(true);
        }
    }

    void DamagedEnd()
    {
        // ������ �޴°� �����ٸ� �������
        enemyState = State.Chase;
    }

    void AttackToPlayer()
    {
        // �÷��̾����� ����
        distance = Vector3.Distance(transform.position, player.position);

        // ���ݰŸ����� �־��������� �������� �ȵ�(���ݸ���� ��)
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk);
    }

    void AttackSkillToPlayer()
    {
        // �÷��̾����� ����
        distance = Vector3.Distance(transform.position, player.position);

        // ���ݰŸ����� �־��������� �������� �ȵ�(���ݸ���� ��)
        if (distance > attackDistance)
            return;
        player.GetComponent<Player>().Damaged(atk * 2);
    }

    void Dead()
    {
        // ����
        anim.SetBool("Dead", true);
    }

    void DeadEnd()
    {
        // �״� �ִϸ��̼��� ������ �����.
        gameObject.SetActive(false);
    }
        
    void AttackEnd()
    {
        // ���� �ִϸ��̼��� ������ ��
        isAttack = false;
        isCounterAttackAble = false;
    }
}
