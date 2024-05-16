using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int hp;


    Animator anim;
    private GameObject player; // 플레이어 객체

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
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
