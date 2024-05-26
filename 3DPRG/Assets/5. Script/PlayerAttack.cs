using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    bool isComboExist = false;
    bool isComboEnable = false;
    int comboIndex = 0;

    PlayerMove.State playerState;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        playerState = GetComponent<PlayerMove>().GetPlayerState();
        /* 공격이 불가능한 경우
     - 점프중
     - 구르기
     - 맞기
     - 넘어짐
     - 사망 */
        if ((playerState == PlayerMove.State.Jump) || (playerState == PlayerMove.State.DiveRoll) || (playerState == PlayerMove.State.Damage) || (playerState == PlayerMove.State.Down) || (playerState == PlayerMove.State.Dead))
            return;

        GetComponent<PlayerMove>().SetPlayerStateAnimator(PlayerMove.State.Attack);
        
        //if (anim.GetBool("isWalk") == true)
        //    anim.SetBool("isWalk", false);
        //anim.SetBool("isAttack", true);
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
            GetComponent<PlayerMove>().SetPlayerStateAnimator(PlayerMove.State.Idle);
            //anim.SetBool("isAttack", false);
            comboIndex = 0;

        }
    }

    public void ComboAttackEnd()
    {
        GetComponent<PlayerMove>().SetPlayerStateAnimator(PlayerMove.State.Idle);
        //anim.SetBool("isAttack", false);
        comboIndex = 0;
    }
}
