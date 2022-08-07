using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Views;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class EnemyData
    {
        public string id;
        public EnemyView view;
        public float speed;
        public float rotateSpeed;
        public float health;
    }
}
