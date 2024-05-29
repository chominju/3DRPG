using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    //bool isSwordClick = false;
    //bool isAxeClick = false;
    //bool isMaceClick = false;
    //bool isShieldClick = false;

    //public GameObject swordButton;
    //public GameObject axeButton;
    //public GameObject maceButton;
    //public GameObject shieldButton;

    //public GameObject swordWeapon;
    //public GameObject axeWeapon;
    //public GameObject maceWeapon;
    //public GameObject shieldWeapon;

    //public RuntimeAnimatorController  noW;
    //public RuntimeAnimatorController oneW;
    //public RuntimeAnimatorController twoW;

    //Animator anim;


    //bool enemyTrigger;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    anim = gameObject.GetComponent<Animator>();
    //    enemyTrigger = false;
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public void SwordClick()
    //{
    //    if(isSwordClick==false)
    //    {
    //        isSwordClick = true;
    //        swordButton.GetComponent<Image>().color = Color.gray;
    //        swordWeapon.SetActive(true);
    //        anim.runtimeAnimatorController = twoW;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);

    //    }
    //    else
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = noW;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = false;
    //        maceButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);

    //        isShieldClick = false;
    //        shieldButton.GetComponent<Image>().color = Color.white;
    //        shieldWeapon.SetActive(false);
    //    }
    //}

    //public void AxeClick()
    //{
    //    if (isAxeClick == false)
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = oneW;

    //        isAxeClick = true;
    //        axeButton.GetComponent<Image>().color = Color.gray;
    //        axeWeapon.SetActive(true);

    //        isMaceClick = false;
    //        maceButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);
    //    }
    //    else
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = noW;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = false;
    //        maceButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);
    //    }
    //}

    //public void MaceClick()
    //{
    //    if (isMaceClick == false)
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = oneW;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = true;
    //        maceButton.GetComponent<Image>().color = Color.gray;
    //        maceWeapon.SetActive(true);
    //    }
    //    else
    //    {
    //        isSwordClick = false;
    //        swordButton.GetComponent<Image>().color = Color.white;
    //        swordWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = noW;

    //        isAxeClick = false;
    //        axeButton.GetComponent<Image>().color = Color.white;
    //        axeWeapon.SetActive(false);

    //        isMaceClick = false;
    //        maceButton.GetComponent<Image>().color = Color.white;
    //        maceWeapon.SetActive(false);
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
    //        anim.runtimeAnimatorController = twoW;

    //    }
    //    else
    //    {
    //        isShieldClick = false;
    //        shieldButton.GetComponent<Image>().color = Color.white;
    //        shieldWeapon.SetActive(false);
    //        anim.runtimeAnimatorController = twoW;
    //    }
    //}

    //public void WeaponSwitch()
    //{

    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //        enemyTrigger = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //        enemyTrigger = false;
    //}

    //public bool Tig()
    //{
    //    return enemyTrigger;
    //}
}
