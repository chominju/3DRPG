using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static QuestManager instance;

    string []questName;
    int[] questClearCount;
    int[] qusetCurrentCount;

    int index;

   public GameObject QuestNameText;
    public GameObject QuestCountText;

    void Start()
    {
        if (instance == null)
            instance = this;

        questName = new string[2];
        questClearCount = new int[2];
        qusetCurrentCount = new int[2];
        index = 0;
        initQuest();
        SetText();
    }

    static public QuestManager GetInstance()
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initQuest()
    {
        questName[0] = "몬스터 10마리 처치하기";
        questClearCount[0] = 10;
        qusetCurrentCount[0] = 0;


        questName[1] = "보스 처치하기";
        questClearCount[1] = 1;
        qusetCurrentCount[1] = 0;

    }

    void SetText()
    {
        QuestNameText.GetComponent<Text>().text = questName[index];
        QuestCountText.GetComponent<Text>().text = qusetCurrentCount[index] + " / " + questClearCount[index];

        if (IsQuestClear())
        {
            if (index <= 0)
                index++;
        }
    }

    public void AddCurrentCount(int count)
    {
        qusetCurrentCount[index] = count;
        SetText();
    }

    bool IsQuestClear()
    {
        if (qusetCurrentCount[index] >= questClearCount[index])
            return true;
        else
            return false;
    }
}
