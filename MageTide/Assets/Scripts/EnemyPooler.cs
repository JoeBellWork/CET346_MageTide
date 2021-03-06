using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolEnemies;
    public GameObject bigSplash, smallDrops, bigDrops;
    private ParticleSystem bigSplashP, smallDropsP, bigDropP;
    private int i;

    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public Transform spawnLocation;
    public int AIMaxLimit, AICurrentLimit;
    public List<pool> pools;
    

    // Start is called before the first frame update
    void Start()
    {
        bigSplashP = bigSplash.GetComponent<ParticleSystem>();
        bigDropP = bigDrops.GetComponent<ParticleSystem>();
        smallDropsP = smallDrops.GetComponent<ParticleSystem>();

        poolEnemies = new Dictionary<string, Queue<GameObject>>();

        foreach (pool pool in pools)
        {
            Queue<GameObject> enemyPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                enemyPool.Enqueue(obj);
            }
            poolEnemies.Add(pool.tag, enemyPool);
        }

        AICurrentLimit = 0;
        StartCoroutine(spawnCounter());
    }     

    IEnumerator spawnCounter()
    {
        if (AICurrentLimit < AIMaxLimit)
        {
            RandomBinary();
            if(i == 0)
            {
                spawnFromPool("WG");
            }
            else
            {
                spawnFromPool("MP");
            }
            AICurrentLimit++;
        }
        yield return new WaitForSeconds(30f);
        StartCoroutine(spawnCounter());        
    }

    public int RandomBinary()
    {
        i = Random.Range(0, 2);
        Debug.Log(i);
        return i;
    }

    public GameObject spawnFromPool(string tag)
    {
        if(poolEnemies.ContainsKey(tag))
        {
            GameObject objToSpawn = poolEnemies[tag].Dequeue();
            objToSpawn.transform.position = spawnLocation.position;
            objToSpawn.SetActive(true);
            
            poolEnemies[tag].Enqueue(objToSpawn);
            bigSplashP.Play();
            bigDropP.Play();
            smallDropsP.Play();

            return objToSpawn;
        }
        else
        {
            Debug.LogError(tag + " does not exist");
            return null;
        }        
    }
}
