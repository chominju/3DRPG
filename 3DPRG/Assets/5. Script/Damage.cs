using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private float destroyTime;                                  // 삭제시간
    TextMeshProUGUI textMeshProComp;                            // 컴포넌트
    int damage;                                                 // 데미지
    float timer;                                                // 시간

    public enum DamageType
    {
        Player,                                                 // 플레이어 공격
        Enemy,                                                  // 적 공격
        Recovery,                                               // 회복
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
            // 상태별 색상 변경
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
        // 적보다 살짝 위쪽에 생성
        Vector3 damagePos = Camera.main.WorldToScreenPoint(new Vector3(enemyPos.x, enemyPos.y+1.5f, enemyPos.z));
        gameObject.GetComponent<RectTransform>().position = damagePos;
    }

    // Update is called once per frame
    void Update()
    {
        // 1초 뒤에 데미지는 사라짐
        timer += Time.deltaTime;
        if (timer >= destroyTime)
            DestroyObject();

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
