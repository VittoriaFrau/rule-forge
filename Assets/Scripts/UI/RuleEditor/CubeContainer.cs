using System;
using System.Linq;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using TMPro;
using UnityEngine;

namespace UI.RuleEditor
{
    public class CubeContainer:MonoBehaviour
    {
        //private Transform cubePosition;
        public GameObject cubeContainerPrefab;
        private bool isInstantiating = false;
        private string trigger; //when or then
        private GameObject parallelContainer;
        private GameObject whenText, thenText;
        private string whenString, thenString;
        
        private void Start()
        {
            //find if this istance is a child of a then or when
            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                if (parent.name == "When" || parent.name == "Then")
                {
                    trigger = parent.name;
                    break;
                }

                parent = parent.parent;
            }
            
            parent = gameObject.transform.parent;

            foreach (Transform child in parent.parent){
                if (child.name == "ParallelRow"){
                    parallelContainer = child.gameObject;
                    break;
                }
            }
            
            whenText = GameObject.FindGameObjectsWithTag("RuleText").FirstOrDefault(o => o.name == "WhenText");
            thenText = GameObject.FindGameObjectsWithTag("RuleText").FirstOrDefault(o => o.name == "ThenText");
        }

        

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("RuleCubes") && !isInstantiating)
            {
                isInstantiating = true;
                //Deactivate object manipulator from object
                collision.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                
                //Position the cube in the right position
                Vector3 positionContainer = gameObject.transform.position;
                collision.gameObject.transform.position = new Vector3(positionContainer.x, positionContainer.y + 0.1f ,positionContainer.z);

                //Set the collision transform velocities to 0
                collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
                collision.gameObject.transform.rotation = gameObject.transform.rotation;
                
                
                //Create a new instance of the container in the sequence position
                GameObject instantiated = Instantiate(cubeContainerPrefab);
                instantiated.transform.parent = transform.parent;
                instantiated.transform.rotation = gameObject.transform.rotation;
                instantiated.transform.localScale = transform.localScale;
                instantiated.transform.localPosition = new Vector3(instantiated.transform.localPosition.x,  
                    instantiated.transform.localPosition.y,3.99f);
                
                //Create a new instance of the container in the parallel position
                GameObject instantiatedParallel = Instantiate(cubeContainerPrefab);
                instantiatedParallel.transform.parent = parallelContainer.transform;
                instantiatedParallel.transform.rotation = gameObject.transform.rotation;
                instantiatedParallel.transform.localScale = transform.localScale;
                instantiatedParallel.transform.localPosition = new Vector3(instantiatedParallel.transform.localPosition.x,  
                    instantiatedParallel.transform.localPosition.y,3.99f);

                collision.gameObject.GetComponent<ObjectManipulator>().enabled = true;
                
                //Update text
                GetPresentRule();
                string cubeDescription = collision.gameObject.transform.Find("Image").gameObject.transform.Find("Text").GetComponent<TextMeshPro>().text;
                Utils.GenerateTextFromCubePosition(trigger.Equals("When") ? whenText : thenText, trigger.Equals("When") ? 
                    whenString : thenString, cubeDescription);

                /*StartCoroutine(ResetInstantiation());*/
            }
        }
        
        private System.Collections.IEnumerator ResetInstantiation()
        {
            yield return new WaitForSeconds(1f);
            isInstantiating = false;
        }

        private void GetPresentRule()
        {
            whenString = whenText.GetComponent<TextMeshProUGUI>().text;
            thenString = thenText.GetComponent<TextMeshProUGUI>().text;
        }
    }
}