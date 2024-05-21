using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,               // 기본
        Walk,               // 걷기(달리기)
        Attack,             // 공격(스킬)
        Damage,             // 맞기
        Dead                // 죽기
    }

    public EnemyState state;   // 현재 적의 상태
    public Slider hpBar;            // 적의 Hp Bar

    public int maxHp;           // 최대 체력
    int currentHp;              // 현재 체력

    //NavMeshAgent navMeshAgent;
    Animator anim;
    private GameObject player; // 플레이어 객체

    float distance;           // 플레이어와의 거리

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

       // navMeshAgent.isStopped = true;      // 이동 중단
       // navMeshAgent.ResetPath();           // 경로 초기화

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
            // 적과 플레이어의 충돌 여부 검사
            if (IsPlayerInAttackRange())
            {
                Debug.Log("Attack!!!");
                //// 플레이어에게 데미지를 입힘
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
        // 임의의 충돌 거리. 적과 플레이어의 거리 기준으로 수정 필요
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
