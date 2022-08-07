using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Views;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

public class BattleInitSystem : IEcsInitSystem
{
    private EcsWorld _world;
    private GameData _gameData;
    private BattleField _battleField;

    public BattleInitSystem(BattleField battleField)
    {
        _battleField = battleField;
    }

    public void Init(IEcsSystems systems)
    {
        _gameData = systems.GetShared<SharedData>().gameData;
        _world = systems.GetWorld();
        AddPlayer();
    }

    private void AddPlayer()
    {
        var player = _world.NewEntity();
        var view = GameObject.Instantiate<PlayerView>(_gameData.playerView, _battleField.Field);
        _world.GetPool<PlayerComponent>().Add(player);
        _world.GetPool<InputEventComponent>().Add(player);
        ref var movable = ref _world.GetPool<MovableComponent>().Add(player);
        movable.transform = view.transform;
        movable.moveSpeed = _gameData.tankData.speed;
        movable.rotateSpeed = _gameData.tankData.rotationSpeed;
        view.AddComponent<EntityReference>().packedEntity = _world.PackEntity(player);

        var firstWeapon = _gameData.weapons[0];
        var weaponView= GameObject.Instantiate(firstWeapon.view, view.WeaponHolder);
        ref var weapon = ref _world.GetPool<WeaponComponent>().Add(player);
        weapon.id = firstWeapon.id;
        weapon.damage = firstWeapon.damage;
        weapon.speed = firstWeapon.speed;
        weapon.view = weaponView;
    }
}
