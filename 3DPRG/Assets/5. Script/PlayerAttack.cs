using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    bool isComboExist = false;                          // �޺��� �����ϴ���
    bool isComboEnable = false;                         // �޺� �Է��� ��������

    Player.State playerState;
    PlayerWeaponChange.WeaponType weaponType;
    

    ArrayList handColliders;
    BoxCollider weaponCollider;

    bool isAttackToEnemy;


    /* ���ݰ��� �ִϸ��̼� �̺�Ʈ ����
     * -> ComboEnable
     * -> ComboDisable
     * -> ComboExist
     * -> AttackEnd
     */


    float atk;                              // ���ݷ�
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
        // ���� ��ư�� ������ ��
        //Debug.Log("weaponType : " + weaponType);
        playerState = GetComponent<Player>().GetPlayerState();
        /* ������ �Ұ����� ���
        - ������
        - ������
        - �±�
        - �Ѿ���
        - ��� */
        if ((playerState == Player.State.Jump) || (playerState == Player.State.DiveRoll) || (playerState == Player.State.Damage) || (playerState == Player.State.Down) || (playerState == Player.State.Dead))
            return;

        // �������� �ִϸ��̼� ����
        GetComponent<Player>().SetPlayerStateAnimator(Player.State.Attack);
        
        // �޺��Է��� ���� �Ҷ� ����Ű�� ��������
        if (isComboEnable)
            isComboExist = true;
    }


    public void ComboEnable()
    {
        // �޺��Է��� ������ ����
        isComboExist = false;
        isComboEnable = true;
    }

    public void ComboDisable()
    {
        // �޺��Է��� �Ұ����� ����
        isComboEnable = false;
    }


    public void ComboExist()
    {
        // �޺��� �����ϴ���

        // �޺��� ������������ ��
        if (isComboExist == false)
            return;

        // �޺��� �����ϸ� ���� �ٽ� false�� �ٲٰ� �޺��ε��� ����
        // �ִϸ������� Ʈ���Ÿ� ����
        isComboExist = false;

        anim.SetTrigger("NextCombo");
    }

    public void AttackEnd()
    {
        // ������ ������ ��

        SetIsAttackToEnemy(false);
        if (isComboExist == false)
        {
            // �޺��� �������� ���� ��
            // �⺻���� ����
            // �������� ����(�Ǽ�) �ڽ� �ݶ��̴����� ���� �ٽ� ��Ȱ��ȭ(���ÿ��� �浹üũ ����)
            GetComponent<Player>().SetPlayerStateAnimator(Player.State.Idle);

            // �Ǽ� �ݶ��̴� ������ 0���� �ƴ϶��
            if(handColliders.Count != 0)
            {
                // �Ǽ� �ݶ��̴��� ��Ȱ��ȭ
                foreach (BoxCollider hand in handColliders)
                    hand.enabled = false;
            }
            // �����ݶ��̴��� null�� �ƴ϶��
            if (weaponCollider != null)
            {
                // ���� �ݶ��̴��� ��Ȱ��ȭ
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
        // ���� �ִϸ��̼� ���߿� ������ ������ ������ ����� ��
        SetIsAttackToEnemy(false);
        GetEqipWeaponCollider();
    }


   void GetEqipWeaponCollider()
    {
        // ������ ������ ������ �����̹Ƿ� �ݶ��̴��� Ȱ��ȭ ���Ѿߵ�
        // �������� ����(�Ǽ�����) �ݶ��̴� Ȱ��ȭ ��Ű��
        // ���� �������� ����Ÿ���� ��������
        var getWeaponType = GameObject.Find("InvenPanel").GetComponent<PlayerWeaponChange>().getEqipWeaponType();
        
        // ���� ���� ����� ������ ���� ���� ��
        if (weaponType == getWeaponType)
        {
            //�̹� ���⿡ ���� ������ ����
            if(handColliders.Count!=0)
            {
                // (�Ǽ�)�ݶ��̴� Ȱ��ȭ
                foreach (BoxCollider hand in handColliders)
                    hand.enabled = true;
            }
                // (����)�ݶ��̴� Ȱ��ȭ
            if (weaponCollider != null)
                weaponCollider.enabled = true;

            return;
        }

        // ������ʹ� ���� ����� �����°��� �ٸ� ��
        // ������ ����Ÿ���� ���� ����Ÿ������
        weaponType = getWeaponType;

        // ����ִ� ���⿡ ���� �������� �ʱ�ȭ ��Ŵ
        handColliders.Clear();
        weaponCollider = null;

        switch (weaponType)
        {
            case PlayerWeaponChange.WeaponType.NoWeapon:
                {
                    // �Ǽ� �� �� 
                    // �Ǽ� �±װ� �پ����ִ� ������Ʈ���� �����´�
                    GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
                    foreach (GameObject hand in hands)
                    {
                        // �Ǽ� �±װ� �پ����ִ� ������Ʈ�� �ڽ��ݶ��̴��� Ȱ��ȭ ��Ŵ
                        hand.GetComponent<BoxCollider>().enabled = true;
                        handColliders.Add(hand.GetComponent<BoxCollider>());
                    }
                }
                break;
                // �Ǽ��� �ƴ� �ٸ� ������ ��
            case PlayerWeaponChange.WeaponType.Sword:
            case PlayerWeaponChange.WeaponType.Axe:
            case PlayerWeaponChange.WeaponType.Mace:
                {
                    // ���� �±װ� �پ����ִ� ������Ʈ�� �����´�
                    // ���� �±װ� �پ����ִ� ������Ʈ�� �ڽ��ݶ��̴��� Ȱ��ȭ ��Ŵ
                    GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
                    weaponCollider = weapon.GetComponent<BoxCollider>();
                    weaponCollider.enabled = true;
                }
                break;
        }
    }
    public void SetIsAttackToEnemy(bool attack)
    {
        // ���� �̹� ������ ���� ����
        isAttackToEnemy = attack;
    }

    public bool GetIsAttack()
    {
        // ���� �̹� ������ ��������
        return isAttackToEnemy;
    }
}
