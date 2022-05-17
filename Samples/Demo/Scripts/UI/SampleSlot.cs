using UnityEngine;
using UnityEngine.UI;

namespace Elysium.AI.Samples.GOAP
{
    public class SampleSlot : MonoBehaviour
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private GameObject broken = default;

        private void Start()
        {
            broken.SetActive(false);
            icon.gameObject.SetActive(false);
        }

        public void PickUpAxe()
        {
            icon.gameObject.SetActive(true);
        }

        public void BreakAxe()
        {
            broken.SetActive(true);
        }

        public void RepairAxe()
        {
            broken.SetActive(false);
        }
    }
}