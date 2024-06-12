using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    [SerializeField]
    private GameObject enemyPrefab;

    private IObjectPool<Enemy> enemyPool;

    // 이미 풀에 있는 항목을 해제할려고 할때 오류발생을 시키는지
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        enemyPool = new ObjectPool<Enemy>(CreatePooledEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
    }


    static public EnemyManager GetInstance()
    {
        return instance;
    }

    public IObjectPool<Enemy> GetEnemyPool()
    {
        return enemyPool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private Enemy CreatePooledEnemy()
    {
        Debug.Log("Log : CreatePooledEnemy");
        Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        return enemy;
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(Enemy enemy)
    {
        Debug.Log("Log : OnReturnedToPool");
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
