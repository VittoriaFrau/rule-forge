using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


namespace UI
{
    public class WsClient
    {
        static WebSocket  ws;
        private static bool isRecording = false;

        public static bool IsRecording
        {
            get => isRecording;
            set => isRecording = value;
        }
       

        public static void StartSocket(List<ECAEvent> _events, GeneralUIController generalUIController)
        {
            
            ws = new WebSocket("ws://localhost:9000");
            ws.Connect();
           
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
                MicrogestureData data = Utils.convertJsonToMicrogestureData(e.Data);
                
                if(isRecording){}
                    _events.Add(new ECAEvent(null, InteractionCreationController.Modalities.Touch, data.Contact));
                
                //TODO understand why generalUIController is null
                generalUIController.SetDebugText("Microgesture: " + data.Contact);    
            };
        }
        
        public static void StopSocket()
        {
            ws.Close();
        }
        
    }
}