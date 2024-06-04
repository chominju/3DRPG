using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    bool isComboExist = false;                          // 콤보가 존재하는지
    bool isComboEnable = false;                         // 콤보 입력이 가능한지

    Player.State playerState;
    PlayerWeaponChange.WeaponType weaponType;
    

    ArrayList handColliders;
    BoxCollider weaponCollider;

    bool isAttackToEnemy;


    /* 공격관련 애니메이션 이벤트 순서
     * -> ComboEnable
     * -> ComboDisable
     * -> ComboExist
     * -> AttackEnd
     */


    float atk;                              // 공격력
    // Start is called before the first frame update
    void Start()
    {
        handColliders = new ArrayList();
        weaponType = PlayerWeaponChange.WeaponType.None;
        SetIsAttackToEnemy(false);
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        // 공격 버튼을 눌렀을 때
        //Debug.Log("weaponType : " + weaponType);
        playerState = GetComponent<Player>().GetPlayerState();
        /* 공격이 불가능한 경우
        - 점프중
        - 구르기
        - 맞기
        - 넘어짐
        - 사망 */
        if ((playerState == Player.State.Jump) || (playerState == Player.State.DiveRoll) || (playerState == Player.State.Damage) || (playerState == Player.State.Down) || (playerState == Player.State.Dead))
            return;

        // 공격으로 애니메이션 세팅
        GetComponent<Player>().SetPlayerStateAnimator(Player.State.Attack);
        
        // 콤보입력이 가능 할때 공격키를 눌렀는지
        if (isComboEnable)
            isComboExist = true;
    }


    public void ComboEnable()
    {
        // 콤보입력이 가능한 시점
        isComboExist = false;
        isComboEnable = true;
    }

    public void ComboDisable()
    {
        // 콤보입력이 불가능한 시점
        isComboEnable = false;
    }


    public void ComboExist()
    {
        // 콤보가 존재하는지

        // 콤보가 존재하지않을 때
        if (isComboExist == false)
            return;

        // 콤보가 존재하면 값을 다시 false로 바꾸고 콤보인데스 증가
        // 애니메이터의 트리거를 실행
        isComboExist = false;

        anim.SetTrigger("NextCombo");
    }

    public void AttackEnd()
    {
        // 공격이 끝났을 때

        SetIsAttackToEnemy(false);
        if (isComboExist == false)
        {
            // 콤보가 존재하지 않을 때
            // 기본으로 변경
            // 장착중인 무기(맨손) 박스 콜라이더들을 전부 다시 비활성화(평상시에는 충돌체크 안함)
            GetComponent<Player>().SetPlayerStateAnimator(Player.State.Idle);

            // 맨손 콜라이더 갯수가 0개가 아니라면
            if(handColliders.Count != 0)
            {
                // 맨손 콜라이더를 비활성화
                foreach (BoxCollider hand in handColliders)
                    hand.enabled = false;
            }
            // 무기콜라이더가 null이 아니라면
            if (weaponCollider != null)
            {
                // 무기 콜라이더를 비활성화
                weaponCollider.enabled = false;
            }

        }
    }

    //public void ComboAttackEnd()
    //{
    //    GetComponent<PlayerMove>().SetPlayerStateAnimator(PlayerMove.State.Idle);
    //}


    void AttackToEnemy()
    {
        // 공격 애니메이션 도중에 적에게 공격이 가능한 모션일 때
        SetIsAttackToEnemy(false);
        GetEqipWeaponCollider();
    }


   void GetEqipWeaponCollider()
    {
        // 적에게 공격이 가능한 상태이므로 콜라이더를 활성화 시켜야됨
        // 장착중인 무기(맨손포함) 콜라이더 활성화 시키기
        // 현재 장착중인 무기타입을 가져오기
        var getWeaponType = GameObject.Find("InvenPanel").GetComponent<PlayerWeaponChange>().getEqipWeaponType();
        
        // 지금 현재 무기랑 가져온 값이 같을 때
        if (weaponType == getWeaponType)
        {
            //이미 무기에 대한 정보가 있음
            if(handColliders.Count!=0)
            {
                // (맨손)콜라이더 활성화
                foreach (BoxCollider hand in handColliders)
                    hand.enabled = true;
            }
                // (무기)콜라이더 활성화
            if (weaponCollider != null)
                weaponCollider.enabled = true;

            return;
        }

        // 여기부터는 현재 무기랑 가져온값이 다를 때
        // 가져온 무기타입을 현재 무기타입으로
        weaponType = getWeaponType;

        // 들어있는 무기에 대한 정보들을 초기화 시킴
        handColliders.Clear();
        weaponCollider = null;

        switch (weaponType)
        {
            case PlayerWeaponChange.WeaponType.NoWeapon:
                {
                    // 맨손 일 떄 
                    // 맨손 태그가 붙어져있는 오브젝트들을 가져온다
                    GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
                    foreach (GameObject hand in hands)
                    {
                        // 맨손 태그가 붙어져있는 오브젝트의 박스콜라이더를 활성화 시킴
                        hand.GetComponent<BoxCollider>().enabled = true;
                        handColliders.Add(hand.GetComponent<BoxCollider>());
                    }
                }
                break;
                // 맨손이 아닌 다른 무기일 때
            case PlayerWeaponChange.WeaponType.Sword:
            case PlayerWeaponChange.WeaponType.Axe:
            case PlayerWeaponChange.WeaponType.Mace:
                {
                    // 무기 태그가 붙어져있는 오브젝트를 가져온다
                    // 무기 태그가 붙어져있는 오브젝트의 박스콜라이더를 활성화 시킴
                    GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
                    weaponCollider = weapon.GetComponent<BoxCollider>();
                    weaponCollider.enabled = true;
                }
                break;
        }
    }
    public void SetIsAttackToEnemy(bool attack)
    {
        // 적을 이미 공격한 상태 변경
        isAttackToEnemy = attack;
    }

    public bool GetIsAttack()
    {
        // 적을 이미 공격한 상태인지
        return isAttackToEnemy;
    }
}
