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

    int skillCount;                         // ��ų�� �������� Ƚ��                              
    int currentSkillCount;                  // ��ų�� �������� ���� Ƚ��                              


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
        // �÷��̾ ���� �� �� ���°� ����
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
            // �÷��̾ �������� �ʴ´ٸ� ��ȸ�Ѵ�.
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
        }
        else if (distance <= attackDistance)
        {
            // ���ݰŸ����� ª�� ��(���� -> ����)

           enemyState = State.Attack;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
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
            currentAttackCoolTime = 0.0f;
            enemyState = State.Chase;
            navMeshAgent.isStopped = false;
        }
        else
        {
            // ���� ��
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
        Debug.Log("player -> enemy Attack");
        currentHp -= damage;
        DamageManager.GetInstance().CreateDamage(damage , Damage.DamageType.Player , transform.position);
        hpBar.value = currentHp;

        navMeshAgent.isStopped = true;      // �̵� �ߴ�
        navMeshAgent.ResetPath();           // ��� �ʱ�ȭ

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
    