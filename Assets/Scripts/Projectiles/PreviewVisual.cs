using System;
using Actions;
using UnitBased;
using UnityEngine;

namespace Projectiles
{
    public class PreviewVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPreview;

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
            BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
            HideAllPreviews();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) => UpdatePreview();
        private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) => HideAllPreviews();
        private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) => UpdatePreview();

        private void UpdatePreview()
        {
            HideAllPreviews();

            BaseAction selectedAction = UnitActionSystem.Instance.SelectedAction;

            switch (selectedAction)
            {
                case GrenadeAction:
                    _explosionPreview.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void HideAllPreviews()
        {
            _explosionPreview.SetActive(false);
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
            BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
        }
    }
}