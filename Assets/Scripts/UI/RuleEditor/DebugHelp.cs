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
        private GameObject modalityRuleCubePrefab, cubePlate, actionRuleCubePrefabVariant, actionRuleCubePrefab;
        private List<ECAEvent> _modalityEvents = new();
        private List<ECAEvent> _actionEvents = new();
        
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
            
            modalityRuleCubePrefab = _interactionCreationController.modalityRuleCubePrefab;
            actionRuleCubePrefabVariant = _interactionCreationController.actionRuleCubePrefabVariant;
            actionRuleCubePrefab = _interactionCreationController.actionRuleCubePrefab;
            cubePlate = _interactionCreationController.cubePlate;
            
            //Modality events
            ECAEvent ecaEvent1 = new ECAEvent(obj1, InteractionCreationController.Modalities.Headgaze, "Hover Entered", Texture2D.blackTexture);
            _modalityEvents.Add(ecaEvent1);

            ECAEvent ecaEvent2 = new ECAEvent(cheese, InteractionCreationController.Modalities.Touch, "Click", Texture2D.redTexture);
            _modalityEvents.Add(ecaEvent2);
            
            //Action events
            
            ECAEvent ecaEvent3 = new ECAEvent(obj1, "change color", "blue", Texture2D.grayTexture);
            _actionEvents.Add(ecaEvent3);

            ECAEvent ecaEvent4 = new ECAEvent(cheese, "Hides", Texture2D.whiteTexture);
            _actionEvents.Add(ecaEvent4);

            Utils.GenerateCubesFromEventList(_modalityEvents, _actionEvents, modalityRuleCubePrefab, 
                actionRuleCubePrefab, actionRuleCubePrefabVariant, cubePlate);
            
            ruleEditorPlate.transform.localPosition = new Vector3(ruleEditorPlate.transform.localPosition.x,
                ruleEditorPlate.transform.localPosition.y, 350);
        }
    }
}