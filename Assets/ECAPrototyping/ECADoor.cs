using System.Collections;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    
        /// <summary>
        /// A <b>Door</b> is a type of ECAEnvironment, it represents a door.
        /// </summary>
        [DisallowMultipleComponent]
        [ECARules4All("door")]
        [RequireComponent(typeof(ECAEnvironment))]

        public class ECADoor : MonoBehaviour
        {
            /// <summary>
            /// <b>isOpen</b> represents the state of the door.
            /// By default, the door is closed.
            /// </summary>
            public bool isOpen;
            private bool rotating;
            private Transform _doorTransform;
            private GameObject _doorChild;
        
            /// <summary>
            /// <b> Color </b> is the color of the object 
            /// </summary>
            [StateVariable("isOpen", ECARules4AllType.Boolean)] 
            
            private void Awake()
            {
                isOpen = false;
                /*_doorChild = gameObject.transform.GetChild(0).gameObject;
                _doorTransform = _doorChild.transform;*/
                
            }
            
            /// <summary>
            /// <b>opens</b> opens the door.
            ///  </summary>
            [Action(typeof(ECADoor), "opens")]
            public void OpenDoor()
            {
                isOpen = true;
                //Deactivate component HingeJoint
                HingeJoint hingeJoint = gameObject.GetComponent<HingeJoint>();
                Destroy(hingeJoint);
                //Rotate the door with time.deltatime
                StartRotation(gameObject, new Vector3(0, 50, 0));

            }
            
            /// <summary>
            /// <b>closes</b> closes the door.
            /// </summary>
            
            [Action(typeof(ECADoor), "closes")]
            public void CloseDoor()
            {
                isOpen = false;
                //Deactivate component HingeJoint
                HingeJoint hingeJoint = gameObject.GetComponent<HingeJoint>();
                if(hingeJoint)
                    Destroy(hingeJoint);
                //Rotate the door with time.deltatime
                StartRotation(gameObject, new Vector3(0, -50, 0));

            }
            
            private IEnumerator Rotate( Vector3 angles, float duration, GameObject objectToRotate )
            {
                rotating = true ;
                Quaternion startRotation = objectToRotate.transform.rotation ;
                Quaternion endRotation = Quaternion.Euler( angles ) * startRotation ;
                for( float t = 0 ; t < duration ; t+= Time.deltaTime )
                {
                    objectToRotate.transform.rotation = Quaternion.Lerp( startRotation, endRotation, t / duration ) ;
                    yield return null;
                }
                objectToRotate.transform.rotation = endRotation  ;
                rotating = false;
            }

            public void StartRotation(GameObject objectToRotate, Vector3 rotation)
            {
                if( !rotating )
                    StartCoroutine( Rotate( rotation, 1, objectToRotate ) ) ;
            }
        }
    
}