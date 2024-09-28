using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance = null;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Vector3[] enemyStartTransform;
    [SerializeField]
    private GameObject boss;

    private IObjectPool<GameObject> enemyPool;

    // �̹� Ǯ�� �ִ� �׸��� �����ҷ��� �Ҷ� �����߻��� ��Ű����
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    int enemyKillCount = 0;         // ���� óġ �� ��


    int index;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        // ������Ʈ Ǯ�� �ʱ�ȭ
        enemyPool = new ObjectPool<GameObject>(CreatePooledEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

        for (index =0; index < maxPoolSize; index++)
        {
            GameObject enemy = CreatePooledEnemy();
            enemyPool.Release(enemy);
            enemy.GetComponent<Transform>().position = enemyStartTransform[index];
            enemy.name = "Enemy" + index;
            enemyPool.Get();
        }
    }


    public static  EnemyManager GetInstance()
    {
        return instance;
    }

    public IObjectPool<GameObject> GetEnemyPool()
    {
        return enemyPool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private GameObject CreatePooledEnemy()
    {
        Debug.Log("Log : CreatePooledEnemy");
        GameObject enemy = Instantiate(enemyPrefab);
        return enemy;
    }

    private void OnTakeFromPool(GameObject enemy)
    {
        enemy.SetActive(true);
    }

    private void OnReturnedToPool(GameObject enemy)
    {
        Debug.Log("Log : OnReturnedToPool");
        enemy.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject enemy)
    {
        Destroy(enemy);
    }

    public int GetEnemyKillCount()
    {
        return enemyKillCount;
    }

    public void AddEnmeyKillCount()
    {
        enemyKillCount++;
        QuestManager.GetInstance().AddCurrentCount(enemyKillCount);
    }

    public void AppearBoss()
    {
        //GameObject boss =  Instantiate(bossPrefab);
        // boss.transform.position = new Vector3(-17.0f, 0.45f, -0.2f);
        boss.SetActive(true);
        GameObject character =  GameObject.Find("Character");
        character.transform.position = new Vector3(-17.0f, 0.45f, 7.73f);

        boss.transform.LookAt(character.transform.position);
        character.transform.LookAt(boss.transform.position);

        GameObject cameraArm = GameObject.Find("CameraArm");
        cameraArm.transform.LookAt(character.transform.position);


    }

}
