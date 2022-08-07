using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public PlayerView playerView;
    public TankData tankData;
    public WeaponData[] weapons;
    public EnemyData[] enemies;
    public HealthView hpView;
    [Tooltip("Time between enemy spawns in seconds.")]
    public float enemySpawnTime;

    public EnemyData GetRandomEnemy()
    {
        if (enemies.Length == 0)
            return null;
        var random = new System.Random();
        int rand = random.Next(0, enemies.Length);
        return enemies[rand];
    }
}
