using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsMenuController : MonoBehaviour
{
    public GameObject objectsMenu;

    public GameObject interactables; //Parent of all interactable gameobjects in the scene
    
    //Getting the prefab of the object to instantiate
    public List<GameObject> shapePrefabs;
    public List<GameObject> animalPrefabs;
    public List<GameObject> foodPrefabs;
    public List<GameObject> environmentPrefabs;
    public List<GameObject> musicPrefabs;
    
    public List<GameObject> categoryButtons;
    private GeneralUIController generalUIController;
    [Header("Skybox")]
    public Material daySkybox;
    public Material nightSkybox;
    public Material sunsetSkybox;

    [Header("Floor")] 
    public Material grassFloor;
    public Material rocksFloor; 
    public Material woodFLoor;
    
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        objectsMenu.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        generalUIController = this.gameObject.GetComponent<GeneralUIController>();
    }
 

    public void NewShape(string type)
    {
        Utils.InstantiateObject(type, shapePrefabs, mainCamera, interactables.transform);
    }

    public void NewCharacter(string characterString)
    {
        GameObject character = Utils.InstantiateObject(characterString, animalPrefabs, mainCamera, interactables.transform);
        character.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void NewFood(string food)
    {
        Utils.InstantiateObject(food, foodPrefabs, mainCamera, interactables.transform);
    }

    public void NewVegetation(string veg)
    {
        Utils.InstantiateObject(veg, environmentPrefabs, mainCamera, interactables.transform);
    }

    public void NewSkybox(string skybox)
    {
        //RenderSettings.skybox = Resources.Load<Material>(path);
        switch (skybox)
        {
            case "Day":
                RenderSettings.skybox = daySkybox;
                break;
            case "Sunset":
                RenderSettings.skybox = sunsetSkybox;
                break;
            case "Night":
                RenderSettings.skybox = nightSkybox;
                break;
        }
    }

    public void NewPlane(string plane)
    {
        Renderer planeRenderer = GameObject.FindGameObjectWithTag("Plane").GetComponent<Renderer>();
        switch (plane)
        {
            case "Grass":
                planeRenderer.material = grassFloor;
                break;
            case "Rocks":
                planeRenderer.material = rocksFloor;
                break;
            case "Wood":
                planeRenderer.material = woodFLoor;
                break;
        }
    }
    
    //TODO: Gador, usa la stessa funzione di NewShape
    public void NewMusic(string musicString)
    {
        var transform1 = mainCamera.transform;
        var go = Instantiate(Utils.GetPrefabFromString(musicString, musicPrefabs),
            transform1.position + transform1.forward * 2,
            transform1.rotation * Quaternion.Euler(0f, 180f, 0f));
        go.transform.parent = interactables.transform;
        
        //go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    
    public void DeActivateObjectsMenu()
    {
        foreach (var button in categoryButtons)
        {
            button.SetActive(false);
        }
        objectsMenu.SetActive(false);
    }

    public void ActivateObjectsMenu()
    {
        objectsMenu.SetActive(true);
        generalUIController.NewObjectState();
    }
}
