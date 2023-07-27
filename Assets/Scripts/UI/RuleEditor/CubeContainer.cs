using System;
using System.Linq;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using TMPro;
using UnityEngine;

namespace UI.RuleEditor
{
    public class CubeContainer:MonoBehaviour
    {
        public GameObject cubeContainerPrefab;  // Prefab of the cube container to be instantiated
        private bool isInstantiating = false;   // Flag to prevent multiple instantiations on collision
        private string trigger;                 // Indicates whether the container belongs to "when" or "then" branch
        private GameObject equivalenceContainer;   // Reference to the equivalence container for each cube container
        private GameObject whenText, thenText;  // References to the "when" and "then" text objects
        private string whenString, thenString;  // Current text contents of the "when" and "then" text objects
        private enum ContainerType { Equivalence, Sequential }
        private ContainerType containerType;    // Indicates whether the container is equivalence (OR) or sequential

        private void Start()
        {
            //find if this instance is a child of a then or when
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
            // Find the parent EquivalenceRow container
            foreach (Transform child in parent.parent){
                if (child.name == "EquivalenceRow"){
                    equivalenceContainer = child.gameObject;
                    break;
                }
            }
            
            // Find the "when" and "then" text objects with appropriate tags
            whenText = GameObject.FindGameObjectsWithTag("RuleText").FirstOrDefault(o => o.name == "WhenText");
            thenText = GameObject.FindGameObjectsWithTag("RuleText").FirstOrDefault(o => o.name == "ThenText");
            
            containerType = transform.parent.name switch
            {
                //Check if the parent of collision.gameobject is an equivalence or sequential container
                "EquivalenceRow" => ContainerType.Equivalence,
                "SequentialRow" => ContainerType.Sequential,
                _ => containerType
            };
        }

        

        private void OnCollisionEnter(Collision collision)
        {
            // Check if collision occurred with a RuleCube and not already instantiating
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
                
                //Create a new instance of the container in the equivalence position
                GameObject instantiatedParallel = Instantiate(cubeContainerPrefab);
                instantiatedParallel.transform.parent = equivalenceContainer.transform;
                instantiatedParallel.transform.rotation = gameObject.transform.rotation;
                instantiatedParallel.transform.localScale = transform.localScale;
                var localPosition = instantiatedParallel.transform.localPosition;
                localPosition = new Vector3(localPosition.x,  
                    localPosition.y,3.99f);
                instantiatedParallel.transform.localPosition = localPosition;

                collision.gameObject.GetComponent<ObjectManipulator>().enabled = true;
                
                //Update text
                GetPresentRule();
                string cubeDescription = Utils.GetRuleDescriptionFromCubePrefab(collision.gameObject);
                string logicalOperator= containerType == ContainerType.Equivalence ? "OR" : "THEN";
                Utils.GenerateTextFromCubePosition(trigger.Equals("When") ? whenText : thenText, trigger.Equals("When") ? 
                    whenString : thenString, cubeDescription, logicalOperator);

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