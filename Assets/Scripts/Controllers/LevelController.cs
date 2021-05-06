using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject[] allies;
    [SerializeField] private GameObject[] enemies;

    [SerializeField] private int allyRewardAmount;
    [SerializeField] private float allyRewardRate;

    [SerializeField] private int enemyRewardAmount;
    [SerializeField] private float enemyRewardRate;

    private Timer allyRewardTimer;
    private Timer enemyRewardTimer;

    private GameObject enemyToSpawn;
    private int enemyCostToSpawn;

    void OnEnable()
    {
        Random.InitState(System.DateTime.UtcNow.Second);
        Random.InitState(Random.Range(System.Int32.MinValue, System.Int32.MaxValue));

        TowerEvents.OnTowerDestroyed += OnTowerDestroyed;
        LevelEvents.OnSpawnAlly += OnSpawnAlly;

        LevelStore.alliesResources = 0;
        LevelStore.enemiesResources = 0;

        allyRewardTimer = new Timer(allyRewardRate);
        enemyRewardTimer = new Timer(enemyRewardRate);

        SelectRandomMinion();
        LevelEvents.EmitBegin();
    }

    void OnDisable()
    {
        TowerEvents.OnTowerDestroyed -= OnTowerDestroyed;
        LevelEvents.OnSpawnAlly -= OnSpawnAlly;
    }

    void FixedUpdate()
    {
        allyRewardTimer.Update(Time.deltaTime);
        enemyRewardTimer.Update(Time.deltaTime);

        if (enemyRewardTimer.isFinished())
        {
            LevelStore.enemiesResources += enemyRewardAmount;
            enemyRewardTimer.Set(allyRewardRate);
        }

        if (allyRewardTimer.isFinished())
        {
            LevelStore.alliesResources += allyRewardAmount;
            allyRewardTimer.Set(enemyRewardRate);
        }

        for (int i = 0; i < allies.Length; i++)
        {
            var ally = allies[i];
            HUDEvents.EmitUnitButtonStatusUpdate(i, LevelStore.alliesResources >= ally.GetComponent<MinionAllyController>().GetCost());
        }

        if (LevelStore.enemiesResources >= enemyCostToSpawn)
        {
            LevelStore.enemiesResources -= enemyCostToSpawn;
            var position = GenerateRandomPosition(enemyToSpawn);
            Instantiate(enemyToSpawn, position, Quaternion.identity);
            SelectRandomMinion();
        }
    }

    private void OnTowerDestroyed(in TowerController tower)
    {
        if (tower.GetParty().Equals(Party.Ally))
        {
            LevelEvents.EmitDefeat();
        }
        else
        {
            LevelEvents.EmitWon();
        }
    }

    private float MapLane(Lane lane)
    {
        switch (lane)
        {
            case Lane.Top: return -2;
            case Lane.Middle: return -8;
            default: return -14;
        }
    }

    private float MapLane(int lane)
    {
        switch (lane)
        {
            case 0: return -2;
            case 1: return -8;
            default: return -14;
        }
    }

    private void OnSpawnAlly(int unitIndex, Lane lane)
    {
        var gameObject = allies[unitIndex];
        var cost = gameObject.GetComponent<MinionController>().GetCost();

        if (LevelStore.alliesResources >= cost)
        {
            LevelStore.alliesResources -= cost;
            var position = new Vector3(12, -gameObject.GetComponent<BoxCollider>().bounds.extents.y, MapLane(lane));
            Instantiate(gameObject, position, Quaternion.identity);
        }
    }

    private void SelectRandomMinion()
    {
        enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
        enemyCostToSpawn = enemyToSpawn.GetComponent<MinionController>().GetCost();
    }

    private Vector3 GenerateRandomPosition(GameObject minion)
    {
        var collider = minion.GetComponent<BoxCollider>().bounds.extents.y;
        return new Vector3(-12, collider, MapLane(Random.Range(0, 4)));
    }
}
