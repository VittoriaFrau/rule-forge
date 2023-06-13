using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI
{
    public class RuleCreationController:MonoBehaviour
    {
        private enum Modalities
        {
            None, 
            Headgaze,
            Touch,
            Laser,
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
            //HideModalitiesBubbles();
            
            
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
                headGazePointerInstance.GetComponent<Renderer>().material = shiningLaserMaterial;
            });
            
            gazeInteractor.GetComponent<FuzzyGazeInteractor>().hoverExited.AddListener((GameObject) =>
            {
                headGazePointerInstance.GetComponent<Renderer>().material = normalLaserMaterial;
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
            ShowModalitiesBubbles();
            generalUIController.SetDebugText("Selected modality: " + _modality 
                                                                   + " use your modality to interact with any object in the scene");
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
            
            //Remove the listeners
            foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
            {
                RemoveListener(go);
            }
            
        }
        
        
        

        private void DeActivateTouchModality()
        {
            //Color of the hands back to the normal
            RightHand.GetComponent<SkinnedMeshRenderer>().material = normalTouchMaterial;
            LeftHand.GetComponent<SkinnedMeshRenderer>().material = normalTouchMaterial;
            
            //Remove the listeners
            foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
            {
                RemoveListener(go);
            }
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
            
        }

        public void StartRecording()
        {
            generalUIController.SetDebugText("Recording started. Please, interact with an object");
            
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
                    //TODO fare prove controllando interactor su oculus
                    manipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Hover entered");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                    });
                    manipulator.onHoverExited.AddListener(interactor => { Debug.Log(manipulator.gameObject.name + " Hover exited"); });
                    break;
                case Modalities.Laser:
                    manipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log("Hover entered");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                    });
                    manipulator.onHoverExited.AddListener(interactor => { Debug.Log(manipulator.gameObject.name +" Hover exited"); });
                    break;
                case Modalities.Touch:
                    //attach listener to object manipulator manipulation started event
                    UnityAction manipulationStarted = () =>
                    {
                        Debug.Log(manipulator.gameObject.name + " On clicked");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " On clicked");
                    };
                    manipulator.OnClicked.AddListener(manipulationStarted);
                    manipulator.onSelectEntered.AddListener(interactor =>
                    {
                       Debug.Log(manipulator.gameObject.name + " Select entered");
                       generalUIController.SetDebugText(manipulator.gameObject.name + " Select entered");
                    });
                    manipulator.onSelectExited.AddListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Select exited");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Select exited");
                    });
                    break;
                
            }
        }

        private void RemoveListener(ObjectManipulator manipulator)
        {
             switch (_modality)
            {
                case Modalities.Headgaze:
                    //TODO fare prove controllando interactor su oculus
                    manipulator.onHoverEntered.RemoveListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Hover entered");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                    });
                    manipulator.onHoverExited.RemoveListener(interactor => { Debug.Log(manipulator.gameObject.name + " Hover exited"); });
                    break;
                case Modalities.Laser:
                    //TODO fare prove controllando interactor su oculus
                    manipulator.onHoverEntered.RemoveListener(interactor =>
                    {
                        Debug.Log("Hover entered");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Hover entered");
                    });
                    manipulator.onHoverExited.RemoveListener(interactor => { Debug.Log(manipulator.gameObject.name +" Hover exited"); });
                    break;
                case Modalities.Touch:
                    //attach listener to object manipulator manipulation started event
                    UnityAction manipulationStarted = () =>
                    {
                        Debug.Log(manipulator.gameObject.name + " On clicked");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " On clicked");
                    };
                    manipulator.OnClicked.RemoveListener(manipulationStarted);
                    manipulator.onSelectEntered.RemoveListener(interactor =>
                    {
                       Debug.Log(manipulator.gameObject.name + " Select entered");
                       generalUIController.SetDebugText(manipulator.gameObject.name + " Select entered");
                    });
                    manipulator.onSelectExited.RemoveListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Select exited");
                        generalUIController.SetDebugText(manipulator.gameObject.name + " Select exited");
                    });
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
    }
}