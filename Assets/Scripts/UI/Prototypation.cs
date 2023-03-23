using System;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI
{
    public class Prototypation: MonoBehaviour
    {
        private EditModeController _editModeController;
        private ObjectManipulator _objectManipulator;
        
        private void Start()
        {
            _editModeController = GameObject.Find("EventHandler").GetComponent<EditModeController>();
            _objectManipulator = GetComponent<ObjectManipulator>();
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
                _editModeController.Text.text = "Selected object "+ gameObject.name;
            }
            
        }
        
        public void HidePieUIMenu()
        {
            if(!_editModeController.EditMode) _editModeController.ShowHideRadialMenu(false);
        }
    }
}