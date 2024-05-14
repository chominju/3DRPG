using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    bool isComboExist = false;
    bool isComboEnable = false;
    int comboIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (anim.GetBool("isWalk") == true)
            anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);
        if (isComboEnable)
            isComboExist = true;
    }

    public void ComboEnable()
    {
        isComboExist = false;
        isComboEnable = true;
    }

    public void ComboDisable()
    {
        isComboEnable = false;
    }


    public void ComboExist()
    {
        if (isComboExist == false)
            return;

        isComboExist = false;
        comboIndex++;

        anim.SetTrigger("NextCombo");
    }

    public void AttackEnd()
    {
        Debug.Log("AttackEnd");
        if (isComboExist == false)
        {
            anim.SetBool("isAttack", false);
            comboIndex = 0;

        }
    }

    public void ComboAttackEnd()
    {
        anim.SetBool("isAttack", false);
        comboIndex = 0;
    }
}
