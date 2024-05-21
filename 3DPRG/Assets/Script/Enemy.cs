using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,               // �⺻
        Walk,               // �ȱ�(�޸���)
        Attack,             // ����(��ų)
        Damage,             // �±�
        Dead                // �ױ�
    }

    public EnemyState state;   // ���� ���� ����
    public Slider hpBar;            // ���� Hp Bar

    public int maxHp;           // �ִ� ü��
    int currentHp;              // ���� ü��

    //NavMeshAgent navMeshAgent;
    Animator anim;
    private GameObject player; // �÷��̾� ��ü

    float distance;           // �÷��̾���� �Ÿ�

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.Idle;
        currentHp = maxHp;
        hpBar.maxValue = maxHp;

        anim = GetComponent<Animator>();
        if (anim == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        //navMeshAgent = GetComponent<NavMeshAgent>();
        //if (navMeshAgent == null)
        //    return;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        switch(state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Walk:
                Walk();
                break;
            case EnemyState.Attack:
                AttackAnim();
                break;
        }
    }

    void Idle()
    {
        if(distance<=8)
        {
            state = EnemyState.Walk;
            //navMeshAgent.isStopped = false;
        }
    }
    void Walk()
    {
        if (distance > 8)
        {
            state = EnemyState.Idle;
            //navMeshAgent.isStopped = true;
           // navMeshAgent.ResetPath();
        }
        else if(distance <=2)
        {
            state = EnemyState.Attack;
           // navMeshAgent.isStopped = true;
           // navMeshAgent.ResetPath();
        }
        else
        {
           // navMeshAgent.SetDestination(player.transform.position);
        }
    }

    void Attack()
    {
        if (distance > 2)
        {
            state = EnemyState.Walk;
           // navMeshAgent.isStopped = false;
        }
        else
            anim.SetBool("isAttack", true);
    }


    void Damaged(int damage)
    {
        currentHp -= damage;

        hpBar.value = currentHp;

       // navMeshAgent.isStopped = true;      // �̵� �ߴ�
       // navMeshAgent.ResetPath();           // ��� �ʱ�ȭ

        if(currentHp > 0)
        {
            anim.SetTrigger("Damage");
            state = EnemyState.Damage;
        }
        else
        {
            anim.SetTrigger("Dead");
            state = EnemyState.Dead;
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
