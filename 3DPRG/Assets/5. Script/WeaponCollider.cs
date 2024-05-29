using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public int atk;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player.GetComponent<PlayerAttack>().GetIsAttack() ::: " + player.GetComponent<PlayerAttack>().GetIsAttack());
        if (player.GetComponent<PlayerAttack>().GetIsAttack())
            return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("AttackToEnemy Trigger!!!!!!");
            // 몬스터에게 데미지를 입힙니다.
            player.GetComponent<PlayerAttack>().SetIsAttackToEnemy(true);
            other.gameObject.GetComponent<Enemy>().Damaged(atk);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("player.GetComponent<PlayerAttack>().GetIsAttack() ::: " + player.GetComponent<PlayerAttack>().GetIsAttack());
    //    if (player.GetComponent<PlayerAttack>().GetIsAttack())
    //        return;
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("AttackToEnemy!!!!!!");
    //        // 몬스터에게 데미지를 입힙니다.
    //        player.GetComponent<PlayerAttack>().SetIsAttackToEnemy(true);
    //        collision.gameObject.GetComponent<Enemy>().Damaged(atk);
    //    }

    //}
}
