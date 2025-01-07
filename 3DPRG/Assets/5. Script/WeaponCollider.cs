using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public int atk;
    GameObject player;


    public Color activeColor = Color.blue;              // �ڽ��ݶ��̴� Ȱ��ȭ�� ����� �Ķ���
    public Color inactiveColor = Color.white;           // �ڽ��ݶ��̴� ��Ȱ��ȭ�� ����� ���


    private BoxCollider col;                            // �ڽ��ݶ��̴� ������Ʈ

    List<GameObject> enemyList;

    void Start()
    {
        player = GameObject.Find("Player");

        col = GetComponent<BoxCollider>();
        enemyList = new List<GameObject>();
    }

    void OnDrawGizmos()
    {
        // ����� �׸���
        if (col == null)
        {
            col = GetComponent<BoxCollider>();
        }

        // �ݶ��̴��� Ȱ��ȭ�� ���
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

        // ������ ������ ������ (�Ұ����ϴٸ� return)
        //if (player.GetComponent<PlayerAttack>().GetIsAttack())
        //    return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            foreach (var enemy in enemyList)
            {
                if (enemy == other.gameObject)
                    return;
            }
            // Ʈ������ ����� ���̶�� �������� �����ϴ�.
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
            // Ʈ������ ����� ���̶�� �������� �����ϴ�.
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
