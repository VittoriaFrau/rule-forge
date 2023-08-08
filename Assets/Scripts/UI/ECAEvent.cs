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

        public ECAEvent(GameObject gameObject, string _verb, string _object)
        {
            _gameObject = gameObject;
            this._verb = _verb;
            this._object = _object;
            this._subject = gameObject.name;
        }
        
        public ECAEvent(GameObject gameObject, string _verb, string _object, Texture2D screenshot)
        {
            _gameObject = gameObject;
            this._verb = _verb;
            this._object = _object;
            this._subject = gameObject.name;
            _texture = screenshot;

        }
        
        public ECAEvent(GameObject gameObject, string _verb, Texture2D screenshot)
        {
            _gameObject = gameObject;
            this._verb = _verb;
            this._subject = gameObject.name;
            _texture = screenshot;

        }
        
        public ECAEvent(GameObject gameObject, InteractionCreationController.Modalities modality, string _event, 
            Texture2D screenshot)
        {
            this._gameObject = gameObject;
            this.modality = modality;
            this._event = _event;
            typeOfObject = InteractionCreationController.CategoryObjectSelected.GameObject; //By default
            _texture = screenshot;
            SetModalityRule();
        }
        
        public ECAEvent(GameObject gameObject, InteractionCreationController.Modalities modality, string _event)
        {
            this._gameObject = gameObject;
            this.modality = modality;
            this._event = _event;
            typeOfObject = InteractionCreationController.CategoryObjectSelected.GameObject; //By default
            _texture = null;
            SetModalityRule();
        }

        public ECAEvent(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public ECAEvent(GameObject gameObject, string verb)
        {
            _gameObject = gameObject;
            _verb = verb;
            _subject = gameObject.name;
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
            if(modality!=InteractionCreationController.Modalities.None)
                return "The user " + modality + " the " + _gameObject.name + " object";
            return _gameObject.name + " " + _verb + " " + _object;
        }

        private void SetModalityRule()
        {
            _subject = "user";
            if (modality == InteractionCreationController.Modalities.Microgesture)
            {
                _verb = "performs";
                _object = _event;
            }
            else
            {
                _verb = modality.ToString();
                _object = _gameObject.name;
            }
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
        
        public string Event
        {
            get => _event;
            set => _event = value;
        }

    }
}