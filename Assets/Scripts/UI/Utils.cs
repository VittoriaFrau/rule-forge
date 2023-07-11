using System.Collections.Generic;
using System.IO;
using ECAPrototyping.Utils;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Utils
    {
        
        public static GameObject GetPrefabFromString(string s, List<GameObject> prefabs)
        {
            foreach (var prefab in prefabs)
            {
                if (prefab.name == s)
                {
                    return prefab;
                }
            }

            return null;
        }

        public static Color GetColorFromString(string s)
        {
            if (ECAColor.colorDict.ContainsKey(s))
            {
                return ECAColor.colorDict[s];
            }

            return Color.white;
        }

        
        //"providerType":"glove","providerId":42,"timestamp":1687168716976,"name":"tap","actuator":"thumb","contact":"index tip","raw":{}}}}
        public static MicrogestureData convertJsonToMicrogestureData(string json)
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
        
        public static Texture2D LoadPNG(string filePath) {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath)) 	{
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            return tex;
        }

        //Remove duplicate events and events with null texture
        public static List<ECAEvent> RemoveDuplicates(List<ECAEvent> events)
        {
            List<ECAEvent> filteredEvents = new List<ECAEvent>();
            foreach (var e in events)
            {
                if (!filteredEvents.Contains(e) && e.Texture != null)
                {
                    filteredEvents.Add(e);
                }
            }

            return filteredEvents;
        }

        public static void GenerateCubesFromEventList(List<ECAEvent> events, GameObject cubePrefab, GameObject cubePlate)
        {
            //Filter events 
            List<ECAEvent> filteredEvents = RemoveDuplicates(events);
            
            //Generate cubes
            foreach (var e in filteredEvents)
            {
                GameObject cube = Object.Instantiate(cubePrefab, cubePlate.transform, true);
                /*cube.transform.position = new Vector3(0, 0, 0);
                cube.transform.localScale = new Vector3(1, 1, 1);*/
                
                cube.transform.localScale = new Vector3(25, 25, 25);
                
                //Change FaceFront image looking for a child named FaceFront
                SpriteRenderer faceFront = cube.transform.Find("FaceFront").GetComponent<SpriteRenderer>();
                Sprite NewSprite = Sprite.Create(e.Texture, new Rect(0, 0, e.Texture.width, e.Texture.height), new Vector2(0, 0), 100.0f);

                faceFront.sprite = NewSprite;
                
                //TODO faceleft & faceright
                
                //TODO change text
                TextMeshProUGUI text = cube.transform.Find("Image").gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                text.text = e.ToString();
            }
        }
    }
}