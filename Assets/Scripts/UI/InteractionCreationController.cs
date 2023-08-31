using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UI.RuleEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Action = ECAPrototyping.RuleEngine.Action;
using Object = UnityEngine.Object;

namespace UI
{
    public class InteractionCreationController:MonoBehaviour
    {
        public enum Modalities
        {
            None, 
            Headgaze,
            Touch,
            Laser,
            Microgesture
        }

        public enum CategoryObjectSelected
        {
            GameObject,
            Category,
            AllObject
        }
        
        private Modalities _modality;
        private GeneralUIController generalUIController;
        private EditModeController editModeController;
        public List<GameObject> modalitiesBubbles;
        public GameObject BackButton, RecordButton, StopButton;
        
        //Touch modality attributes
        private GameObject OpenXRRightHandController, OpenXRLeftHandController;
        private GameObject RightHand, LeftHand;
        private Material normalTouchMaterial;
        public Material shiningTouchMaterial;
        public GameObject interactables;
        
        //Microgesture
        
        
        //Laser modality attributes
        private GameObject rightHandLaserPointer, leftHandLaserPointer;
        private LineRenderer rightHandLaserPointerLineRenderer, leftHandLaserPointerLineRenderer;
        private Material normalLaserMaterial;
        public Material shiningLaserMaterial;
        
        //Headgaze modality attributes
        public GameObject headGazePointer;
        public GameObject gazeInteractor;
        private GameObject headGazePointerInstance;
        public Material normalHeadGazeMaterial;
        public Material shiningHeadGazeMaterial;
        
        
        //Recording
        private List<ECAEvent> _modalityEvents = new();
        private List<ECAEvent> _actionEvents = new();
        public GameObject ruleEditorPlate;
        public GameObject modalityRuleCubePrefab;
        public GameObject actionRuleCubePrefab;
        public GameObject actionRuleCubePrefabVariant;
        public GameObject cubePlate;
        public GameObject screenshotCamera;
        private ScreenshotCamera _screenshotCamera;

        //Category choice
        public GameObject categoryMenu;
        public TextMeshProUGUI SingleObjectLabel;
        public TextMeshProUGUI CategoryLabel;
        public FontIconSelector CategoryIcon;
        
        //Rule composition
        public GameObject removableBarrier;
        private Dictionary<GameObject, Vector3> _originalPositions = new(); //positions of the cubes to easily revert it
        public TextMeshProUGUI whenText, thenText;
        private RuleManager _ruleManager;

        private void Start()
        {
            generalUIController = this.gameObject.GetComponent<GeneralUIController>();
            editModeController = this.gameObject.GetComponent<EditModeController>();
            //find the gameobject looking for the tag and then filtering by name
            OpenXRRightHandController = GameObject.FindGameObjectsWithTag("handController").FirstOrDefault(obj => obj.name 
                == "MRTK RightHand Controller");
            OpenXRLeftHandController = GameObject.FindGameObjectsWithTag("handController").FirstOrDefault(obj => obj.name 
                == "MRTK LeftHand Controller");
            _screenshotCamera =screenshotCamera.GetComponent<ScreenshotCamera>();
            _ruleManager = this.gameObject.GetComponent<RuleManager>();
        }

        public void SelectModality(string modality)
        {
            if(_modality != Modalities.None) DeActivateCurrentModality();
                _modality = (Modalities) System.Enum.Parse(typeof(Modalities), modality);
            generalUIController.SetDebugText("Selected modality: " + _modality 
                                                                   + " use your modality to interact with any object in the scene");
            switch (_modality)
            {
               case Modalities.Headgaze:
                   ActivateHeadGazeModality();
                   break;
               case Modalities.Laser:
                   ActivateLaserModality();
                     break;
               case Modalities.Touch:
                   ActivateTouchModality();
                   break;
            }

            if (WsClient.IsRecording)
            {
                foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
                {
                    AddListener(go);
                }
            }
        }

        public void ClearEventLists()
        {
            _modalityEvents.Clear();
            _actionEvents.Clear();
        }
        
        private void ActivateHeadGazeModality()
        {
            //Instantiate the headgaze pointer inside the gaze interactor object
            headGazePointerInstance = Instantiate(headGazePointer, gazeInteractor.transform);
            //Set z axes to 0.33
            headGazePointerInstance.transform.localPosition = new Vector3(0,0,0.33f);
            
            //Change the material of the gaze pointer everytime the user looks at an object
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverEntered.AddListener((GameObject) =>
            {
                headGazePointerInstance.GetComponent<Renderer>().material = shiningHeadGazeMaterial;
            });
            
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverExited.AddListener((GameObject) =>
            {
                headGazePointerInstance.GetComponent<Renderer>().material = normalHeadGazeMaterial;
            });
            
            //Disappear the Bubble of the modality
            HideModalitiesBubble("Headgaze");
        }

        private void DeActivateHeadGazeModality()
        {
            Destroy(headGazePointerInstance);
        }

        private void DeActivateCurrentModality()
        {
            switch (_modality)
            {
                case Modalities.Headgaze:
                    DeActivateHeadGazeModality();
                    break;
                case Modalities.Laser:
                    DeActivateLaserModality();
                    break;
                case Modalities.Touch:
                    DeActivateTouchModality();
                    break;
            }
            //Remove the listeners
            foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
            {
                RemoveListener(go);
            }
            
            ShowModalitiesBubbles();
            generalUIController.SetDebugText("Selected modality: " + _modality 
                                                                   + " use your modality to interact with any object in the scene");
            categoryMenu.SetActive(false);
        }
        
        private void ActivateTouchModality()
        {
            //Color of the hands to the shining
            GameObject modelParentRight = OpenXRRightHandController.transform
                .Find("[MRTK RightHand Controller] Model Parent").gameObject;
            GameObject modelParentLeft = OpenXRLeftHandController.transform
                .Find("[MRTK LeftHand Controller] Model Parent").gameObject;
            
            GameObject L_R_Hand = modelParentLeft.transform.Find("openxr_left_hand(Clone)").gameObject;
            GameObject R_R_Hand = modelParentRight.transform.Find("openxr_right_hand(Clone)").gameObject;
            
            LeftHand = L_R_Hand.transform.Find("R_Hand").gameObject;
            RightHand = R_R_Hand.transform.Find("R_Hand").gameObject;
            
            normalTouchMaterial = LeftHand.GetComponent<SkinnedMeshRenderer>().material;
            
            RightHand.GetComponent<SkinnedMeshRenderer>().material = shiningTouchMaterial;
            LeftHand.GetComponent<SkinnedMeshRenderer>().material = shiningTouchMaterial;
            
            //Disappear the Bubble of the modality
            HideModalitiesBubble("Touch");
            
            //Microgesture listener
            WsClient.StartSocket();
        }

        private void ActivateLaserModality()
        {
            rightHandLaserPointer = OpenXRRightHandController.transform.Find("Far Ray").gameObject.transform.Find("BendyRay").gameObject;
            leftHandLaserPointer = OpenXRLeftHandController.transform.Find("Far Ray").gameObject.transform.Find("BendyRay").gameObject;
            
            rightHandLaserPointerLineRenderer = rightHandLaserPointer.GetComponent<LineRenderer>();
            leftHandLaserPointerLineRenderer = leftHandLaserPointer.GetComponent<LineRenderer>();

            normalLaserMaterial = rightHandLaserPointer.GetComponent<LineRenderer>().material;
            
            //Assign the new material to both laser hands
            rightHandLaserPointerLineRenderer.material = shiningLaserMaterial;
            leftHandLaserPointerLineRenderer.material = shiningLaserMaterial;

            //Disappear the Bubble of the modality
            HideModalitiesBubble("Laser");
        }
        
        public void SetLaserPointLineWidth(float width)
        {
            leftHandLaserPointerLineRenderer.widthMultiplier = width;
            rightHandLaserPointerLineRenderer.widthMultiplier = width;
        }

        private void DeActivateLaserModality()
        {
            //Color of the laser back to the normal
            rightHandLaserPointer.GetComponent<LineRenderer>().material = normalLaserMaterial;
            leftHandLaserPointer.GetComponent<LineRenderer>().material = normalLaserMaterial;
        }
        
        private void DeActivateTouchModality()
        {
            //Color of the hands back to the normal
            RightHand.GetComponent<SkinnedMeshRenderer>().material = normalTouchMaterial;
            LeftHand.GetComponent<SkinnedMeshRenderer>().material = normalTouchMaterial;

            WsClient.StopSocket();
        }

        public void HideModalitiesBubbles()
        {
            foreach (var go in modalitiesBubbles)
            {
                go.SetActive(false);
            }
        }
        
        private void HideModalitiesBubble(string modality)
        {
            GameObject go = modalitiesBubbles.FirstOrDefault(obj => obj.name == modality);
            go.SetActive(false);
        }
        
        public void ShowModalitiesBubbles()
        {
            foreach (var go in modalitiesBubbles)
            {
                go.SetActive(true);
            }
        }

        public void ShowModalitiesBubblesExceptModality()
        {
            GameObject go = modalitiesBubbles.FirstOrDefault(obj => obj.name == _modality.ToString());
            foreach (var bubble in modalitiesBubbles)
            {
                if(bubble != go)
                    bubble.SetActive(true);
            }
        }
        
        public void ActivateNewRuleMode()
        {
            generalUIController.NewRuleState();
            ShowModalitiesBubbles();
        }

        public void StopRecording()
        {
            generalUIController.isRecording = false;
            StopButton.SetActive(false);
            if(generalUIController.UIstate != GeneralUIController.UIState.Default)
                RecordButton.SetActive(true);
            screenshotCamera.SetActive(false);

            generalUIController.SetDebugText("Recording stopped.");
            
           // generalUIController.AddCombineRulesButtonToRadialMenu();
            
            if(categoryMenu.activeSelf)
                categoryMenu.SetActive(false);
            WsClient.IsRecording= false;
            if (WsClient.ServerOpen)
            {
                WsClient.StopSocket();
            }
            
            _modalityEvents.AddRange(WsClient.MicrogestureEvents);

            DeActivateCurrentModality();
            HideModalitiesBubbles();

        }

        public void ActivateCombineRulesMode()
        {
            if (_modalityEvents.Count == 0 && _actionEvents.Count == 0)
            {
                generalUIController.SetDebugText("No recorded actions, please use the record button to record actions");
                return;
            }
                
            generalUIController.CombineRulesState();
            
            //Set the rule plate visible
            ruleEditorPlate.SetActive(true);
            
            //Barrier to prevent the cubes from falling
            removableBarrier.SetActive(true);

            //Generate the cubes using the list of events
            _originalPositions.Clear();
            _originalPositions = Utils.GenerateCubesFromEventList(_modalityEvents, _actionEvents, 
                modalityRuleCubePrefab, actionRuleCubePrefab, actionRuleCubePrefabVariant, cubePlate);

            removableBarrier.SetActive(false);
            
            _ruleManager.InitializeVariables();
            
            editModeController.ShowHideRadialMenu(false);
        }

        public void DeActivateRuleComposition()
        {
            _ruleManager.NewTask();
            
            //Set the rule plate visible
            ruleEditorPlate.SetActive(false);
            
            //Barrier to prevent the cubes from falling
            removableBarrier.SetActive(false);
            
            generalUIController.State = GeneralUIController.UIState.Default;
            generalUIController.DefaultState();
            generalUIController.DefaultRM();
            editModeController.ShowHideRadialMenu(true);

            //Clear events and action queues 
            ClearEventLists();
        }

        public void StartRecording()
        {
            generalUIController.isRecording = true;
            //Make the record button not interactable
            StopButton.SetActive(true);
            RecordButton.SetActive(false);
            
            switch (generalUIController.UIstate)
            {
                case GeneralUIController.UIState.NewRule:
                    RecordRule();
                    break;
                case GeneralUIController.UIState.EditMode:
                    RecordAction();
                    break;
            }
        }

        public void RecordAction()
        {
            generalUIController.SetDebugText("Recording started.");
            
            _actionEvents.Clear();
            
            screenshotCamera.SetActive(true);
        }


        public void RecordActionPressedButton(Action action, GameObject selectedObject)
        {
            ECAEvent ecaEvent = Utils.ConvertActionToECAEvent(action);
            if (!_actionEvents.Contains(ecaEvent))
            {
                _actionEvents.Add(ecaEvent);
                Debug.Log(ecaEvent);
                generalUIController.SetDebugText(ecaEvent.ToString());
                PrepareForActionScreenShot(selectedObject);
            }
        }

        public void RecordRule()
        {
            generalUIController.SetDebugText("Recording started. Please, interact with an object");
            
            if (_modality == Modalities.None)
            {
                Debug.Log("No modality selected");
                generalUIController.SetDebugText("No modality selected, please select one");
                return;
            }
            
            //Clean the event list
            _modalityEvents.Clear();
            
            //Alert WsClient that we are recording
            WsClient.IsRecording = true;

            //Activate screenshot camera
            screenshotCamera.SetActive(true);

            foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
            {
                AddListener(go);
            }
        }

        private void AddListener(ObjectManipulator manipulator)
        {
            switch (_modality)
            {
                case Modalities.Headgaze:
                    AddHeadGazeListener(manipulator);
                    break;
                case Modalities.Laser:
                    AddLaserListener(manipulator);
                    break;
                case Modalities.Touch:
                    AddTouchListener(manipulator);
                    break;
            }
            
        }

        private void RemoveListener(ObjectManipulator manipulator)
        {
             switch (_modality)
            {
                case Modalities.Headgaze:
                    gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverEntered.RemoveAllListeners();
                    gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverExited.RemoveAllListeners();
                    break;
                case Modalities.Laser:
                    manipulator.onHoverEntered.RemoveAllListeners();
                    manipulator.onHoverExited.RemoveAllListeners();
                    break;
                case Modalities.Touch:
                    manipulator.OnClicked.RemoveAllListeners();
                    manipulator.onSelectEntered.RemoveAllListeners();
                    manipulator.onSelectExited.RemoveAllListeners();
                    break;
            }
        }
        

        //TODO: per chiudere il menu
        public void DeActivateNewRule()
        {
            DeActivateCurrentModality();
            // Nascondere bolle se presenti
            HideModalitiesBubbles();
            // Nascondere opzioni menu
            BackButton.SetActive(false);
            RecordButton.SetActive(false);
            StopButton.SetActive(false);
        }

        IEnumerator TakeScreenShot(List<ECAEvent> _events)
        {
            yield return new WaitForEndOfFrame();
            var texture = ScreenCapture.CaptureScreenshotAsTexture();
            //Set the texture of the last events in the list to the screenshot
            _events.Last().Texture = texture;
        }
        
        private void AddHeadGazeListener(ObjectManipulator manipulator)
        {
            GameObject gameObject = manipulator.gameObject;
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverEntered.AddListener((GameObject) =>
            {
                Debug.Log(gameObject.name + " Hover entered");
                
                //Note: event should be added before starting the coroutine
                //ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Headgaze, "Entered");
                ECAEvent ecaEvent = new ECAEvent(gameObject, Modalities.Headgaze, "points");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(gameObject, Modalities.Headgaze);
                }

                PrepareCategoryMenu(gameObject);        
            });
            
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverExited.AddListener((GameObject) =>
            {
                Debug.Log(manipulator.gameObject.name + " Hover exited");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Hover exited");
                //ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Headgaze, "Exited");
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Headgaze, "stops");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Headgaze);
                }
            });
        }
        
        private void AddLaserListener(ObjectManipulator manipulator)
        {
            GameObject gameObject = manipulator.gameObject;
            manipulator.onHoverEntered.AddListener(interactor =>
            {
                Debug.Log("Hover entered");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                
                //Set the laser pointer line width for the screenshot
                SetLaserPointLineWidth(5.0f);
                
                //Note: event should be added before starting the coroutine
                //ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Laser, "Point Entered");
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Laser, "points");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Laser);
                }
                PrepareCategoryMenu(gameObject);
                
                

            });
            manipulator.onHoverExited.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name +" Hover exited"); 
                
                //Note: event should be added before starting the coroutine
                //ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Laser, "Hover Exited");
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Laser, "stops");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Laser);
                }

            });
        }

        private void AddTouchListener(ObjectManipulator manipulator)
        {
            GameObject gameObject = manipulator.gameObject;

            //attach listener to object manipulator manipulation started event
            UnityAction manipulationStarted = () =>
            {
                Debug.Log(manipulator.gameObject.name + " On clicked");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " On clicked");
                
                //Note: event should be added before starting the coroutine
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Touch, "Clicked");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Touch);
                }

                PrepareCategoryMenu(gameObject);
            };
            manipulator.OnClicked.AddListener(manipulationStarted);
            manipulator.onSelectEntered.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name + " Select entered");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Select entered");
                
                PrepareCategoryMenu(gameObject);
                
                //Note: event should be added before starting the coroutine
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Touch, "Select entered");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Touch);
                }

            });
            manipulator.onSelectExited.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name + " Select exited");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Select exited");
                //Note: event should be added before starting the coroutine
                //Add the event only if it doesn't exist already
                //ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Touch, "Select exited");
                ECAEvent ecaEvent = new ECAEvent(manipulator.gameObject, Modalities.Touch, "stops");
                if (!_modalityEvents.Contains(ecaEvent))
                {
                    _modalityEvents.Add(ecaEvent);
                    PrepareForModalityScreenshot(manipulator.gameObject, Modalities.Touch);
                }
                //categoryMenu.SetActive(false);
            });
        }

        public void PrepareForModalityScreenshot(GameObject gameObject, Modalities modality)
        {
            HideModalitiesBubbles();
            editModeController.ShowHideRadialMenu(false);
            _screenshotCamera.TakeModalityScreenshot(gameObject, modality, _modalityEvents.Last());
            ShowModalitiesBubblesExceptModality();
            editModeController.ShowHideRadialMenu(true);

        }

        public void ResetCubePositions()
        {
            //Repositioning the plate in case the user has moved it
            ruleEditorPlate.transform.localPosition= new Vector3(-14.4f, -119.0f, 774.0f);
            // Using the original position of the cube, position it again there
            foreach (var k in _originalPositions)
            {
                k.Key.transform.position = k.Value;
            }
            Utils.ClearTextDescription(whenText, thenText);

            Utils.ResetCubeContainers();
        }

        public void PrepareForActionScreenShot(GameObject gameObject)
        {
            editModeController.ShowHideRadialMenu(false);
            _screenshotCamera.TakeActionScreenshot(gameObject, _actionEvents.Last());
            editModeController.ShowHideRadialMenu(true);
        }

        public void PrepareCategoryMenu(GameObject gameObject)
        {
            string objectCategory = Utils.GetECALastScriptFromECAObject(gameObject);
            generalUIController.SetDebugText("Are you selecting the " + gameObject.name + ", any "+ objectCategory +" or any object?");
            categoryMenu.SetActive(true);
            
            CategoryLabel.text = objectCategory;
            SingleObjectLabel.text = gameObject.name;
            string icon = Utils.GetIconForECACategory(objectCategory);
            if(icon!= null)
                CategoryIcon.CurrentIconName = icon;
        }

    }
}