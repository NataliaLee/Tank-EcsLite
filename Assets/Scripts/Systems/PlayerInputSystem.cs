using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Components;
using Assets.Scripts.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsFilter _inputEventsFilter;
        EcsFilter _canChangeWeaponEventsFilter;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            _inputEventsFilter = _world.Filter<InputEventComponent>().End();
            foreach (var i in _inputEventsFilter)
            {
                ref var inputEvent = ref _world.GetPool<InputEventComponent>().Get(i);
                inputEvent.direction = new Vector2(x,y);
            }
            _canChangeWeaponEventsFilter = _world.Filter<PlayerComponent>().Inc<WeaponComponent>().End();

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (var i in _canChangeWeaponEventsFilter)
                {
                    ref var changeEvent = ref _world.GetPool<ChangeWeaponEvent>().Add(i);
                    changeEvent.next = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach (var i in _canChangeWeaponEventsFilter)
                {
                    ref var changeEvent = ref _world.GetPool<ChangeWeaponEvent>().Add(i);
                    changeEvent.next = false;
                }
            }
        }
    }
}
