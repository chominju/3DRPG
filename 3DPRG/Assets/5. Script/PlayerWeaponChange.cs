using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponChange : MonoBehaviour
{
    enum WeaponType
    {
        NoWeapon,           // �Ǽ�
        Sword,              // Į
        Axe,                // ����
        Mace,               // ��ġ
        Shield              // ����
    }

    WeaponType currentWeapon;

    bool isSwordEqip = false;                  // Į�� �������ΰ�
    bool isAxeEqip = false;                    // ������ �������ΰ�
    bool isMaceEqip = false;                   // ��ġ�� �������ΰ�
    bool isShieldEqip = false;                 // ���и� �������ΰ�

    public GameObject swordButton;             // Į ��ư 
    public GameObject axeButton;               // ���� ��ư
    public GameObject maceButton;              // ��ġ ��ư
    public GameObject shieldButton;            // ���� ��ư

    public GameObject swordWeapon;             // Į ������Ʈ     
    public GameObject axeWeapon;               // ���� ������Ʈ
    public GameObject maceWeapon;              // ��ġ ������Ʈ
    public GameObject shieldWeapon;            // ���� ������Ʈ

    public RuntimeAnimatorController noWeaponAnim;  // �Ǽ� �ִϸ�����
    public RuntimeAnimatorController oneWeaponAnim; // �Ѽ� �ִϸ�����(Į, ����, ��ġ)
    public RuntimeAnimatorController twoWeaponAnim; // �μ� �ִϸ�����(Į+����)

    Animator anim;                                  // �÷��̾� �ִϸ����� ������Ʈ

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.Log("Player no Find");
        anim = player.GetComponent<Animator>();

        currentWeapon = WeaponType.NoWeapon;
        // ��ư�� Ŭ�� �̺�Ʈ ������ �߰�
        swordButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Sword));
        axeButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Axe));
        maceButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Mace));
        shieldButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Shield));
    }

    void WeaponButtonClick(WeaponType clickType)
    {
        // Ŭ���Ѱ� ������
        if (clickType == WeaponType.Shield)
        {
            // Į�� �����ߴ°�?
            if (isSwordEqip)
                isShieldEqip = !isShieldEqip;
            else
                isShieldEqip = false;
        }
        else
        {
            // ��� ���� Ŭ�� ���� �ʱ�ȭ
            isSwordEqip = false;
            isAxeEqip = false;
            isMaceEqip = false;

            // ������ ���� ���Ⱑ ���� ��
            if ((currentWeapon == clickType))
            {
                if (currentWeapon == WeaponType.Sword)
                {
                    isShieldEqip = false;
                }
                currentWeapon = WeaponType.NoWeapon;
            }
            else
            {
                currentWeapon = clickType;
                // ������ ���� ����� ���°� �ٸ��ٸ� ����
                switch (currentWeapon)
                {
                    case WeaponType.Sword:
                        isSwordEqip = true;
                        break;
                    case WeaponType.Axe:
                        isAxeEqip = true;
                        break;
                    case WeaponType.Mace:
                        isMaceEqip = true;
                        break;
                    default:
                        break;
                }
            }
        }

        // ���¿� ���� ���� Ȱ��ȭ/��Ȱ��ȭ
        SetActiveWeapon();
        SetAnimWeapon();
        SetButtonColor();
    }

    void SetActiveWeapon()
    {
        // ������ Ȱ��ȭ ���¸� ������Ʈ
        swordWeapon.SetActive(isSwordEqip);
        axeWeapon.SetActive(isAxeEqip);
        maceWeapon.SetActive(isMaceEqip);
        shieldWeapon.SetActive(isShieldEqip);
    }

    void SetAnimWeapon()
    {
        // ���⿡ ���� �ִϸ����� ����
        if(isSwordEqip && isShieldEqip)
            anim.runtimeAnimatorController = twoWeaponAnim;
        else if (isSwordEqip || isAxeEqip || isMaceEqip)
            anim.runtimeAnimatorController = oneWeaponAnim;
        else
            anim.runtimeAnimatorController = noWeaponAnim;
    }

    void SetButtonColor()
    {
        swordButton.GetComponent<Image>().color = Color.white;
        axeButton.GetComponent<Image>().color = Color.white;
        maceButton.GetComponent<Image>().color = Color.white;
        shieldButton.GetComponent<Image>().color = Color.white;

        switch (currentWeapon)
        {
            case WeaponType.Sword:
                swordButton.GetComponent<Image>().color = Color.gray;
                break;
            case WeaponType.Axe:
                axeButton.GetComponent<Image>().color = Color.gray;
                break;
            case WeaponType.Mace:
                maceButton.GetComponent<Image>().color = Color.gray;
                break;
            default:
                break;
        }

        if(isShieldEqip)
            shieldButton.GetComponent<Image>().color = Color.gray;
    }





    // ��������

    //// Start is called before the first frame update
    //void Start()
    //{
    //    anim = gameObject.GetComponent<Animator>();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //public void NoWeapon()
    //{
    //    isSwordClick = false;
    //    swordButton.GetComponent<Image>().color = Color.white;
    //    swordWeapon.SetActive(false);
    //    anim.runtimeAnimatorController = noWeaponAnim;

    //    isAxeClick = false;
    //    axeButton.GetComponent<Image>().color = Color.white;
    //    axeWeapon.SetActive(false);

    //    isMaceClick = false;
    //    maceButton.GetComponent<Image>().color = Color.white;
    //    maceWeapon.SetActive(false);

    //    isShieldClick = false;
    //    shieldButton.GetComponent<Image>().color = Color.white;
    //    shieldWeapon.SetActive(false);
    //}

    //public void SwordClick()
    //{
    //    if (isSwordClick == false)
    //    {
    //        isSwordClick = true;
    //        swordButton.GetComponent<Image>().color = Color.gray;
    //        swordWeapon.SetActive(true);
    //        anim.runtimeAnimatorController = twoWeaponAnim;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);

    //    }
    //    else
    //    {
    //        NoWeapon();
    //        //isSwordClick = false;
    //        //swordButton.GetComponent<Image>().color = Color.white;
    //        //swordWeapon.SetActive(false);
    //        //anim.runtimeAnimatorController = noWeaponAnim;

    //        //isAxeClick = false;
    //        //axeButton.GetComponent<Image>().color = Color.white;
    //        //axeWeapon.SetActive(false);

    //        //isMaceClick = false;
    //        //maceButton.GetComponent<Image>().color = Color.white;
    //        //maceWeapon.SetActive(false);

    //        //isShieldClick = false;
    //        //shieldButton.GetComponent<Image>().color = Color.white;
    //        //shieldWeapon.SetActive(false);
    //    }
    //}

    //public void AxeClick()
    //{
    //    if (isAxeClick == false)
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = oneWeaponAnim;

    //        isAxeClick = true;
    //        axeButton.GetComponent<Image>().color = Color.gray;
    //        axeWeapon.SetActive(true);

    //        isMaceClick = false;
    //        maceButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);
    //    }
    //    else
    //    {
    //        NoWeapon();
    //        //isSwordClick = false;
    //        //swordButton.GetComponent<Image>().color = Color.white;
    //        //swordWeapon.SetActive(false);
    //        //anim.runtimeAnimatorController = noWeaponAnim;

    //        //isAxeClick = false;
    //        //axeButton.GetComponent<Image>().color = Color.white;
    //        //axeWeapon.SetActive(false);

    //        //isMaceClick = false;
    //        //maceButton.GetComponent<Image>().color = Color.white;
    //        //maceWeapon.SetActive(false);
    //    }
    //}

    //public void MaceClick()
    //{
    //    if (isMaceClick == false)
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = oneWeaponAnim;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = true;
    //        maceButton.GetComponent<Image>().color = Color.gray;
    //        maceWeapon.SetActive(true);
    //    }
    //    else
    //    {
    //        NoWeapon();
    //        //isSwordClick = false;
    //        //swordButton.GetComponent<Image>().color = Color.white;
    //        //swordWeapon.SetActive(false);
    //        //anim.runtimeAnimatorController = noWeaponAnim;

    //        //isAxeClick = false;
    //        //axeButton.GetComponent<Image>().color = Color.white;
    //        //axeWeapon.SetActive(false);

    //        //isMaceClick = false;
    //        //maceButton.GetComponent<Image>().color = Color.white;
    //        //maceWeapon.SetActive(false);
    //    }
    //}

    //public void ShieldClick()
    //{
    //    if (!isSwordClick)
    //        return;
    //    if (isShieldClick == false)
    //    {
    //        isShieldClick = true;
    //        shieldButton.GetComponent<Image>().color = Color.gray;
    //        shieldWeapon.SetActive(true);
    //        anim.runtimeAnimatorController = twoWeaponAnim;

    //    }
    //    else
    //    {
    //        isShieldClick = false;
    //        shieldButton.GetComponent<Image>().color = Color.white;
    //        shieldWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = twoWeaponAnim;
    //    }
    //}
}
