using System;
using System.Collections.Generic;
using System.Linq;
using ECAPrototyping.RuleEngine;
using ECAPrototyping.Utils;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using Action = ECAPrototyping.RuleEngine.Action;

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
        private Animator _animator;
        private EventBus eventBus;
        private RuleEngine _ruleEngine;
        public Light light;
        public Slider _slider;
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
            radialMenu.SetActive(true);
            _ruleEngine = RuleEngine.GetInstance();
            eventBus = EventBus.GetInstance();
        }

        /*private void Update()
        {
            UpdateBrightness();
        }*/

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
            //RemoveColorEventListener();
            /*text.text = "Please, select an object to continue";*/
            selectedObject = null;
            radialMenu.SetActive(true);
            colorPalette.SetActive(false);
            generalUIController.resetEditButtons();
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
            if(interactables.Count==0) return;
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
        /*public void ChangeColorEventListener()
        {
            if (selectedObject == null) return;
            
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
        }*/
        
        public void ShowObject()
        {
            selectedObject.SetActive(true);
        }

        public void HideObject()
        {
            selectedObject.SetActive(false);
        }

        public void DeleteObject()
        {
            Destroy(selectedObject);
            generalUIController.SetDebugText(selectedObject.name + " has been deleted");
            selectedObject = null;
        }

        public void WaveHand()
        {
            if (selectedObject.GetComponent<Animator>() != null)
            {
                _animator = selectedObject.GetComponent<Animator>();
                _animator.SetBool("isWaving", true);
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0))
                {
                    _animator.SetBool("isWaving", false);
                }
            }
            
        }
        
        public void Dancing()
        {
            if (selectedObject.GetComponent<Animator>() != null)
            {
                _animator = selectedObject.GetComponent<Animator>();
                _animator.SetBool("isDancing", true);
                //if the animation isDancing has finished, set the boolean to false
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0))
                {
                    _animator.SetBool("isDancing", false);
                }
            }
            
        }
        
        public void addAccessories(String accessory)
        {
            if (selectedObject.GetComponent<ECACharacter>() != null)
            {
                GameObject accessoryObject = selectedObject.transform.Find(accessory).gameObject;
                if (!accessoryObject.activeInHierarchy)
                {
                    accessoryObject.SetActive(true);
                }
            }
        }
        
        public void removeAccessories(String accessory)
        {
            if (selectedObject.GetComponent<ECACharacter>() != null)
            {
                GameObject accessoryObject = selectedObject.transform.Find(accessory).gameObject;
                if (accessoryObject.activeInHierarchy)
                {
                    accessoryObject.SetActive(false);
                }
            }
        }

        //TODO: da spostare in ECALight
        private void UpdateBrightness()
        {
            light.intensity = _slider.Value;
        }

        /// <summary>
        /// <b> CreateAndPublishAction </b> creates an action and publishes it to the event bus
        /// </summary>
        /// <param name="actionName"></param>
        public void CreateAndPublishAction(string actionName)
        {
            switch (actionName)
            {
                case "Show":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "shows"));
                    break;
                
                case "Hide":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "hides"));
                    break;
                
                case "Delete":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "deleted"));
                    break;
                
                case "GravityON":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "gravityON"));
                    break;
                
                case "GravityOFF":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "gravityOFF"));
                    break;
                
                case "ColorRed":
                    ECAColor red = new ECAColor("red");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", red));
                    break;
                
                case "ColorBlue":
                    ECAColor blue = new ECAColor("blue");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", blue));
                    break;

                case "ColorGreen":
                    ECAColor green = new ECAColor("green");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", green));
                    break;

                case "ColorPurple":
                    ECAColor purple = new ECAColor("purple");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", purple));
                    break;

                case "ColorGray":
                case "ColorGrey":
                    ECAColor gray = new ECAColor("gray");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", gray));                    
                    break;

                case "ColorYellow":
                    ECAColor yellow = new ECAColor("yellow");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", yellow));
                    break;

                case "ColorCyan":
                    ECAColor cyan = new ECAColor("cyan");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", cyan));                    
                    break;

                case "ColorWhite":
                    ECAColor white = new ECAColor("white");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", white));
                    break;
                case "ColorBlack":
                    ECAColor black = new ECAColor("black");
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "color", "to", black));
                    break;
                    
                case "WaveHand":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "waves","hand", "to", SelectedObject));
                    break;
                
                case "Dance":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "dance"));
                    break;
                
                case "TurnOn":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "modeON"));
                    break;
                
                case "TurnOff":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "modeOFF"));
                    break;
                
                case "Volume":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "volume"));
                    break;
                
                case "Skyboxes":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "skybox", "to", SelectedObject));
                    break;
                
                case "Floors":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "changes", "floor", "to", SelectedObject));
                    break;
                
            }
        }
    }
}