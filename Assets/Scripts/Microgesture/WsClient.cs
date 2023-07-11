using System;
using System.Collections.Generic;
using ECAPrototyping.RuleEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;


namespace UI
{
    public class WsClient : MonoBehaviour
    {
        static WebSocket  ws;
        private static bool isRecording = false;
        private static GameObject debugText;
        private static string canvasStatusUpdate;
        TextMeshProUGUI textMeshPro;
        private static bool serverOpen = false;

        public static bool IsRecording
        {
            get => isRecording;
            set => isRecording = value;
        }

        private void Start()
        {
            canvasStatusUpdate = "";
            debugText = GameObject.FindGameObjectWithTag("debugText");
            textMeshPro = debugText.GetComponent<TextMeshProUGUI>();
        }


        public static void StartSocket(List<ECAEvent> _events)
        {
            serverOpen = true;
            
            ws = new WebSocket("ws://localhost:9000");
            ws.Connect();
            
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
                MicrogestureData data = Utils.convertJsonToMicrogestureData(e.Data);

                if (isRecording)
                {
                    Texture2D image = null;
                    //TODO : check when glove if fixed
                    if(data.Contact.Contains("middle"))
                        image = Utils.LoadPNG("Assets/Resources/Icons/Modalities/middle.jpeg");
                    else if(data.Contact.Contains("tip"))
                        image = Utils.LoadPNG("Assets/Resources/Icons/Modalities/tip.jpeg");
                    //TODO ask Laurence a new base image 
                    else image = Utils.LoadPNG("Assets/Resources/Icons/Modalities/base.jpeg");
                        _events.Add(new ECAEvent(null, InteractionCreationController.Modalities.Microgesture, data.Contact, image));
                }
                
                canvasStatusUpdate = "Microgesture: " + data.Contact;
                    
                
            };
        }

        private void FixedUpdate()
        {
            if (serverOpen && canvasStatusUpdate!="")
            {
                textMeshPro.text =canvasStatusUpdate;
                canvasStatusUpdate = "";
            }
        }

        public static void StopSocket()
        {
            serverOpen = false;
            ws.Close();
        }
        
    }
}