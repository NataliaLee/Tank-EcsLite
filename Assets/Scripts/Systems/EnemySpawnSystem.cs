using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Events;
using Assets.Scripts.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class EnemySpawnSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _spawnEventFilter;
        private EcsFilter _spawnerFilter;
        private BattleField _battleField;
        private Camera _uiCamera;
        private GameData _gameData;
        private EcsPool<SpawnEnemyEvent> _spawnEvents;
        private EcsPool<EnemySpawnerComponent> _spawners;

        public EnemySpawnSystem(BattleField battleField, Camera uiCamera)
        {
            _battleField = battleField;
            _uiCamera = uiCamera;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _gameData = systems.GetShared<SharedData>().gameData;
            _spawnEventFilter = _world.Filter<SpawnEnemyEvent>().End();
            _spawnerFilter = _world.Filter<EnemySpawnerComponent>().End();
            _spawnEvents = _world.GetPool<SpawnEnemyEvent>();
            _spawners = _world.GetPool<EnemySpawnerComponent>();
            AddSpawner();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _spawnerFilter)
            {
                ref var spawner = ref _spawners.Get(i);
                if (Time.time - spawner.lastTimeSpawned > _gameData.enemySpawnTime)
                {
                    ref var spawnEvent = ref _spawnEvents.Add(i);
                    spawnEvent.enemyId = _gameData.GetRandomEnemy().id;
                    spawner.lastTimeSpawned = Time.time;
                }
            }

            foreach (var i in _spawnEventFilter)
            {
                var spawnEvent = _spawnEvents.Get(i);
                var enemyData = _gameData.enemies.FirstOrDefault(_ => _.id.Equals(spawnEvent.enemyId));
                if(enemyData==null)
                    continue;
                var enemyView = GameObject.Instantiate<EnemyView>(
                    enemyData.view,
                    _battleField.GetSpawnPoint(),
                    Quaternion.identity,
                    _battleField.Field);
                var enemy = _world.NewEntity();
                ref var health=ref _world.GetPool<HealthComponent>().Add(enemy);
                health.Max = health.Current = enemyData.health;

                ref var movable = ref _world.GetPool<MovableComponent>().Add(enemy);
                movable.moveSpeed = enemyData.speed;
                movable.rotateSpeed = enemyData.rotateSpeed;
                movable.transform = enemyView.transform;

                _world.GetPool<EnemyComponent>().Add(enemy);


                var hpView = GameObject.Instantiate(_gameData.hpView, enemyView.transform);
                hpView.Setup(_uiCamera);
                ref var hpViewComponent = ref _world.GetPool<HealthViewComponent>().Add(enemy);
                hpViewComponent.hpView = hpView;
            }
        }

        private void AddSpawner()
        {
            var spawner=_world.NewEntity();
            ref var spawnerComponet=ref _world.GetPool<EnemySpawnerComponent>().Add(spawner);
            spawnerComponet.lastTimeSpawned = Time.time;
            ref var spawnEvent= ref _spawnEvents.Add(spawner);
            spawnEvent.enemyId = _gameData.GetRandomEnemy().id;
        }
    }
}
