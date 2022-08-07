using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Views;

namespace Assets.Scripts.Components
{
    public struct WeaponComponent
    {
        public string id;
        public float damage;
        public float speed;
        public WeaponView view;
    }
}
