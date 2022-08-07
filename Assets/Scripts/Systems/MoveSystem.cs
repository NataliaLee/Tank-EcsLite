using Assets.Scripts.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class MoveSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsFilter _moveFilter;
        private EcsWorld _world;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            _moveFilter = _world.Filter<MovableComponent>().Inc<InputEventComponent>().End();
            var movables = _world.GetPool<MovableComponent>();
            var inputs = _world.GetPool<InputEventComponent>();
            foreach (var i in _moveFilter)
            {
                ref var movableComponent = ref movables.Get(i);
                ref var inputComponent = ref inputs.Get(i);
                movableComponent.transform.Rotate( 0,inputComponent.direction.x * Time.deltaTime * movableComponent.rotateSpeed,0);
                movableComponent.transform.position += movableComponent.transform.forward * inputComponent.direction.y * Time.deltaTime * movableComponent.moveSpeed;
            }
        }
    }
}
