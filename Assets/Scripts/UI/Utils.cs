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

            float previousZ = 0.63f;
            //Generate cubes
            foreach (var e in filteredEvents)
            {
                //Adjust cube transform
                float zPosition;
                if (filteredEvents.IndexOf(e) == 6)
                    previousZ = 0.63f;
                zPosition=previousZ - 0.13f;
                previousZ = zPosition;
                float xPosition = filteredEvents.IndexOf(e) < 6 ? -0.37f : -0.25f; //One or more rows
                Vector3 position = new Vector3(xPosition, -0.19f, zPosition);
                GameObject cube = Object.Instantiate(cubePrefab, position, Quaternion.Euler(0f,0f,0f), cubePlate.transform);
                cube.transform.rotation = Quaternion.identity;
                cube.transform.localScale = new Vector3(25, 25, 25);
                
                //Material using screenshot
                Material material = new Material(Shader.Find("Standard"));
                material.mainTexture = e.Texture;
                Renderer renderer = cube.GetComponent<Renderer>();
                renderer.material = material;
                material.mainTextureScale = new Vector2(0.5f, 0.5f);
                material.mainTextureOffset = new Vector2(0.25f, 0.25f);
                
                FillTextLabelsInCube(e, cube);

            }
        }

        public static void GenerateTextFromCubePosition(GameObject textLabel, string previousString, string cubeDescription)
        {
            if(previousString == "WHEN" || previousString == "THEN")
                textLabel.GetComponent<TextMeshProUGUI>().text = previousString +" " + cubeDescription;
            else
                textLabel.GetComponent<TextMeshProUGUI>().text = previousString + " AND " + cubeDescription;
        }

        public static void FillTextLabelsInCube(ECAEvent e, GameObject cube)
        {
            //Front face
            GameObject frontFace = cube.transform.Find("FrontFaceRule").gameObject;
            TextMeshProUGUI subjectFront = frontFace.transform.Find("Subject").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            subjectFront.text = e.Subject;
                
            TextMeshProUGUI verbFront = frontFace.transform.Find("Verb").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            verbFront.text = e.Verb;
                
            TextMeshProUGUI objectFront = frontFace.transform.Find("Object").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            objectFront.text = e.Object;
                
            //Top face 
            GameObject topFace = cube.transform.Find("TopFaceRule").gameObject;
            TextMeshProUGUI subjectTop = topFace.transform.Find("Subject").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            subjectTop.text = e.Subject;
                
            TextMeshProUGUI verbTop = topFace.transform.Find("Verb").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            verbTop.text = e.Verb;
                
            TextMeshProUGUI objectTop = topFace.transform.Find("Object").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            objectTop.text = e.Object;
        }
    }
}