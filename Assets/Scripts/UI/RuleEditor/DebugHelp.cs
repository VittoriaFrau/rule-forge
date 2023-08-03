using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.RuleEditor
{
    public class DebugHelp:MonoBehaviour
    {
        public GameObject ruleEditorPlate, barriers, radialMenu, obj1, cheese;
        private InteractionCreationController _interactionCreationController;
        private GameObject ruleCubePrefab, cubePlate;
        private List<ECAEvent> _events = new();
        /*
        private ScreenshotCamera _screenshotCamera;
        */
        
        [SerializeField]
        private bool deectivateRadialMenu = true;

        // Metodo chiamato quando il valore dell'attributo cambia nell'editor di Unity
        private void OnValidate()
        {
            // Controlla se il GameObject deve essere attivato o disattivato
            radialMenu.SetActive(deectivateRadialMenu);
        }
        private void Start()
        {
            ruleEditorPlate.SetActive(true);


            barriers.SetActive(false);

            _interactionCreationController = GetComponent<InteractionCreationController>();
            
            ruleCubePrefab = _interactionCreationController.ruleCubePrefab;
            cubePlate = _interactionCreationController.cubePlate;
            
            Texture2D texture2D = Texture2D.blackTexture;
            ECAEvent ecaEvent1 = new ECAEvent(obj1, InteractionCreationController.Modalities.Headgaze, "Hover Entered", texture2D);
            _events.Add(ecaEvent1);
            
            
            Texture2D textureSecondCube = Texture2D.redTexture;
            ECAEvent ecaEvent2 = new ECAEvent(cheese, InteractionCreationController.Modalities.Touch, "Click", textureSecondCube);
            _events.Add(ecaEvent2);

            Utils.GenerateCubesFromEventList(_events, ruleCubePrefab, cubePlate);
            
            ruleEditorPlate.transform.localPosition = new Vector3(ruleEditorPlate.transform.localPosition.x,
                ruleEditorPlate.transform.localPosition.y, 350);
        }
    }
}