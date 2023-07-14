using UnityEngine;

namespace UI
{
    public class ECAEvent
    {
        private GameObject _gameObject;
        private InteractionCreationController.Modalities modality;
        private string _event;
        //TODO: sto selezionando cubo, shape o un qualsiasi oggetto?
        private InteractionCreationController.CategoryObjectSelected typeOfObject;
        private Texture2D _texture;
        private string _subject, _verb, _object;
        
        public ECAEvent(GameObject gameObject, InteractionCreationController.Modalities modality, string _event, 
            Texture2D screenshot)
        {
            this._gameObject = gameObject;
            this.modality = modality;
            this._event = _event;
            typeOfObject = InteractionCreationController.CategoryObjectSelected.GameObject; //By default
            _texture = screenshot;
            SetRule();
        }
        
        public ECAEvent(GameObject gameObject, InteractionCreationController.Modalities modality, string _event)
        {
            this._gameObject = gameObject;
            this.modality = modality;
            this._event = _event;
            typeOfObject = InteractionCreationController.CategoryObjectSelected.GameObject; //By default
            _texture = null;
            SetRule();
        }


        public override bool Equals(object obj)
        {
            //Two ECAEvents are the same when they have the same gameObject, modality and event
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return (_gameObject == ((ECAEvent)obj).GameObject && modality == ((ECAEvent)obj).modality && _event == ((ECAEvent)obj)._event);
        }

        public override string ToString()
        {
            return "The user " + modality + " the " + _gameObject.name + " object";
        }

        private void SetRule()
        {
            _subject = "user";
            _verb = modality.ToString();
            _object = _gameObject.name;
        }
        
        public string Subject
        {
            get => _subject;
            set => _subject = value;
        }
        
        public string Verb
        {
            get => _verb;
            set => _verb = value;
        }
        
        public string Object
        {
            get => _object;
            set => _object = value;
        }
        
        public GameObject GameObject
        {
            get => _gameObject;
            set => _gameObject = value;
        }
        
        public Texture2D Texture
        {
            get => _texture;
            set => _texture = value;
        }

    }
}