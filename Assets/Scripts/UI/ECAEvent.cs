using UnityEngine;

namespace UI
{
    public class ECAEvent
    {
        private GameObject _gameObject;
        private InteractionCreationController.Modalities modality;
        private string _event;
        //TODO: sto selezionando cubo, shape o un qualsiasi oggetto?
        private string typeOfObject;
        
        public ECAEvent(GameObject gameObject, InteractionCreationController.Modalities modality, string _event)
        {
            this._gameObject = gameObject;
            this.modality = modality;
            this._event = _event;
        }
        
        public GameObject GameObject
        {
            get => _gameObject;
            set => _gameObject = value;
        }
        
        
    }
}