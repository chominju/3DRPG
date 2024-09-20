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

    public State enemyState;                // ���� ���� ����
    public Slider hpBar;                    // ���� Hp Bar

    public float maxHp;                     // �ִ� ü��
    float currentHp;                        // ���� ü��
    float atk;                              // ���ݷ�

    Animator anim;                          // �ִϸ�����
    private Transform player;               // �÷��̾� ��ü

    float distance;                         // �÷��̾���� �Ÿ�
    float attackCoolTime;                   // ���� ��Ÿ��
    float currentAttackCoolTime;            // ���� ���� ��Ÿ��

    float chaseDistance;                    // ������ ���� �� �Ÿ�
    float attackDistance;                   // ������ �� �Ÿ�

    int skillCount;                         // ��ų�� �������� Ƚ��                              
    int currentSkillCount;                  // ��ų�� �������� ���� Ƚ��  

    int defendCoolTime;                     // ���(��ȸ��) ��Ÿ��
    int currentDefendCoolTime;              // ���� ���(��ȸ��) ��Ÿ��





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
            }
        }
    }

    void Idle()
    {
        // �⺻
        if (distance <= chaseDistance)
        {
            // �����Ÿ��ȿ� ������ ��(�⺻ -> ����)
            enemyState = State.Chase;
        }
    }
    void Chase()
    {
        // ����
        if (distance > chaseDistance)
        {
            // �����Ÿ����� �� ��(���� -> �ȱ�)
            enemyState = State.Idle;
        }
        else if (distance <= attackDistance)
        {
            // ���ݰŸ����� ª�� ��(���� -> ����)
            enemyState = State.Attack;
        }
    }

    void Attack()
    {
        // ����
        if (distance > attackDistance)
        {
            // ���ݰŸ��ȿ� �������� ��(���� -> ����)
            currentAttackCoolTime = 0.0f;
            enemyState = State.Chase;
        }
        else
        {
            // ���� ��
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
        // ����� ������ �Ѿ
        if (enemyState == newState)
            return;

        enemyState = newState;

        anim.SetBool("isWalk", false);

        // ���¿� �´� �ִϸ����� �Ķ���� ����
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
