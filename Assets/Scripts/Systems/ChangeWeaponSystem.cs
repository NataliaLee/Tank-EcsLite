using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Data;
using Assets.Scripts.Events;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class ChangeWeaponSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _changeFilter;
        private EcsPool<WeaponComponent> _weapons;
        private EcsPool<ChangeWeaponEvent> _changeEvents;
        private GameData _gameData;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _gameData = systems.GetShared<SharedData>().gameData;
            _changeFilter = _world.Filter<ChangeWeaponEvent>().Inc<WeaponComponent>().End();
            _weapons = _world.GetPool<WeaponComponent>();
            _changeEvents = _world.GetPool<ChangeWeaponEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _changeFilter)
            {
                ref var currentWeapon = ref _weapons.Get(i);
                var newData = GetNewWeapon(currentWeapon.id, _changeEvents.Get(i).next);
                var newView = GameObject.Instantiate(newData.view, currentWeapon.view.transform.parent);
                GameObject.Destroy(currentWeapon.view.gameObject);
                currentWeapon.view = newView;
                currentWeapon.id = newData.id;
                currentWeapon.damage = newData.damage;
                currentWeapon.speed = newData.speed;

            }
        }

        private WeaponData GetNewWeapon(string currentId, bool next)
        {
            var weapons = _gameData.weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (currentId.Equals(weapons[i].id))
                {
                    if (next)
                    {
                        if (i + 1 < weapons.Length)
                        {
                            return _gameData.weapons[i + 1];
                        }
                        else
                        {
                            return _gameData.weapons[0];
                        }
                    }
                    else
                    {
                        if (i - 1 < 0)
                        {
                            return _gameData.weapons[weapons.Length - 1];
                        }
                        else
                        {
                            return _gameData.weapons[i-1];
                        }
                    }
                }
            }

            return _gameData.weapons[0];
        }
    }
}
