using Actions;
using TMPro;
using UnitBased;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;

    public void SetBaseAction(BaseAction baseAction)
    {
        _textMeshPro.text = baseAction.GetActionName().ToUpper();
        _button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SelectedAction = baseAction;
        });
    }
    
}
