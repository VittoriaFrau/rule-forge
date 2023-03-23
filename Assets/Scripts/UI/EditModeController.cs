using System;
using System.Collections.Generic;
using System.Linq;
using ECAPrototyping.RuleEngine;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EditModeController : MonoBehaviour
    {
        
        internal bool EditMode { get;set; }
        private List<GameObject> interactables;
        private GameObject canvas;
        public GameObject radialMenu;
        /*public GameObject textGo;
        private TextMeshPro text;
        public TextMeshPro Text
        {
            get => text;
            set { text = value; }
        }*/
        private GeneralUIController generalUIController;
        public List<GameObject> colors;
        public GameObject colorPalette;
        private GameObject selectedObject;
        public GameObject SelectedObject
        {
            get => selectedObject;
            set { selectedObject = value; }
        }

        private void Start()
        {
            EditMode = false;
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            if(radialMenu==null) radialMenu = canvas.transform.Find("RadialMenu").gameObject;
            /*text = textGo.GetComponent<TextMeshPro>();*/
            generalUIController = this.gameObject.GetComponent<GeneralUIController>();
            radialMenu.SetActive(false);
        }

        public void ActivateEditMode()
        {
            EditMode = true;
            UpdateInteractablesList();
            AddListenerToInteractables();
            generalUIController.EditModeState();
        }
        
        public void DeActivateEditMode()
        {
            EditMode = false;
            UpdateInteractablesList();
            RemoveListenerToInteractables();
            RemoveColorEventListener();
            /*text.text = "Please, select an object to continue";*/
            selectedObject = null;
            radialMenu.SetActive(false);
            colorPalette.SetActive(false);
        }
        
        private void AddListenerToInteractables()
        {
            foreach (var interactable in interactables)
            {
                ObjectManipulator _objectManipulator = interactable.GetComponent<ObjectManipulator>();
                _objectManipulator.OnClicked.AddListener(() => interactable.GetComponent<Prototypation>().ShowPieUIMenu());
            }
        }
        
        public void RemoveListenerToInteractables()
        {
            foreach (var interactable in interactables)
            {
                ObjectManipulator _objectManipulator = interactable.GetComponent<ObjectManipulator>();
                _objectManipulator.OnClicked.RemoveAllListeners();
            }
        }

        private void UpdateInteractablesList()
        {
            interactables = (from Transform child in GameObject.Find("Interactables").transform select child.gameObject).ToList();
        }

        public void ShowHideRadialMenu(bool visibility)
        {
            radialMenu.SetActive(visibility);
        }

        /*
         * Since the object is instantiated a runtime, we need to add the listener to each button of the color palette
         * to change the right selected object
         */
        public void ChangeColorEventListener()
        {
            if(selectedObject==null) return;
            foreach (var color in colors)
            {
                PressableButton button = color.GetComponent<PressableButton>();
                button.OnClicked.AddListener(() => selectedObject.GetComponent<ECAObject>().ChangeColor(color.name));
            }
        }
        
        public void RemoveColorEventListener()
        {
            if(selectedObject==null) return;
            foreach (var color in colors)
            {
                PressableButton button = color.GetComponent<PressableButton>();
                button.OnClicked.RemoveAllListeners();
            }
        }
       
        
    }
}