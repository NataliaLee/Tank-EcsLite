using Assets.Scripts.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

public sealed class BattleStarter : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private BattleField _battlefield;
    [SerializeField] private Camera _uiCamera;
    private EcsWorld _world;
    private EcsSystems _systems;
    private SharedData _sharedData;


    void Start()
    {
        _world = new EcsWorld();
        _sharedData = new SharedData()
        {
            gameData = _gameData
        };
        _systems = new EcsSystems(_world, _sharedData);
        AddSystems();
        _systems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Init();
    }

    private void AddSystems()
    {
        _systems.Add(new BattleInitSystem(_battlefield))
            .Add(new EnemySpawnSystem(_battlefield, _uiCamera))
            .Add(new PlayerInputSystem())
            .Add(new MoveSystem())
            .Add(new ChangeWeaponSystem())
            .DelHere<ChangeWeaponEvent>()
            .DelHere<SpawnEnemyEvent>();
    }

    void Update()
    {
        _systems.Run();
    }

    private void OnDestroy()
    {
        _systems.Destroy();
        _world.Destroy();
    }
}
