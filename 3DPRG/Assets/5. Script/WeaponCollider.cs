using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public int atk;
    GameObject player;


    public Color activeColor = Color.blue;              // 박스콜라이더 활성화시 기즈모 파란색
    public Color inactiveColor = Color.white;           // 박스콜라이더 비활성화시 기즈모 흰색


    private BoxCollider col;                            // 박스콜라이더 컴포넌트

    List<GameObject> enemyList;

    void Start()
    {
        player = GameObject.Find("Player");

        col = GetComponent<BoxCollider>();
        enemyList = new List<GameObject>();
    }

    void OnDrawGizmos()
    {
        // 기즈모를 그린다
        if (col == null)
        {
            col = GetComponent<BoxCollider>();
        }

        // 콜라이더가 활성화된 경우
        if (col.enabled)
        {
                Gizmos.color = activeColor;
        }
        else
        {
            Gizmos.color = inactiveColor;
        }
        DrawColliderGizmo();
    }

    void DrawColliderGizmo()
    {
        Gizmos.matrix = Matrix4x4.TRS(col.transform.position, col.transform.rotation, col.transform.lossyScale);

        Gizmos.DrawWireCube(col.center, col.size);
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.GetComponent<PlayerAttack>().GetAttackAbleAnimEnd())
            return;

        // 공격이 가능한 것인지 (불가능하다면 return)
        //if (player.GetComponent<PlayerAttack>().GetIsAttack())
        //    return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            foreach (var enemy in enemyList)
            {
                if (enemy == other.gameObject)
                    return;
            }
            // 트리거한 대상이 적이라면 데미지를 입힙니다.
            enemyList.Add(other.gameObject);
            //player.GetComponent<PlayerAttack>().SetIsAttackToEnemy(true);
            other.gameObject.GetComponent<Enemy>().Damaged(atk);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            foreach (var enemy in enemyList)
            {
                if (enemy == other.gameObject)
                    return;
            }
            // 트리거한 대상이 적이라면 데미지를 입힙니다.
            enemyList.Add(other.gameObject);
           // player.GetComponent<PlayerAttack>().SetIsAttackToEnemy(true);
            other.gameObject.GetComponent<Boss>().Damaged(atk);
        }

    }

    public void ResetEnemyList()
    {
        enemyList.Clear();
    }
}
