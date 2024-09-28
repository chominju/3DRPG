using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private float destroyTime;                                  // �����ð�
    TextMeshProUGUI textMeshProComp;                            // ������Ʈ
    int damage;                                                 // ������
    float timer;                                                // �ð�

    public enum DamageType
    {
        Player,                                                 // �÷��̾� ����
        Enemy,                                                  // �� ����
        Recovery,                                               // ȸ��
    }

    void Start()
    {
        timer = 0;
        destroyTime = 1.0f;

        if (textMeshProComp == null)
            textMeshProComp = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(int setDamage, DamageType type)
    {
        damage = setDamage;
        if (textMeshProComp == null)
            textMeshProComp = GetComponent<TextMeshProUGUI>();
        if (textMeshProComp != null)
            textMeshProComp.text = damage.ToString();   

        switch (type)
        {
            // ���º� ���� ����
            case DamageType.Player:
                textMeshProComp.color = Color.blue;
                break;
            case DamageType.Enemy:
                textMeshProComp.color = Color.red;
                break;
            case DamageType.Recovery:
                textMeshProComp.color = Color.green;
                break;
        }
    }

    public void SetDamagePos(Vector3 enemyPos)
    {
        // ������ ��¦ ���ʿ� ����
        Vector3 damagePos = Camera.main.WorldToScreenPoint(new Vector3(enemyPos.x, enemyPos.y+1.5f, enemyPos.z));
        gameObject.GetComponent<RectTransform>().position = damagePos;
    }

    // Update is called once per frame
    void Update()
    {
        // 1�� �ڿ� �������� �����
        timer += Time.deltaTime;
        if (timer >= destroyTime)
            DestroyObject();

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
