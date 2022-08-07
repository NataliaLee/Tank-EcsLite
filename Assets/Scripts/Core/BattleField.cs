using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Core
{
    public class BattleField: MonoBehaviour
    {
        [SerializeField] private Transform _field;
        [SerializeField] private Transform[] _enemySpawnPoints;

        public Transform Field => _field;

        public Vector3 GetSpawnPoint()
        {
            if(_enemySpawnPoints.Length==0)
                return Vector3.zero;
            Random random = new Random();
            int rand = random.Next(0, _enemySpawnPoints.Length);
            return _enemySpawnPoints[rand].position;
        }

    }
}
