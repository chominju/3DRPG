using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int hp;


    Animator anim;
    private GameObject player; // �÷��̾� ��ü

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
