using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public Vector2 spawnRange;
    
    private int m_EnemyCounter;
    private int m_SpawnCounter =1;
    public GameObject startScene;
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    

    private void Awake()
    {
        enabled = false;
    }
    
    private void Update()
    {
        m_EnemyCounter = FindObjectsOfType<EnemyController>().Length;
        if (m_EnemyCounter== 0)
        {
            int i = 0;
            while (i <= m_SpawnCounter)
            {
                SpawnEnemy();
                i++;
            }
            SpawnPowerUp();
            m_SpawnCounter ++;
        }
    }

    private void SpawnEnemy()
    {
        SpawnObject(enemyPrefab);
    }

    private void SpawnPowerUp()
    {
        SpawnObject(powerUpPrefab);
    }

    private void SpawnObject(GameObject entity)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRange[0], spawnRange[1]),
            entity.transform.position.y,
            Random.Range(spawnRange[0], spawnRange[1])
            );
        Instantiate(entity, spawnPosition, entity.transform.rotation);
    }


    public void StartGame()
    {
        enabled = true;
        startScene.SetActive(false);
        SpawnEnemy();
    }
}
