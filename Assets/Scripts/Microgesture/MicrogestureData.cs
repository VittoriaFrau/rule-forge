using UnityEngine;

namespace UI
{
    public class MicrogestureData
    {
        private string providerType;
        private int providerId;
        private long timestamp;
        private string name;
        private string actuator;
        private string contact;
        
        public string Contact
        {
            get => contact;
            set => contact = value;
        }
        
        public MicrogestureData(string providerType, int providerId, long timestamp, string name, string actuator, string contact)
        {
            this.providerType = providerType;
            this.providerId = providerId;
            this.timestamp = timestamp;
            this.name = name;
            this.actuator = actuator;
            this.contact = contact;
        }
        
        
    }
}

