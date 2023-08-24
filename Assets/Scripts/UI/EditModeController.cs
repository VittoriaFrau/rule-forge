using System;
using System.Collections.Generic;
using System.Linq;
using ECAPrototyping.RuleEngine;
using ECAPrototyping.Utils;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Action = ECAPrototyping.RuleEngine.Action;
using RenderSettings = UnityEngine.RenderSettings;

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
        private Light _light;
        private GameObject _skybox;
        private Renderer plane;
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
            _light = GameObject.Find("Directional Light").GetComponent<Light>();
            _skybox = GameObject.Find("skybox");
            plane = GameObject.FindGameObjectWithTag("Plane").GetComponent<Renderer>();
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
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "waves hand"));
                    break;
                
                case "Dance":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "dance"));
                    break;
                
                case "TurnOn":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "TurnON"));
                    break;
                
                case "TurnOff":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "TurnOFF"));
                    break;
                
                case "Volume":
                    _ruleEngine.ExecuteAction(new Action(SelectedObject, "volume"));
                    break;
                
                case "Brightness":
                    _ruleEngine.ExecuteAction(new Action(_light.gameObject, "change brightness"));
                    break;

                case "Floor_Grass":
                    _ruleEngine.ExecuteAction(new Action(plane.gameObject, "changes", "floor", "to", "Grass"));
                    break;
                
                case "Floor_Rocks":
                    _ruleEngine.ExecuteAction(new Action(plane.gameObject, "changes", "floor", "to", "Rocks"));
                    break;
                
                case "Floor_Wood":
                    _ruleEngine.ExecuteAction(new Action(plane.gameObject, "changes", "floor", "to", "Wood"));
                    break;
                
                case "Skybox_Day":
                    _ruleEngine.ExecuteAction(new Action(_skybox, "changes skybox","Day"));
                    break;
                
                case "Skybox_Sunset":
                    _ruleEngine.ExecuteAction(new Action(_skybox, "changes skybox", "Sunset"));
                    break;
                
                case "Skybox_Night":
                    _ruleEngine.ExecuteAction(new Action(_skybox, "changes skybox", "Night"));
                    break;
                
            }
        }
    }
}