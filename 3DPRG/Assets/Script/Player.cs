using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    bool isSwordClick = false;
    public GameObject sword;
    public GameObject weapon;

    public RuntimeAnimatorController  noW;
    public RuntimeAnimatorController twoW;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwordClick()
    {
        if(isSwordClick==false)
        {
            isSwordClick = true;
            sword.GetComponent<Image>().color = Color.gray;
            weapon.SetActive(true);

            anim.runtimeAnimatorController = twoW;

        }
        else
        {
            sword.GetComponent<Image>().color = Color.white;
            anim.runtimeAnimatorController = noW;
            weapon.SetActive(false);
        }
    }
}
