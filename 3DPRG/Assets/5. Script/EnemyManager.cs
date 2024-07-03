using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Vector3[] enemyStartTransform; 

    private IObjectPool<GameObject> enemyPool;

    // 이미 풀에 있는 항목을 해제할려고 할때 오류발생을 시키는지
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        // 오브젝트 풀링 초기화
        enemyPool = new ObjectPool<GameObject>(CreatePooledEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

        for (index =0; index < maxPoolSize; index++)
        {
            GameObject enemy = CreatePooledEnemy();
            enemyPool.Release(enemy);
            enemy.GetComponent<Transform>().position = enemyStartTransform[index];
            enemyPool.Get();
        }
    }


    static public EnemyManager GetInstance()
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
}
