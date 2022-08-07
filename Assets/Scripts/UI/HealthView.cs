using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthView: MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _hpHolder;
        [SerializeField] private Image _hpProgress;

        public void Setup(Camera uiCamera)
        {
            _canvas.worldCamera = uiCamera;
            _hpHolder.gameObject.SetActive(false);
        }
        public void UpdateHp(float percent)
        {
            _hpProgress.fillAmount = percent;
            if (percent <= 0)
            {
                _hpHolder.gameObject.SetActive(false);
            }
            else
            {
                if (!_hpHolder.activeInHierarchy)
                {
                    _hpHolder.SetActive(true);
                }
            }
        }
    }
}
