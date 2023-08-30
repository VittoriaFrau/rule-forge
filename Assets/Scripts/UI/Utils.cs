using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ECAPrototyping.RuleEngine;
using ECAPrototyping.Utils;
using Microsoft.MixedReality.Toolkit.UX;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UI;
using Action = ECAPrototyping.RuleEngine.Action;
using Object = UnityEngine.Object;

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

        /*public static Dictionary<GameObject, Vector3> GenerateCubesFromEventList(List<ECAEvent> _modalityEvents, List<ECAEvent>
            _actionEvents, GameObject cubePrefab, GameObject prefabVariant, GameObject cubePlate)
        {
            Dictionary<GameObject, Vector3> result = new Dictionary<GameObject, Vector3>();
            //Filter events 
            List<ECAEvent> filteredModalityEvents = RemoveDuplicates(_modalityEvents);

            float previousZ = 1.41f;
            //Generate cubes
            foreach (var e in filteredModalityEvents)
            {
                //Adjust cube transform
                Vector3 position = CalculatePositionInPlate(previousZ, filteredModalityEvents.IndexOf(e));
                previousZ = position.z;

                GameObject cube = InstantiateRuleCube(cubePrefab, 1, position, cubePlate.transform, new Texture[]{e.Texture});
                result.Add(cube, position);
                FillTextLabelsInCube(e, cube);
            }

            List<ECAEvent> filteredActionsEvents = RemoveDuplicates(_actionEvents);
            
            foreach (var e in filteredActionsEvents)
            {
                //Adjust cube transform
                Vector3 position = CalculatePositionInPlate(previousZ, filteredActionsEvents.IndexOf(e));
                previousZ = position.z;
                GameObject cube;
                if (e.Object != "")
                {
                    cube = InstantiateRuleCube(cubePrefab, 1, position, cubePlate.transform, 
                        new Texture[]{e.Texture});
                }
                else
                {
                    cube = InstantiateRuleCube(prefabVariant, 1, position, cubePlate.transform, 
                        new Texture[]{e.Texture});
                }

                cube.tag = "ActionRuleCube";
                
                result.Add(cube, position);
                FillTextLabelsInCube(e, cube);
            }
            
            return result;
        }*/
        
        public static Dictionary<GameObject, Vector3> GenerateCubesFromEventList(List<ECAEvent> _modalityEvents, 
            List<ECAEvent> _actionEvents, GameObject modalityCubePrefab, GameObject actionCubePrefab, 
            GameObject actionCubePrefabVariant, GameObject cubePlate)
        {
            Dictionary<GameObject, Vector3> result = new Dictionary<GameObject, Vector3>();

            // Combine the two lists
            List<ECAEvent> allEvents = new List<ECAEvent>();
            allEvents.AddRange(RemoveDuplicates(_modalityEvents));
            allEvents.AddRange(RemoveDuplicates(_actionEvents));

            // Generate cubes for all events
            float previousZ = 1.41f;
            foreach (var e in allEvents)
            {
                Vector3 position = CalculatePositionInPlate(previousZ, allEvents.IndexOf(e));
                previousZ = position.z;
                GameObject cube;

                bool isModality = e.Event != null;

                if (isModality)
                {
                    cube = InstantiateRuleCube(modalityCubePrefab, 1, 
                        position, cubePlate.transform, new Texture[] { e.Texture });
                }
                else cube = InstantiateRuleCube(e.Object == null ? actionCubePrefabVariant : actionCubePrefab, 1, 
                    position, cubePlate.transform, new Texture[] { e.Texture });

                result.Add(cube, position);
                FillTextLabelsInCube(e, cube);
            }

            return result;
        }

        public static GameObject InstantiateObject(string prefabType, List<GameObject> prefabList, Camera mainCamera, Transform interactableTransform)
        {
            var transform1 = mainCamera.transform;
            var go = Object.Instantiate(GetPrefabFromString(prefabType, prefabList),
                transform1.position + transform1.forward * 3,
                Quaternion.identity);
    
            go.transform.parent = interactableTransform;
    
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            
            rigidbody.useGravity = true;
            
            go.name = prefabType;

            ECAObject ecaObject = go.GetComponent<ECAObject>();
            ecaObject.isUsingGravity = ECABoolean.YES;
            
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            return go;
        }

        public static ECAEvent GetEventFromCube(GameObject cube)
        {
            ECAEvent e = new ECAEvent(cube);
            
            GameObject frontFace = cube.transform.Find("FrontFaceRule").gameObject;
            TextMeshProUGUI subjectFront = frontFace.transform.Find("Subject").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            e.Subject = subjectFront.text;
                
            TextMeshProUGUI verbFront = frontFace.transform.Find("Verb").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            string[] verbAndEvent = verbFront.text.Split(' ');
            if (verbAndEvent.Length == 1)
            {
                verbAndEvent = verbFront.text.Split('\n');
            }
            e.Verb = verbAndEvent[0];
            e.Event = verbAndEvent[1];
                
            TextMeshProUGUI objectFront = frontFace.transform.Find("Object").transform.Find("Image").GetComponent<TextMeshProUGUI>();
            e.Object = objectFront.text;
            
            e.Texture = (Texture2D)cube.GetComponent<Renderer>().material.mainTexture;

            return e;
        }

        /*
         * Instantiate a cube with a rule description
         * @params: cubeLevel: 1, 2, 3 is the number of joint cubes
         */
        public static GameObject InstantiateRuleCube(GameObject cubePrefab, int cubeLevel, Vector3 position, Transform parent, Texture[] texture )
        {
            GameObject cube = Object.Instantiate(cubePrefab, position, Quaternion.Euler(0f,0f,0f), parent);
            cube.transform.rotation = Quaternion.identity;
            cube.transform.localScale = new Vector3(25, 25, 25);
                
            //Material using screenshot
            
            if (cubeLevel < 2)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.mainTexture = texture[0];
                Renderer renderer = cube.GetComponent<Renderer>();
                renderer.material = material;
                material.mainTextureScale = new Vector2(0.5f, 0.5f);
                material.mainTextureOffset = new Vector2(0.25f, 0.25f);
            }
            else
            {
                
                // Check if the texture array contains valid textures
                if (texture == null || texture.Length < 2 || texture[0] == null || texture[1] == null)
                {
                    Debug.LogError("Invalid texture array or missing textures!");
                    return cube;
                }
                
                
                GameObject cubeLeft = cube.transform.Find("CubeLeft").gameObject;
                GameObject cubeRight = cube.transform.Find("CubeRight").gameObject;

                Material materialLeft = new Material(Shader.Find("Standard"));
                materialLeft.mainTexture = texture[0];
                Renderer rendererLeft = cubeLeft.GetComponent<Renderer>();
                rendererLeft.material = materialLeft;
                materialLeft.mainTextureScale = new Vector2(0.5f, 0.5f);
                materialLeft.mainTextureOffset = new Vector2(0.25f, 0.25f);

                Material materialRight = new Material(Shader.Find("Standard"));
                materialRight.mainTexture = texture[1]; // Fix the assignment to materialRight
                Renderer rendererRight = cubeRight.GetComponent<Renderer>();
                rendererRight.material = materialRight;
                materialRight.mainTextureScale = new Vector2(0.5f, 0.5f);
                materialRight.mainTextureOffset = new Vector2(0.25f, 0.25f);

            }

            return cube;
        }
        

        public static Vector3 CalculatePositionInPlate(float previousZ, int eventIndex)
        {
            float zPosition;
            if (eventIndex == 6)
                previousZ = 1.41f;
            zPosition=previousZ - 0.13f;
            
            float xPosition = eventIndex < 6 ? -0.37f : -0.25f; //One or more rows
            Vector3 position = new Vector3(xPosition, -0.19f, zPosition);
            return position;
        }

        public static void GenerateTextFromCubePosition(TextMeshProUGUI textLabel, string cubeDescription, string logicalOperator)
        {
            string previousString = textLabel.text;
            //Remove the new line
            string formattedText = cubeDescription.Replace("\n", " "); 
            if(previousString == "..." || previousString=="") //if it's the first cube
                textLabel.text = formattedText;
            else
                textLabel.text = previousString + " "+ logicalOperator + " " + formattedText;
        }
        
        public static void RemoveTextFromCubePosition(TextMeshProUGUI textLabel, string cubeDescription, string locution)
        {
            string input = textLabel.text;

            /*int index = input.IndexOf(locutionToRemove, StringComparison.OrdinalIgnoreCase);

            if (index != -1)
            {
                // Trova l'indice della locuzione da eliminare
                int phraseIndex = input.IndexOf(cubeDescription, StringComparison.OrdinalIgnoreCase);

                if (phraseIndex != -1)
                {
                    if (phraseIndex < index)
                    {
                        // Se la locuzione da eliminare è presente prima della locuzione da rimuovere,
                        // rimuoviamo entrambe le locuzioni dalla stringa di input
                        input = input.Remove(phraseIndex, cubeDescription.Length).TrimStart(' ');
                        input = input.Remove(0, index + locutionToRemove.Length).TrimStart(' ');
                        textLabel.text = input;
                    }
                    else
                    {
                        // Se la locuzione da rimuovere viene trovata prima della locuzione da eliminare,
                        // rimuoviamo solo la locuzione da rimuovere dalla stringa di input
                        input = input.Remove(index, locutionToRemove.Length).TrimStart(' ');
                        textLabel.text = input;
                    }
                }
            }
            else
            {
                // Se la locuzione da rimuovere non viene trovata, ma la frase da eliminare è presente nella stringa di input,
                // restituiamo una stringa vuota
                int phraseIndex = input.IndexOf(cubeDescription, StringComparison.OrdinalIgnoreCase);
                if (phraseIndex != -1)
                {
                    input = "...";
                    textLabel.text = input;
                }
            }*/
            
            int locutionIndex = input.IndexOf(locution, StringComparison.OrdinalIgnoreCase);
            int phraseIndex = input.IndexOf(cubeDescription, StringComparison.OrdinalIgnoreCase);

            if (phraseIndex != -1)
            {
                if (locutionIndex != -1 && locutionIndex < phraseIndex)
                {
                    // Se la locuzione viene trovata prima della frase da eliminare,
                    // rimuoviamo entrambe dalla stringa di input
                    input = input.Remove(locutionIndex, locution.Length).TrimStart(' ');
                    input = input.Remove(phraseIndex - locution.Length, cubeDescription.Length).TrimStart(' ');
                    textLabel.text = input;
                }
                else
                {
                    // Altrimenti, rimuoviamo solo la frase da eliminare
                    input = input.Remove(phraseIndex, cubeDescription.Length).TrimStart(' ');
                    textLabel.text = input;
                }
            }

            /*int index = previousString.IndexOf(cubeDescription);
            if (index >= 0)
            {
                //Remove the cube description
                previousString = previousString.Remove(index, cubeDescription.Length);
                //Remove the logical operator AND or OR
                int indexOfLogicalOperator = previousString.IndexOf(logicalOperator);
                if (indexOfLogicalOperator >= 0)
                    previousString = previousString.Remove(indexOfLogicalOperator - 1, logicalOperator.Length + 1);
                /*previousString = previousString.Remove(index - 3, logicalOperator.Length + 3);#1#
                textLabel.text = previousString;
            }*/
        }

        public static void FillTextLabelsInCube(ECAEvent e, GameObject cube)
        {
            // Define the face names and text labels
            string[] faceNames = { "FrontFaceRule", "TopFaceRule" };
            string[] labelTexts = { e.Subject, e.Verb + " " + e.Event, e.Object };
            if (e.Event == "stops")
            {
                labelTexts[1] = e.Event + " " + e.Verb;
            }
            if (e.Modality == InteractionCreationController.Modalities.Microgesture)
            {
                labelTexts[1] = e.Verb;
            }

            // Loop through each face and fill the text labels
            foreach (string faceName in faceNames)
            {
                TextMeshProUGUI[] faceLabels = GetTextLabelsInCube(cube, faceName);

                // Fill the text labels with the appropriate text
                for (int i = 0; i < faceLabels.Length; i++)
                {
                    //To read the text better we replace the spaces with new lines
                    string formattedText = labelTexts[i].Replace(" ", "\n"); 
                    faceLabels[i].text =formattedText;
                }
            }
        }

        /*public static void FillTextLabelsInActionCubes(ECAEvent e, GameObject cube)
        {
            // Define the face names and text labels
            string[] faceNames = { "FrontFaceRule", "TopFaceRule" };
            string[] labelTexts = { e.Subject, e.Verb + e.Object };

            if (e.Object.Equals(String.Empty))
            {
                // Loop through each face and fill the text labels
                foreach (string faceName in faceNames)
                {
                    TextMeshProUGUI[] faceLabels = GetTextLabelsInCube(cube, faceName);

                    // Fill the text labels with the appropriate text
                    for (int i = 0; i < faceLabels.Length; i++)
                    {
                        faceLabels[i].text = labelTexts[i];
                    }
                }
            }
            else
            {
                
            }
        }*/

        public static TextMeshProUGUI[] GetTextLabelsInCube(GameObject cube, string face)
        {
            Transform faceTransform = cube.transform.Find(face);
            if (faceTransform == null)
            {
                Debug.LogWarning("Face not found in the cube.");
                return new TextMeshProUGUI[0];
            }

            Transform subjectTransform = faceTransform.Find("Subject/Image");
            Transform verbTransform = faceTransform.Find("Verb/Image");
            Transform objTransform = faceTransform.Find("Object/Image");
            Transform secondVerbTransform = faceTransform.Find("SecondVerb/Image");
            Transform meanwhileTransform = faceTransform.Find("Meanwhile/Image");

            TextMeshProUGUI subject = subjectTransform?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI verb = verbTransform?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI obj = objTransform?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI secondVerbText = secondVerbTransform?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI meanwhileText = meanwhileTransform?.GetComponent<TextMeshProUGUI>();

            if (obj != null && secondVerbText != null && meanwhileText != null)
            {
                return new[] { subject, verb, obj,  meanwhileText, secondVerbText };
            }

            if (obj != null && secondVerbText != null)
            {
                return new[] { subject, verb, obj, secondVerbText };
            }

            if (obj != null)
            {
                return new[] { subject, verb, obj };
            }

            if (secondVerbText != null && meanwhileText != null)
            {
                return new[] { subject, verb, meanwhileText, secondVerbText  };
            }

            return secondVerbText != null ? new[] { subject, verb, secondVerbText } : new[] { subject, verb };
        }

        
        public static void FillTextLabelsInMergedCubes(GameObject newCube, ECAEvent [] events)
        {
            // Define the face names and text labels
            string[] faceNames = { "FrontFaceRule", "TopFaceRule" };
            //string[] labelTexts = { events[0].Subject, events[0].Verb + " " + events[0].Event, events[0].Object, events[1].Verb + " " + events[1].Event, events[1].Object };
            string[] labelTexts = { events[0].Subject,  events[0].Event + " " + events[0].Object, "meanwhile", events[1].Event + " " + events[1].Object };

            // Loop through each face and fill the text labels
            foreach (string faceName in faceNames)
            {
                TextMeshProUGUI[] faceLabels = GetTextLabelsInCube(newCube, faceName);

                // Fill the text labels with the appropriate text
                for (int i = 0; i < faceLabels.Length; i++)
                {
                    faceLabels[i].text = labelTexts[i];
                }
            }
        }

        public static string GetRuleDescriptionFromCubePrefab(GameObject cube)
        {
            string ruleDescription = "";
            //Front face
            TextMeshProUGUI [] frontFaceR = GetTextLabelsInCube(cube, "FrontFaceRule");
            foreach (var t in frontFaceR)
            {
                ruleDescription += t.text + " ";
            }

            return ruleDescription;
        }

        public static void ClearTextDescription(TextMeshProUGUI whenText, TextMeshProUGUI thenText)
        {
            whenText.text = "...";
            thenText.text = "...";
        }

        //Delete all the unnecessary containers
        public static void ResetCubeContainers()
        {
            Transform whenContainer = GameObject.FindGameObjectsWithTag("RuleUtils").FirstOrDefault(x => x.name == "When").transform;
            Transform whenFrontplate = whenContainer.Find("Frontplate");
            GameObject whenSequentialRow = whenFrontplate.Find("SequentialRow").gameObject;
            GameObject whenEquivalenceRow = whenFrontplate.Find("EquivalenceRow").gameObject;
            //Delete all the gameobject that are called "Cube Container (Clone)"
            foreach (Transform child in whenSequentialRow.transform)
            {
                if (child.name.StartsWith("CubeContainer(Clone)")) Object.Destroy(child.gameObject);
            }

            foreach (Transform child in whenEquivalenceRow.transform)
            {
                if (child.name.StartsWith("CubeContainer(Clone)")) Object.Destroy(child.gameObject);
            }
            
            Transform thenContainer = GameObject.FindGameObjectsWithTag("RuleUtils").FirstOrDefault(x => x.name == "Then").transform;
            Transform thenFrontplate = thenContainer.Find("Frontplate");
            GameObject thenSequentialRow = thenFrontplate.Find("SequentialRow").gameObject;
            GameObject thenEquivalenceRow = thenFrontplate.Find("EquivalenceRow").gameObject;
            //Delete all the gameobject that are called "Cube Container (Clone)"
            foreach (Transform child in thenSequentialRow.transform)
            {
                if (child.name.StartsWith("CubeContainer(Clone)")) Object.Destroy(child.gameObject);
            }
            
            foreach (Transform child in thenEquivalenceRow.transform)
            {
                if (child.name.StartsWith("CubeContainer(Clone)")) Object.Destroy(child.gameObject);
            }
        }

        //TODO: capire dove lo metto
        public static ECAEvent GetActionFromButton(GameObject button, GameObject selectedGameobject, Slider volumeSlider, 
            Slider lightSlider, GameObject plane, GameObject _skybox, Light _mainlight)
        {
            //TODO add new Gador actions
            
            Action action = GetActionFromString(button.name, selectedGameobject, volumeSlider, lightSlider, plane, _skybox, _mainlight);
            
            ECAEvent e = ConvertActionToECAEvent(action);
            
            return e;
        }

        //Returns the category of each gameobject. E.g. cube --> shape, cheese --> food
        public static string GetECALastScriptFromECAObject(GameObject gameObject)
        {
            Component [] components = gameObject.GetComponents(typeof(MonoBehaviour));
            
            foreach (var component in components)
            {
                string componentFullName = component.GetType().ToString();
                int lastDotIndex = componentFullName.LastIndexOf('.');
                string componentName = componentFullName.Substring(lastDotIndex + 1);
                if (componentName.Contains("ECA") && !componentName.Equals("ECAObject"))
                {
                    //TODO find the most appropriate category
                    if (componentName.Equals("ECAScene"))
                    {
                        return "Light";
                    }
                    return componentName.Substring(3);
                }
            }

            return "Shape";
        }

        public static string GetIconForECACategory(string category)
        {
            switch (category)
            {
                case "Character":
                    return "Icon 117";
                case "shape":
                case "Shape":
                    return "Icon 133";
                case "Food":
                    return "Icon 54";
                case "Environment":
                    return "Icon 4";
                case "Music":
                    return "Icon 22";
                case "Light":
                    return "Icon 90";
            }
            Debug.LogError("Icon null for category "+ category);
            return null;
        }

        public static ECAEvent ConvertActionToECAEvent(Action action)
        {
            ECAEvent e = new ECAEvent(action.GetSubject(), action.GetActionMethod());
            e.Action = action;
            switch (action.GetActionType())
            {
                case Action.ActionType.INVALID:
                    Debug.Log("Action INVALID");
                    break;
                case Action.ActionType.CHANGES:
                case Action.ActionType.CUSTOMCHANGE:
                    e.Verb = action.GetActionMethod() + " " + action.GetObject() + " " +  action.GetModifier();
                    e.Object = action.GetModifierValue().ToString();
                    break;
                case Action.ActionType.VALUE:
                    e.Verb = action.GetActionMethod();
                    e.Object = action.GetObject().ToString();
                    break;
                case Action.ActionType.VERB:
                    break;
                case Action.ActionType.OBJECT:
                    e.Object = action.GetModifierValue().ToString();
                    break;
            }

            return e;
        }

        public static Action GetActionFromString(string s, GameObject SelectedObject, Slider volumeSlider, 
            Slider lightSlider, GameObject plane, GameObject _skybox, Light _mainlight)
        {
            switch (s)
            {
                case "Show":
                   return new Action(SelectedObject, "shows");
                
                case "Hide":
                    return (new Action(SelectedObject, "hides"));
                
                case "Delete":
                    return (new Action(SelectedObject, "deleted"));
                
                case "GravityON":
                    return (new Action(SelectedObject, "gravityON"));
                
                case "GravityOFF":
                    return (new Action(SelectedObject, "gravityOFF"));
                
                case "red":
                    ECAColor red = new ECAColor("red");
                    return (new Action(SelectedObject, "changes", "color", "to", red));
                
                case "blue":
                    ECAColor blue = new ECAColor("blue");
                    return (new Action(SelectedObject, "changes", "color", "to", blue));
                    
                case "green":
                    ECAColor green = new ECAColor("green");
                    return (new Action(SelectedObject, "changes", "color", "to", green));
                    
                case "purple":
                    ECAColor purple = new ECAColor("purple");
                    return (new Action(SelectedObject, "changes", "color", "to", purple));
                
                case "gray":
                case "grey":
                    ECAColor gray = new ECAColor("gray");
                    return (new Action(SelectedObject, "changes", "color", "to", gray));                    
                    
                case "yellow":
                    ECAColor yellow = new ECAColor("yellow");
                    return (new Action(SelectedObject, "changes", "color", "to", yellow));
                    
                case "cyan":
                    ECAColor cyan = new ECAColor("cyan");
                    return (new Action(SelectedObject, "changes", "color", "to", cyan));                    
                    
                case "white":
                    ECAColor white = new ECAColor("white");
                    return (new Action(SelectedObject, "changes", "color", "to", white));
                    
                case "black":
                    ECAColor black = new ECAColor("black");
                    return (new Action(SelectedObject, "changes", "color", "to", black));
                
                case "WaveHand":
                    return (new Action(SelectedObject, "waves hand"));
                
                case "Dance":
                    return (new Action(SelectedObject, "dance"));
                
                case "TurnOn":
                    return (new Action(SelectedObject, "turns", ECABoolean.ON));
                
                case "TurnOff":
                    return (new Action(SelectedObject, "turns", ECABoolean.OFF));
                
                case "Volume":
                    float volumeValue =  volumeSlider.Value;
                    return (new Action(SelectedObject, "changes", "volume", "to", volumeValue));
                
                case "Brightness":
                    float lightValue =  lightSlider.Value;
                    return (new Action(_mainlight.gameObject, "changes", "intensity", "to", lightValue ));
                    
                case "Floor_Grass":
                    return (new Action(plane, "changes", "floor", "to", "Grass"));
                
                case "Floor_Rocks":
                    return (new Action(plane, "changes", "floor", "to", "Rocks"));
                
                case "Floor_Wood":
                    return (new Action(plane, "changes", "floor", "to", "Wood"));
                
                case "Skybox_Day":
                    return (new Action(_skybox, "changes skybox","Day"));
                
                case "Skybox_Sunset":
                    return (new Action(_skybox, "changes skybox", "Sunset"));
                
                case "Skybox_Night":
                    return (new Action(_skybox, "changes skybox", "Night"));
            }

            return null;
        }
    }
}