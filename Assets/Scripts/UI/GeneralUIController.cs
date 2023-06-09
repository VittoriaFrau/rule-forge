using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class GeneralUIController: MonoBehaviour
    {
        private GameObject textGo;
        public List<GameObject> optionButtons;
        public List<GameObject> creationButtons;
        public List<GameObject> editingButtons;
        public List<GameObject> ruleButtons;
        public GameObject closeButton;
        public GameObject eventHandler;
        private EditModeController _editModeController;
        private ObjectsMenuController _objectsMenuController;
        private RuleCreationController _ruleCreationController;
        public RadialMenu radialMenu;
        
        
        public enum UIState
        {
            Default,
            EditMode,
            NewObject,
            NewRule
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
            _editModeController = eventHandler.GetComponent<EditModeController>();
            _objectsMenuController = eventHandler.GetComponent<ObjectsMenuController>();
            _ruleCreationController = eventHandler.GetComponent<RuleCreationController>();
            DefaultState();
            radialMenu.getListButtons(optionButtons);
            radialMenu.gameObject.SetActive(true);
        }

        public void Close()
        {
            var previousState = _uiState;
            DefaultState();
            ShowOptionsMenu();
            switch (previousState)
            {
                case UIState.EditMode:
                    _editModeController.DeActivateEditMode();
                    break;
                case UIState.NewObject:
                    _objectsMenuController.DeActivateObjectsMenu();
                    break;
                case UIState.NewRule:
                    _ruleCreationController.DeActivateNewRule();
                    break;
            }
            closeButton.SetActive(false);
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
            radialMenu.getListButtons(editingButtons);
        }
        
        public void NewRuleState()
        {
            _uiState = UIState.NewRule;
            text.text = "Please, grab the modality you want to use to create the rule";
            HideOptionsMenu();
            radialMenu.getListButtons(ruleButtons);
        }

        public void SelectedObject(string name)
        {
            text.text = "Selected object " + name;
        }

        public void SetDebugText(string text)
        {
            this.text.text = text;
        }
        
        
        public void NewObjectState()
        {
            _uiState = UIState.NewObject;
            text.text = "Please, select the object you want to create";
            HideOptionsMenu();
            radialMenu.getListButtons(creationButtons);
        }

        private void HideOptionsMenu()
        {
            foreach (var button in optionButtons)
            {
                button.SetActive(false);
            }
        }

        

        private void ShowOptionsMenu()
        {
            foreach (var button in optionButtons)
            {
                button.SetActive(true);
            }
        }

        //Funzioni nuove
        private void HideCreationMenu()
        {
            foreach (var button in creationButtons)
            {
                button.SetActive(false);
            }
        }

        private void HideRuleMenu()
        {
            foreach (var button in ruleButtons)
            {
                button.SetActive(false);
            }
        }

        private void HideEditMenu()
        {
            foreach (var button in editingButtons)
            {
                button.SetActive(false);
            }
        }

        public void DefaultRM()
        {
            HideOptionsMenu();
            HideCreationMenu();
            HideEditMenu();
            HideRuleMenu();
            radialMenu.getListButtons(optionButtons);
        }
    }
}