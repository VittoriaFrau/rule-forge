using TMPro;
using UnityEngine;

namespace UI
{
    public class GeneralUIController: MonoBehaviour
    {
        public GameObject textGo;
        public enum UIState
        {
            Default,
            EditMode,
            NewObject,
        }
        private UIState _uiState;
        public UIState State
        {
            get => _uiState;
            set { _uiState = value; }
        }
        private TextMeshPro text;
        public TextMeshPro Text
        {
            get => text;
            set { text = value; }
        }
        
        private void Start()
        {
            _uiState = UIState.Default;
            text = textGo.GetComponent<TextMeshPro>();
        }
        
        
    }
}