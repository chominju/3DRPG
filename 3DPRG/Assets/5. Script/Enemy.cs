using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum State
    {
        Idle,               // �⺻
        Wander,             // ��ȸ
        Chase,              // ����
        Attack,             // ����(��ų)
        Damage,             // �±�
        Dead                // �ױ�
    }

    public State enemyState;                // ���� ���� ����
    public Slider hpBar;                    // ���� Hp Bar

    public int maxHp;                       // �ִ� ü��
    int currentHp;                          // ���� ü��

    NavMeshAgent navMeshAgent;              // �׺�޽�
    Animator anim;                          // �ִϸ�����
    private Transform player;               // �÷��̾� ��ü

    float distance;                         // �÷��̾���� �Ÿ�
    float attackCoolTime;                   // ���� ��Ÿ��
    float currentAttackCoolTime;            // ���� ���� ��Ÿ��
    float wanderCooTime;                    // ��ȸ ��Ÿ��
    float currentWanderCooTime;             // ���� ��ȸ ��Ÿ��

    float wanderRadius;                     // ��ȸ �ݰ�
    float chaseDistance;                    // ������ ���� �� �Ÿ�
    float attackDistance;                   // ������ �� �Ÿ�



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
        // �⺻
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
        // �ȱ�
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
        // ����
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
        // ��ȸ
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

        navMeshAgent.isStopped = true;      // �̵� �ߴ�
        navMeshAgent.ResetPath();           // ��� �ʱ�ȭ

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
            // ���� �÷��̾��� �浹 ���� �˻�
            if (IsPlayerInAttackRange())
            {
                Debug.Log("Attack!!!");
                //// �÷��̾�� �������� ����
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
        // ������ �浹 �Ÿ�. ���� �÷��̾��� �Ÿ� �������� ���� �ʿ�
        return distance < 1.5f;
    }

    public void SetEnemyStateAnimator(State newState)
    {
        // ����� ������ �Ѿ
        if (enemyState == newState)
            return;

        enemyState = newState;

        //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);
        //  anim.SetBool("IsDamage", false);
        //  anim.SetBool("IsDown", false);
        //  anim.SetBool("IsDead", false);

        // ���¿� �´� �ִϸ����� �Ķ���� ����
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
