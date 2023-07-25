using System;
using System.Collections.Generic;
using ECAPrototyping.RuleEngine;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Action = ECAPrototyping.RuleEngine.Action;

namespace UI
{
    public class Prototypation: MonoBehaviour
    {
        private EditModeController _editModeController;
        private ObjectManipulator _objectManipulator;
        private GeneralUIController _generalUIController;
        private GameObject eventHandler;
        private RuleEngine _ruleEngine;
        bool action_executed;

        private void Start()
        {
            eventHandler = GameObject.Find("EventHandler");
            _editModeController = eventHandler.GetComponent<EditModeController>();
            _objectManipulator = GetComponent<ObjectManipulator>();
            _generalUIController = eventHandler.GetComponent<GeneralUIController>();
            _ruleEngine = RuleEngine.GetInstance();
            //CheckActionsAttribute(GameObject.Find("animal_people_wolf_1"));
            
            
            /*if (_editModeController.EditMode)
            {
                //Show the PiMenu when the object is selected
                UnityAction<SelectEnterEventArgs> firstSelectEntered = null;
                firstSelectEntered = args =>
                {
                    _editModeController.ShowHidePiUIMenu(true);
                    _objectManipulator.firstSelectEntered.RemoveListener(firstSelectEntered);
                };
                _objectManipulator.firstSelectEntered.AddListener(firstSelectEntered);
            }*/
        }
        
        public void ShowPieUIMenu()
        {
            if (_editModeController.EditMode)
            {
                _editModeController.ShowHideRadialMenu(true);
                _editModeController.SelectedObject = gameObject;
                _generalUIController.SelectedObject(gameObject.name);
                CheckActionsAttribute(_editModeController.SelectedObject);
                _generalUIController.radialMenu.getListButtons(CheckECAObject(_editModeController.SelectedObject));
            }
        }
        
        //Function to check all the Action attributes of the selected object 
        public void CheckActionsAttribute(GameObject gameObject)
        {
            List<ActionAttribute> list = new List<ActionAttribute>();
            if (gameObject.GetComponents<ECAObject>() != null)
            {
                Component [] components = gameObject.GetComponents(typeof(MonoBehaviour));
                foreach (Component component in components)
                {
                    list.AddRange(_ruleEngine.ListActionsAttribute(component));
                }
            }
        }

        //Function to check if the object is ECA Food/Character/Environment (only for the fist time I select the object)
        public List<GameObject> CheckECAObject(GameObject gameObject)
        {
            while (!action_executed)
            {
                if (gameObject.GetComponent<ECACharacter>() != null)
                {
                    _generalUIController.editingButtonsTMP.AddRange(_generalUIController.characterButtons);
                    
                }   
                action_executed = true;
            }
            return _generalUIController.editingButtonsTMP;
        }

        public void HidePieUIMenu()
        {
            if(!_editModeController.EditMode) _editModeController.ShowHideRadialMenu(false);
        }
    }
}