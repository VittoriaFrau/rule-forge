using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
using UnityEngine.Events;
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
        private Material normalLaserMaterial;
        public Material shiningLaserMaterial;
        
        //Headgaze modality attributes
        public GameObject headGazePointer;
        public GameObject gazeInteractor;
        private GameObject headGazePointerInstance;
        public Material normalHeadGazeMaterial;
        public Material shiningHeadGazeMaterial;
        
        
        //Recording
        private List<ECAEvent> _events = new();
        public GameObject ruleEditorPlate;
        public GameObject ruleCubePrefab;
        public GameObject cubePlate;

        //Category choice
        public GameObject categoryMenu;
        
        private void Start()
        {
            generalUIController = this.gameObject.GetComponent<GeneralUIController>();
            //find the gameobject looking for the tag and then filtering by name
            OpenXRRightHandController = GameObject.FindGameObjectsWithTag("handController").FirstOrDefault(obj => obj.name 
                == "MRTK RightHand Controller");
            OpenXRLeftHandController = GameObject.FindGameObjectsWithTag("handController").FirstOrDefault(obj => obj.name 
                == "MRTK LeftHand Controller");
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
            WsClient.StartSocket(_events);
        }

        private void ActivateLaserModality()
        {
            rightHandLaserPointer = OpenXRRightHandController.transform.Find("Far Ray").gameObject.transform.Find("BendyRay").gameObject;
            leftHandLaserPointer = OpenXRLeftHandController.transform.Find("Far Ray").gameObject.transform.Find("BendyRay").gameObject;

            normalLaserMaterial = rightHandLaserPointer.GetComponent<LineRenderer>().material;
            
            //Assign the new material to both laser hands
            rightHandLaserPointer.GetComponent<LineRenderer>().material = shiningLaserMaterial;
            leftHandLaserPointer.GetComponent<LineRenderer>().material = shiningLaserMaterial;
            
            //Disappear the Bubble of the modality
            HideModalitiesBubble("Laser");
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
        
        public void ActivateNewRuleMode()
        {
            generalUIController.NewRuleState();
            ShowModalitiesBubbles();
        }

        public void StopRecording()
        {
            //TODO 
            StopButton.SetActive(false);
            RecordButton.SetActive(true);
            
            generalUIController.SetDebugText("Recording stopped.");
            if(categoryMenu.activeSelf)
                categoryMenu.SetActive(false);
            WsClient.IsRecording= false;
            
            
            
            //Set the rule plate visible
            ruleEditorPlate.SetActive(true);

            //Generate the cubes using the list of events
            Utils.GenerateCubesFromEventList(_events, ruleCubePrefab, cubePlate);

        }

        public void StartRecording()
        {
            generalUIController.SetDebugText("Recording started. Please, interact with an object");
            
            if (_modality == Modalities.None)
            {
                Debug.Log("No modality selected");
                generalUIController.SetDebugText("No modality selected, please select one");
                return;
            }
            
            //Clean the event list
            _events.Clear();
            
            //Alert WsClient that we are recording
            WsClient.IsRecording = true;
            
            //Make the record button not interactable
            StopButton.SetActive(true);
            RecordButton.SetActive(false);

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

        IEnumerator TakeScreenShot()
        {
            yield return new WaitForEndOfFrame();
            var texture = ScreenCapture.CaptureScreenshotAsTexture();
            //Set the texture of the last events in the list to the screenshot
            _events.Last().Texture = texture;
        }
        
        private void AddHeadGazeListener(ObjectManipulator manipulator)
        {
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverEntered.AddListener((GameObject) =>
            {
                Debug.Log(manipulator.gameObject.name + " Hover entered");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Hover Entered" );
                //TODO non farlo hard coded ma dandogli i nomi giusti (cubo, ...)
                categoryMenu.SetActive(true);
                
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Headgaze, "HoverEntered"));
                StartCoroutine(TakeScreenShot());
                generalUIController.SetDebugText("You are selecting the cube, any object or any shape? By default, the object is a cube.");
                //TODO metti un meccanismo che modifica il tipo di evento dandogli la tipologia di oggetto una volta selezionato (cubo, oggetto generico, ...)
                        
            });
            
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverExited.AddListener((GameObject) =>
            {
                Debug.Log(manipulator.gameObject.name + " Hover exited");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Hover exited");
                _events.Add(new ECAEvent(manipulator.gameObject, _modality, "HoverEntered"));
                StartCoroutine(TakeScreenShot());

            });
        }
        
        private void AddLaserListener(ObjectManipulator manipulator)
        {
            manipulator.onHoverEntered.AddListener(interactor =>
            {
                Debug.Log("Hover entered");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Laser, "Hover Entered"));
                StartCoroutine(TakeScreenShot());
                
                generalUIController.SetDebugText("You are selecting the cube, any object or any shape? By default, the object is a cube.");
                categoryMenu.SetActive(true);

            });
            manipulator.onHoverExited.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name +" Hover exited"); 
                
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Laser, "Hover Exited"));
                StartCoroutine(TakeScreenShot());
                //categoryMenu.SetActive(false);

            });
        }

        private void AddTouchListener(ObjectManipulator manipulator)
        {
            //attach listener to object manipulator manipulation started event
            UnityAction manipulationStarted = () =>
            {
                Debug.Log(manipulator.gameObject.name + " On clicked");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " On clicked");
                
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Touch, "Clicked"));
                StartCoroutine(TakeScreenShot());
                
                generalUIController.SetDebugText("You are selecting the cube, any object or any shape? By default, the object is a cube.");
                categoryMenu.SetActive(true);
            };
            manipulator.OnClicked.AddListener(manipulationStarted);
            manipulator.onSelectEntered.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name + " Select entered");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Select entered");
                generalUIController.SetDebugText("You are selecting the cube, any object or any shape? By default, the object is a cube.");
                
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Touch, "Clicked"));
                StartCoroutine(TakeScreenShot());
                categoryMenu.SetActive(true);


            });
            manipulator.onSelectExited.AddListener(interactor =>
            {
                Debug.Log(manipulator.gameObject.name + " Select exited");
                //generalUIController.SetDebugText(manipulator.gameObject.name + " Select exited");
                //Note: event should be added before starting the coroutine
                _events.Add(new ECAEvent(manipulator.gameObject, Modalities.Touch, "Select exited"));
                StartCoroutine(TakeScreenShot());
                //categoryMenu.SetActive(false);
            });
            
        }
        
    }
}