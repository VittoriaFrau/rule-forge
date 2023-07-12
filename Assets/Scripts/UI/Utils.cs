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
            
            //Obtain plate position
            Vector3 platePosition = cubePlate.transform.position;
            //Obtain area limits
            Vector3 minPosition = platePosition  / 2f;
            Vector3 maxPosition = platePosition / 2f;
            
            //Generate cubes
            foreach (var e in filteredEvents)
            {
                //Random position inside the plate
                Vector3 randomPosition = new Vector3(
                    Random.Range(minPosition.x, maxPosition.x),
                    Random.Range(minPosition.y, maxPosition.y),
                    Random.Range(minPosition.z, maxPosition.z)
                );
                
                GameObject cube = Object.Instantiate(cubePrefab, randomPosition, Quaternion.Euler(0f,0f,0f), cubePlate.transform);

                cube.transform.localScale = new Vector3(25, 25, 25);
                //TODO check why the rotation is always wrong
                cube.transform.rotation = Quaternion.Euler(0f,0f,0f);

                Material material = new Material(Shader.Find("Standard"));
                material.mainTexture = e.Texture;
                Renderer renderer = cube.GetComponent<Renderer>();
                renderer.material = material;
                material.mainTextureScale = new Vector2(0.5f, 0.5f);
                material.mainTextureOffset = new Vector2(0.25f, 0.25f);

                TextMeshPro text = cube.transform.Find("Image").gameObject.transform.Find("Text").GetComponent<TextMeshPro>();
                text.text = e.ToString();
            }
        }


        public static void GenerateTextFromCubePosition(GameObject textLabel, string previousString, string cubeDescription)
        {
            if(previousString == "WHEN" || previousString == "THEN")
                textLabel.GetComponent<TextMeshProUGUI>().text = previousString +" " + cubeDescription;
            else
                textLabel.GetComponent<TextMeshProUGUI>().text = previousString + " AND " + cubeDescription;
            
        }
    }
}