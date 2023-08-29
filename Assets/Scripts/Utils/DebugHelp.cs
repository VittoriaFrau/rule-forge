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
        private RuleManager _ruleManager;
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
            _ruleManager = gameObject.GetComponent<RuleManager>();
            
            _ruleManager.InitializeVariables();
            
            //Modality events
            ECAEvent ecaEvent1 = new ECAEvent(obj1, InteractionCreationController.Modalities.Headgaze, "Hover Entered", Texture2D.blackTexture);
            _modalityEvents.Add(ecaEvent1);

            ECAEvent ecaEvent2 = new ECAEvent(cheese, InteractionCreationController.Modalities.Touch, "Click", Texture2D.redTexture);
            _modalityEvents.Add(ecaEvent2);
            
            //Action events
            
            /*ECAEvent ecaEvent3 = new ECAEvent(obj1, "changes color to", "blue");
            ecaEvent3.Texture = Texture2D.grayTexture;
            _actionEvents.Add(ecaEvent3);

            ECAEvent ecaEvent4 = new ECAEvent(cheese, "hides");
            ecaEvent4.Texture = Texture2D.whiteTexture;
            _actionEvents.Add(ecaEvent4);*/
            
            //Microgesture events
            /*ECAEvent ecaEvent5 = new ECAEvent(null, InteractionCreationController.Modalities.Microgesture,
                "middle tip", Utils.LoadPNG("Assets/Resources/Icons/Modalities/middle.png"));
            _modalityEvents.Add(ecaEvent5);
*/
            Utils.GenerateCubesFromEventList(_modalityEvents, _actionEvents, modalityRuleCubePrefab, 
                actionRuleCubePrefab, actionRuleCubePrefabVariant, cubePlate);
            
            ruleEditorPlate.transform.localPosition = new Vector3(ruleEditorPlate.transform.localPosition.x,
                ruleEditorPlate.transform.localPosition.y, 350);
        }
    }
}