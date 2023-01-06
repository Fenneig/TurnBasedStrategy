using UnityEngine;

namespace Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _selectedGO;

        public void Show(Material material)
        {
            _meshRenderer.material = material;
            _meshRenderer.enabled = true;
        }

        public void Hide() => _meshRenderer.enabled = false;

        public void ShowSelected() => _selectedGO.SetActive(true);
        public void HideSelected() => _selectedGO.SetActive(false);
        
    }
}