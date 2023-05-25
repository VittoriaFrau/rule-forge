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
            Eyegaze,
            Touch,
            Laser
        }
        
        private Modalities _modality;
        private GeneralUIController generalUIController;
        public List<GameObject> modalitiesBubbles;
        public GameObject BackButton, RecordButton, StopButton;
       
        //Touch modality attributes
        private GameObject OpenXRRightHandController, OpenXRLeftHandController;
        private GameObject RightHand, LeftHand;
        private Material normalMaterial;
        public Material shiningMaterial;
        public GameObject interactables;
        
        
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
            _modality = (Modalities) System.Enum.Parse(typeof(Modalities), modality);
            generalUIController.SetDebugText("Selected modality: " + _modality 
                                                                   + " use your modality to interact with any object in the scene");
            HideModalitiesBubbles();
            switch (_modality)
            {
               case Modalities.Eyegaze:
                   //TODO
                   break;
               case Modalities.Laser:
                   //TODO
                     break;
               case Modalities.Touch:
                   ActivateTouchModality();
                   break;
            }
            //TODO show buttons in radial menu
            
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
            
            normalMaterial = LeftHand.GetComponent<SkinnedMeshRenderer>().material;
            
            RightHand.GetComponent<SkinnedMeshRenderer>().material = shiningMaterial;
            LeftHand.GetComponent<SkinnedMeshRenderer>().material = shiningMaterial;
            
            
        }

        private void DeActivateTouchModality()
        {
            //Color of the hands back to the normal
            RightHand.GetComponent<SkinnedMeshRenderer>().material = normalMaterial;
            LeftHand.GetComponent<SkinnedMeshRenderer>().material = normalMaterial;
        }

        public void HideModalitiesBubbles()
        {
            foreach (var go in modalitiesBubbles)
            {
                go.SetActive(false);
            }
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
            StopButton.GetComponent<PressableButton>().enabled = false;
            //ruleMenu.SetActive(true);
        }

        public void StopRecording()
        {
            //TODO 
            StopButton.GetComponent<PressableButton>().enabled = false;
            RecordButton.GetComponent<PressableButton>().enabled = true;
            
        }

        public void StartRecording()
        {
            //Make the record button not interactable
            RecordButton.GetComponent<PressableButton>().enabled = false;
            StopButton.GetComponent<PressableButton>().enabled = true;

            foreach (var go in interactables.transform.GetComponentsInChildren<ObjectManipulator>())
            {
                AddListener(go);
            }

        }

        private void AddListener(ObjectManipulator manipulator)
        {
            switch (_modality)
            {
                case Modalities.Eyegaze:
                    //TODO fare prove controllando interactor su oculus
                    manipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Hover entered");
                    });
                    manipulator.onHoverExited.AddListener(interactor => { Debug.Log(manipulator.gameObject.name + " Hover exited"); });
                    break;
                case Modalities.Laser:
                    //TODO fare prove controllando interactor su oculus
                    manipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log("Hover entered");
                    });
                    manipulator.onHoverExited.AddListener(interactor => { Debug.Log(manipulator.gameObject.name +" Hover exited"); });
                    break;
                case Modalities.Touch:
                    //attach listener to object manipulator manipulation started event
                    UnityAction manipulationStarted = () =>
                    {
                        Debug.Log(manipulator.gameObject.name + " On clicked");
                    };
                    manipulator.OnClicked.AddListener(manipulationStarted);
                    manipulator.onSelectEntered.AddListener(interactor =>
                    {
                       Debug.Log(manipulator.gameObject.name + " Select entered");
                    });
                    manipulator.onSelectExited.AddListener(interactor =>
                    {
                        Debug.Log(manipulator.gameObject.name + " Select exited");
                    });
                    break;
                
            }
        }
        

        //TODO: per chiudere il menu
        public void DeActivateNewRule()
        {
            // Nascondere bolle se presenti
            HideModalitiesBubbles();
            // Nascondere opzioni menu
            BackButton.SetActive(false);
            RecordButton.SetActive(false);
            StopButton.SetActive(false);
        }
    }
}