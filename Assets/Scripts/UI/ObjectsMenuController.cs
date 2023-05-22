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
    
    public List<GameObject> categoryButtons;
    private GeneralUIController generalUIController;

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
        var transform1 = mainCamera.transform;
        Instantiate(Utils.GetPrefabFromString(type, shapePrefabs), transform1.position + transform1.forward*3, 
            transform1.rotation).transform.parent = interactables.transform;
            //Instantiate(Utils.GetPrefabFromString(type, shapePrefabs), 
            
    }

    public void NewCharacter(string characterString)
    {
        var transform1 = mainCamera.transform;
        Instantiate(Utils.GetPrefabFromString(characterString, animalPrefabs), transform1.position + transform1.forward * 3, 
            transform1.rotation * Quaternion.Euler (0f, 180f, 0f)).transform.parent = interactables.transform;
    }

    public void NewFood(string food)
    {
        
        var transform1 = mainCamera.transform;
        Instantiate(Utils.GetPrefabFromString(food, foodPrefabs),transform1.position + transform1.forward * 3, Quaternion.identity)
            .transform.parent = interactables.transform;

    }

    public void NewVegetation(string veg)
    {
        var transform1 = mainCamera.transform;
        Instantiate(Utils.GetPrefabFromString(veg, environmentPrefabs),transform1.position + transform1.forward * 3, Quaternion.identity)
            .transform.parent = interactables.transform;
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
