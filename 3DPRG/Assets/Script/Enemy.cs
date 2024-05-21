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
        Walk,               // �ȱ�(�޸���)
        Attack,             // ����(��ų)
        Damage,             // �±�
        Dead                // �ױ�
    }

    public State enemyState;   // ���� ���� ����
    public Slider hpBar;            // ���� Hp Bar

    public int maxHp;           // �ִ� ü��
    int currentHp;              // ���� ü��

    NavMeshAgent navMeshAgent;
    Animator anim;
    private Transform player; // �÷��̾� ��ü

    float distance;           // �÷��̾���� �Ÿ�

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
        //if (navMeshAgent == null)
        //    return;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);

        switch(enemyState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                Walk();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        if(distance<=8)
        {
            enemyState = State.Walk;
            navMeshAgent.isStopped = false;
            Debug.Log("Idle distance<=8");
        }
    }
    void Walk()
    {
        if (distance > 8)
        {
            enemyState = State.Idle;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Debug.Log("Walk distance>8");
        }
        else if (distance <= 2)
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
        if (distance > 2)
        {
            enemyState = State.Walk;
            navMeshAgent.isStopped = false;
            Debug.Log("Attack distance>2");
        }
        else
            anim.SetBool("isAttack", true);
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
            case State.Walk:
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
