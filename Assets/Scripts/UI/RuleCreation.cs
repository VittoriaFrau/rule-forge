using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI
{
    public class RuleCreation:MonoBehaviour
    {
        private enum Modalities
        {
            None, 
            Eyegaze,
            Touch,
            Laser
        }
        
        private Modalities _modality;

        public void SetModality(string modality)
        {
            _modality = (Modalities) System.Enum.Parse(typeof(Modalities), modality);
            StartRecording();
        }

        public void StartRecording()
        {
            GameObject cube = GameObject.Find("Cube");
            ObjectManipulator objectManipulator = cube.GetComponent<ObjectManipulator>();
            //attach listener to object manipulator manipulation started event
            UnityAction manipulationStarted = () => { Debug.Log("On clicked"); };
            objectManipulator.OnClicked.AddListener(manipulationStarted);
            //TODO fare prove controllando interactor su oculus
            objectManipulator.onHoverEntered.AddListener(interactor =>
            {
                Debug.Log(interactor); Debug.Log("Hover entered");
            });
            objectManipulator.onHoverExited.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Hover exited"); });
            objectManipulator.onSelectEntered.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Select entered"); });
            objectManipulator.onSelectExited.AddListener(interactor => { Debug.Log(interactor); Debug.Log("Select exited"); });
            //objectManipulator.
        }
    }
}