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

    public float maxHp;                     // �ִ� ü��
    float currentHp;                        // ���� ü��
    float atk;                              // ���ݷ�

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
        // �⺻
        if(distance<= chaseDistance)
        {
            // �����Ÿ��ȿ� ������ ��(�⺻ -> ����)
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Idle distance<=8");
        }
        else
        {
            // �����Ÿ� ���� ��(�⺻ -> ��ȸ)
            enemyState = State.Wander;
        }
    }
    void Chase()
    {
        // ����
        if (distance > chaseDistance)
        {
            // �����Ÿ����� �� ��(���� -> �ȱ�)
            enemyState = State.Idle;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance>8");
        }
        else if (distance <= attackDistance)
        {
            // ���ݰŸ����� ª�� ��(���� -> ����)
            enemyState = State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance<=2");
        }
        else
        {
            // �����Ÿ��ȿ� ������ ��(����)
             navMeshAgent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        // ����
        if (distance > attackDistance)
        {
            // ���ݰŸ��ȿ� �������� ��(���� -> ����)
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
            Debug.Log("Attack distance>2");
        }
        else
        {
            // ���� ��
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
        // ��ȸ
        currentWanderCooTime += Time.deltaTime;
        if (currentWanderCooTime >= wanderCooTime)
        {
            Vector3 newPos = RandomNavPos();
            navMeshAgent.SetDestination(newPos);
            currentWanderCooTime = 0.0f;
        }

        if (distance <= chaseDistance)
        {
            // �����Ÿ����� ª�� �� (��ȸ -> ����)
            enemyState = State.Chase;
            navMeshAgent.ResetPath();
        }

    }

    Vector3 RandomNavPos()
    {
        // �� ������ ������ �� ��ȯ. (3D �������� ������ ��ġ ���� ����)
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;


        // SamplePosition �� ������ NavMesh �󿡼� ����� ���� ã��
        NavMeshHit navHit;  // ��ȿ�� ��ġ ��ȯ
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        return navHit.position;
    }

    public void Damaged(int damage)
    {
        Debug.Log("enmey Damage!!!!!");
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
        // ����� ������ �Ѿ
        if (enemyState == newState)
            return;

        enemyState = newState;

        //  anim.SetBool("IsIdle", false);
        anim.SetBool("isWalk", false);
        //anim.SetBool("isAttack", false);
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
