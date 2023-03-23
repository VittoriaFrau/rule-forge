using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GeneralUIController: MonoBehaviour
    {
        private GameObject textGo;
        public List<GameObject> optionButtons;
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
        private TextMeshProUGUI text;
        public TextMeshProUGUI Text
        {
            get => text;
            set { text = value; }
        }
        
        private void Start()
        {
            _uiState = UIState.Default;
            textGo = GameObject.FindGameObjectWithTag("debugText");
            text = textGo.GetComponent<TextMeshProUGUI>();
            DefaultState();
        }

        public void DefaultState()
        {
            text.text = "Choose if you want to create an object, modify an existing one or create a rule";
        }
        
        public void EditModeState()
        {
            _uiState = UIState.EditMode;
            text.text = "Please, select an object to continue";
            HideOptionsMenu();
        }

        public void SelectedObject(string name)
        {
            text.text = "Selected object " + name;
        }
        
        
        public void NewObjectState()
        {
            _uiState = UIState.NewObject;
            text.text = "Please, select the object you want to create";
            HideOptionsMenu();
        }

        private void HideOptionsMenu()
        {
            foreach (var button in optionButtons)
            {
                button.SetActive(false);
            }
        }
    }
}