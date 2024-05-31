using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public int atk;
    GameObject player;


    public Color activeColor = Color.blue;
    public Color inactiveColor = Color.white;


    private BoxCollider col;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        col = GetComponent<BoxCollider>();
    }

    void OnDrawGizmos()
    {
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

        // 콜라이더 유형에 따라 다르게 그립니다.
        //Gizmos.DrawWireCube(col.center + transform.position, col.size);
        DrawColliderGizmo();
    }

    void DrawColliderGizmo()
    {
        Gizmos.matrix = Matrix4x4.TRS(col.transform.position, col.transform.rotation, col.transform.lossyScale);

        Gizmos.DrawWireCube(col.center, col.size);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerEnter : " + player.GetComponent<PlayerAttack>().GetIsAttack());
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("AttackToEnemy Trigger OUT!!!!!!");
        }
    }
}
