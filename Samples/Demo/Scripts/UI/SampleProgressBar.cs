using Elysium.AI.GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.AI.Samples.GOAP
{
    public class SampleProgressBar : MonoBehaviour, IProgressBar
    {
        [SerializeField] private Image progress = default;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void SetFillAmount(float _fill)
        {
            progress.fillAmount = _fill;
        }

        public void SetActive(bool _active)
        {
            gameObject.SetActive(_active);
        }
    }
}