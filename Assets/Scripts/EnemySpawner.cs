using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private Tilemap levelFloor;
    [SerializeField] private GameObject enemyPrefab;
    [Space]
    [SerializeField] private int targetEnemies = 50;
    private int currentEnemies;
    private List<Vector3> spawnPoints = new();


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (enemyPrefab == null)  Debug.LogError("No enemy prefab selected");
        SetUpSpwnPoints();
        SpawnInitial();
    }

    private void SetUpSpwnPoints()
    {
        foreach (Vector3Int position in levelFloor.cellBounds.allPositionsWithin)
        {
            if (levelFloor.HasTile(position))
            {
                spawnPoints.Add(new Vector3(position.x + 0.5f, position.y + 0.5f));
            }
        }
    }

    private void SpawnInitial()
    {
        for (int i = 0; i < targetEnemies; i++)
        {
            SpawnEnemyAtRandomPos();
        }
    }


    public GameObject SpawnEnemyAtRandomPos()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)];


        return enemy;
    }
}
