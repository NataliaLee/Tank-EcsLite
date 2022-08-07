using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform _weaponHolder;

    public Transform WeaponHolder => _weaponHolder;
}
