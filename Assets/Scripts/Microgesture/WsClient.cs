using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;


namespace UI
{
    public class WsClient: MonoBehaviour
    {
        WebSocket ws;
        public bool isRecording = false;
        
        public void StartSocket(List<ECAEvent> _events)
        {
            ws = new WebSocket("ws://localhost:9000");
            ws.Connect();
           
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
                MicrogestureData data = convertJsonToMicrogestureData(e.Data);
                Debug.Log("Microgesture data: "+data);
                if(isRecording)
                    _events.Add(new ECAEvent(null, InteractionCreationController.Modalities.Touch, data.Contact));
            };
        }
        
        public void StopSocket()
        {
            ws.Close();
        }
        
        //"providerType":"glove","providerId":42,"timestamp":1687168716976,"name":"tap","actuator":"thumb","contact":"index tip","raw":{}}}}
        public MicrogestureData convertJsonToMicrogestureData(string json)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(json);
            string providerType = jsonObject.microgesture.content.providerType;
            int providerId = jsonObject.microgesture.content.providerId;
            long timestamp = jsonObject.microgesture.content.timestamp;
            string name = jsonObject.microgesture.content.name;
            string actuator = jsonObject.microgesture.content.actuator;
            string contact = jsonObject.microgesture.content.contact;
            
            MicrogestureData data = new MicrogestureData(providerType, providerId, timestamp, name, actuator, contact);
            return data;
        }
    }
}