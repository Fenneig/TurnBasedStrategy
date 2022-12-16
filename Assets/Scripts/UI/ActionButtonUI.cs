using Actions;
using TMPro;
using UnitBased;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _selectedImage;

        private BaseAction _baseAction;

        public void SetBaseAction(BaseAction baseAction)
        {
            _baseAction = baseAction;
            _textMeshPro.text = baseAction.GetActionName().ToUpper();
            _button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SelectedAction = baseAction;
            });
        }

        public void UpdateSelectedVisual()
        {
            BaseAction selectedBaseAction = UnitActionSystem.Instance.SelectedAction;
            _selectedImage.SetActive(selectedBaseAction == _baseAction);
        }
    }
}
