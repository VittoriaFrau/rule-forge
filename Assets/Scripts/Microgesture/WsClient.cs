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
        private static Texture2D[] images = new Texture2D[3];
        private static List<ECAEvent> microgestureEvents = new List<ECAEvent>();

        public static List<ECAEvent> MicrogestureEvents
        {
            get => microgestureEvents;
            set => microgestureEvents = value;
        }

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
            images[0] = Utils.LoadPNG("Assets/Resources/Icons/Modalities/middle.png");
            images[1] = Utils.LoadPNG("Assets/Resources/Icons/Modalities/tip.png");
            images[2] = Utils.LoadPNG("Assets/Resources/Icons/Modalities/base.png");
        }


        public static void StartSocket()
        {
            serverOpen = true;
            MicrogestureEvents.Clear();

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
                    if (data.Contact.Contains("middle"))
                        image = images[0];
                    else if (data.Contact.Contains("tip"))
                        image = images[1];
                    //TODO ask Laurence a new base image 
                    else image = images[2];
                    
                    microgestureEvents.Add(new ECAEvent(null, InteractionCreationController.Modalities.Microgesture, data.Contact, image));
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