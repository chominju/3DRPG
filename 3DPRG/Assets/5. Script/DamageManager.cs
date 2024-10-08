using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static DamageManager instance = null;
    
    public GameObject damagePrefab;                                // 데미지 프리팹
    public GameObject canvasDamageObject;                          // 데미지 띄우려는 캔버스

    void Start()
    {
        if (instance == null)
            instance = this;

        if (canvasDamageObject == null)
            canvasDamageObject = GameObject.Find("Canvas");

    }

    public static DamageManager GetInstance()
    {
        return instance;
    }

    public void CreateDamage(int damage, Damage.DamageType type, Vector3 pos)
    {
        GameObject damageObject = Instantiate(damagePrefab, canvasDamageObject.transform);
        // 위치조정(몬스터 위)
        damageObject.GetComponent<Damage>().SetDamagePos(pos);
        // 표시 데미지+색상
        damageObject.GetComponent<Damage>().SetDamage(damage, type);
    }
}
