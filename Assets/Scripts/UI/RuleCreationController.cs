using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
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
        public GameObject ruleMenu;
        /*public List<Material> normalMaterials;
        public List<Material> shiningMaterials;*/
        private GameObject OpenXRRightHandController, OpenXRLeftHandController;
        /*public GameObject shiningLeft;*/
        private GameObject RightHand, LeftHand;
        private Material normalMaterial;
        public Material shiningMaterial;
        
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
            generalUIController.SetDebugText("Selected modality: " + _modality);
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
        }
        
        private void ActivateTouchModality()
        {
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
            ruleMenu.SetActive(true);
        }

        public void StopRecording()
        {
            //TODO 
        }

        public void StartRecording()
        {
            //TODO all the objects in the scene
            GameObject cube = GameObject.Find("Cube");
            ObjectManipulator objectManipulator = cube.GetComponent<ObjectManipulator>();

            switch (_modality)
            {
                case Modalities.Eyegaze:
                    //TODO fare prove controllando interactor su oculus
                    objectManipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log(interactor); Debug.Log("Hover entered");
                    });
                    objectManipulator.onHoverExited.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Hover exited"); });
                    break;
                case Modalities.Laser:
                    //TODO fare prove controllando interactor su oculus
                    objectManipulator.onHoverEntered.AddListener(interactor =>
                    {
                        Debug.Log(interactor); Debug.Log("Hover entered");
                    });
                    objectManipulator.onHoverExited.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Hover exited"); });
                    break;
                case Modalities.Touch:
                    //attach listener to object manipulator manipulation started event
                    UnityAction manipulationStarted = () => { Debug.Log("On clicked"); };
                    objectManipulator.OnClicked.AddListener(manipulationStarted);
                    objectManipulator.onSelectEntered.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Select entered"); });
                    objectManipulator.onSelectExited.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Select exited"); });
                    break;
                
            }

        }

        //TODO: per chiudere il menu
        public void DeActivateNewRule()
        {
            
        }
    }
}