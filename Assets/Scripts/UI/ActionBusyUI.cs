using UnitBased;
using UnityEngine;

namespace UI
{
    public class ActionBusyUI : MonoBehaviour
    {
        private void Start()
        {
            UnitActionSystem.Instance.OnBusyChanged += OnBusyChanged;
            Hide();
        }

        private void OnBusyChanged(object sender, bool isBusy)
        {
            if (isBusy) Show();
            else Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnBusyChanged -= OnBusyChanged;
        }
    }
}