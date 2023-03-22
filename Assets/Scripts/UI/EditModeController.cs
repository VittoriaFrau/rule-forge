using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EditModeController : MonoBehaviour
    {
        
        public TextMeshProUGUI editModeButtonLabel;
        internal bool EditMode { get;set; }
        private List<GameObject> interactables;
        private GameObject canvas;
        public GameObject radialMenu;

        private void Start()
        {
            EditMode = false;
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            if(radialMenu==null) radialMenu = canvas.transform.Find("RadialMenu").gameObject;
            radialMenu.SetActive(false);

        }

        public void ChangeEditModeButton()
        {
            editModeButtonLabel.text = EditMode ? "Exit Edit Mode" : "Edit Mode";
            
        }

        public void ActivateEditMode()
        {
            EditMode = !EditMode;
            //ChangeEditModeButton();
            UpdateInteractablesList();
            if(!EditMode) radialMenu.SetActive(false);
            /*if (EditMode) {
                //When the edit mode is on, all the objects inside the Interactables gameobject will have the Prototupation component
                foreach (var interactable in interactables)
                {
                    interactable.AddComponent<Prototypation>();
                    
                }
            }
            else {
                //When the edit mode is off, all the objects inside the Interactables gameobject will have the Prototupation component
                foreach (var interactable in interactables)
                {
                    Destroy(interactable.GetComponent<Prototypation>());
                }
            }*/
            
        }

        private void UpdateInteractablesList()
        {
            interactables = (from Transform child in GameObject.Find("Interactables").transform select child.gameObject).ToList();
        }

        public void ShowHideRadialMenu(bool visibility)
        {
            radialMenu.SetActive(visibility);
        }
    }
}