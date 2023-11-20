using System;
using System.Collections.Generic;
using System.Linq;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using UnityEngine;

namespace UI.RuleEditor
{
    public class DebugHelp:MonoBehaviour
    {
        public GameObject ruleEditorPlate, barriers, radialMenu, cube, cheese;
        private InteractionCreationController _interactionCreationController;
        private GameObject modalityRuleCubePrefab, cubePlate, actionRuleCubePrefabVariant, actionRuleCubePrefab;
        private RuleManager _ruleManager;
        
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
            /*ECAEvent ecaEvent1 = new ECAEvent(null, InteractionCreationController.Modalities.Speech, "leviosa", Utils.LoadPNG("Assets/Resources/Icons/microphone.png"));
            _interactionCreationController.ModalityEvents.Add(ecaEvent1);

            ECAEvent ecaEvent2 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Laser, "points", Texture2D.redTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent2);
            
            ECAEvent ecaEvent3 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Headgaze, "points", Texture2D.grayTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent3);
            
            ECAEvent ecaEvent4 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Touch, "touch", Texture2D.whiteTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent4);*/
            
            ECAEvent ecaEvent1 = new ECAEvent(GameObject.Find("Cube"), InteractionCreationController.Modalities.Laser, "leviosa", Utils.LoadPNG("Assets/Resources/Icons/microphone.png"));
            _interactionCreationController.ModalityEvents.Add(ecaEvent1);

            ECAEvent ecaEvent2 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Laser, "points", Texture2D.redTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent2);
            
            ECAEvent ecaEvent3 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Headgaze, "points", Texture2D.grayTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent3);
            
            ECAEvent ecaEvent4 = new ECAEvent(GameObject.Find("feather"), InteractionCreationController.Modalities.Touch, "touch", Texture2D.whiteTexture);
            _interactionCreationController.ModalityEvents.Add(ecaEvent4);
            
            
            ruleEditorPlate.transform.localPosition = new Vector3(ruleEditorPlate.transform.localPosition.x,
                ruleEditorPlate.transform.localPosition.y, 80);
            
            //Action events
            
            ECAEvent actionEvent1 = new ECAEvent(GameObject.Find("Canvas"), "changes color to", "blue");
            actionEvent1.Texture = Texture2D.grayTexture;
            _interactionCreationController.ActionEvents.Add(actionEvent1);

            ECAEvent actionEvent2 = new ECAEvent(GameObject.Find("Canvas"), "hides");
            actionEvent2.Texture = Texture2D.redTexture;
            _interactionCreationController.ActionEvents.Add(actionEvent2);
            
            ECAEvent actionEvent3 = new ECAEvent(GameObject.Find("Canvas"), "gravityON");
            actionEvent3.Texture = Texture2D.blackTexture;
            _interactionCreationController.ActionEvents.Add(actionEvent3);
            
            ECAEvent actionEvent4 = new ECAEvent(GameObject.Find("Canvas"), "shows");
            actionEvent4.Texture = Texture2D.normalTexture;
            _interactionCreationController.ActionEvents.Add(actionEvent4);
            
            Utils.GenerateCubesFromEventList(_interactionCreationController.ModalityEvents, _interactionCreationController.ActionEvents, modalityRuleCubePrefab, 
                actionRuleCubePrefab, actionRuleCubePrefabVariant, cubePlate);
            
            //Microgesture events
            /*ECAEvent ecaEvent5 = new ECAEvent(null, InteractionCreationController.Modalities.Microgesture,
                "middle tip", Utils.LoadPNG("Assets/Resources/Icons/Modalities/middle.png"));
            _modalityEvents.Add(ecaEvent5);
            
            Utils.GenerateCubesFromEventList(_modalityEvents, _actionEvents, modalityRuleCubePrefab, 
                actionRuleCubePrefab, actionRuleCubePrefabVariant, cubePlate);
            
            ruleEditorPlate.transform.localPosition = new Vector3(ruleEditorPlate.transform.localPosition.x,
                ruleEditorPlate.transform.localPosition.y, 350);*/
        }
    }
}