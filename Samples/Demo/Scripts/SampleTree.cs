using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class SampleTree : MonoBehaviour
    {
        [SerializeField] private GameObject full = default;
        [SerializeField] private GameObject empty = default;

        private float regenDuration = 10f;
        private float regenTimer = 0f;
        private bool isEmpty = false;

        public bool IsEmpty => isEmpty;

        private void Start()
        {
            Regenerate();
        }

        public void Chop()
        {
            full.SetActive(false);
            empty.SetActive(true);
            isEmpty = true;
        }

        private void Regenerate()
        {
            full.SetActive(true);
            empty.SetActive(false);
            isEmpty = false;
        }

        private void Update()
        {
            if (isEmpty)
            {
                regenTimer += Time.unscaledDeltaTime;
                if (regenTimer >= regenDuration)
                {
                    Regenerate();
                    regenTimer = 0;
                }
            }
        }
    }
}