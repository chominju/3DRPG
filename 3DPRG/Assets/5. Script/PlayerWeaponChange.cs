using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponChange : MonoBehaviour
{
    enum WeaponType
    {
        NoWeapon,           // 맨손
        Sword,              // 칼
        Axe,                // 도끼
        Mace,               // 망치
        Shield              // 방패
    }

    WeaponType currentWeapon;

    bool isSwordEqip = false;                  // 칼을 장착중인가
    bool isAxeEqip = false;                    // 도끼를 장착중인가
    bool isMaceEqip = false;                   // 망치를 장착중인가
    bool isShieldEqip = false;                 // 방패를 장착중인가

    public GameObject swordButton;             // 칼 버튼 
    public GameObject axeButton;               // 도끼 버튼
    public GameObject maceButton;              // 망치 버튼
    public GameObject shieldButton;            // 방패 버튼

    public GameObject swordWeapon;             // 칼 오브젝트     
    public GameObject axeWeapon;               // 도끼 오브젝트
    public GameObject maceWeapon;              // 망치 오브젝트
    public GameObject shieldWeapon;            // 방패 오브젝트

    public RuntimeAnimatorController noWeaponAnim;  // 맨손 애니메이터
    public RuntimeAnimatorController oneWeaponAnim; // 한손 애니메이터(칼, 도끼, 망치)
    public RuntimeAnimatorController twoWeaponAnim; // 두손 애니메이터(칼+방패)

    Animator anim;                                  // 플레이어 애니메이터 컴포넌트

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.Log("Player no Find");
        anim = player.GetComponent<Animator>();

        currentWeapon = WeaponType.NoWeapon;
        // 버튼에 클릭 이벤트 리스너 추가
        swordButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Sword));
        axeButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Axe));
        maceButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Mace));
        shieldButton.GetComponentInChildren<Button>().onClick.AddListener(() => WeaponButtonClick(WeaponType.Shield));
    }

    void WeaponButtonClick(WeaponType clickType)
    {
        // 클릭한게 쉴드라면
        if (clickType == WeaponType.Shield)
        {
            // 칼을 장착했는가?
            if (isSwordEqip)
                isShieldEqip = !isShieldEqip;
            else
                isShieldEqip = false;
        }
        else
        {
            // 모든 무기 클릭 상태 초기화
            isSwordEqip = false;
            isAxeEqip = false;
            isMaceEqip = false;

            // 기존에 누른 무기가 같을 때
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
                // 기존에 누른 무기와 상태가 다르다면 장착
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

        // 상태에 따른 무기 활성화/비활성화
        SetActiveWeapon();
        SetAnimWeapon();
        SetButtonColor();
    }

    void SetActiveWeapon()
    {
        // 무기의 활성화 상태를 업데이트
        swordWeapon.SetActive(isSwordEqip);
        axeWeapon.SetActive(isAxeEqip);
        maceWeapon.SetActive(isMaceEqip);
        shieldWeapon.SetActive(isShieldEqip);
    }

    void SetAnimWeapon()
    {
        // 무기에 따른 애니메이터 변경
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





    // 이전버전

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
