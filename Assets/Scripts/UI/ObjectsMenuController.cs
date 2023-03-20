using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsMenuController : MonoBehaviour
{
    public GameObject objectsMenu;

    public GameObject interactables; //Parent of all interactable gameobjects in the scene
    
    //Getting the prefab of the object to instantiate
    public GameObject cubePrefab, spherePrefab, capsulePrefab, cylinderPrefab;
    public GameObject kidPrefab, foxPrefab, zombiePrefab;
    public GameObject bananaPrefab, hamburgerPrefab, cheesePrefab, watermelonPrefab;
    public GameObject treePrefab, mushroomPrefab, signPrefab, rockPrefab;
    public List<GameObject> shapePrefabs = new();
    public List<GameObject> animalPrefabs = new ();
    public List<GameObject> foodPrefabs = new ();
    public List<GameObject> environmentPrefabs = new ();

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        objectsMenu.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shapePrefabs = new List<GameObject>(){cubePrefab, spherePrefab, capsulePrefab, cylinderPrefab};
        animalPrefabs = new List<GameObject>(){kidPrefab, foxPrefab, zombiePrefab};
        foodPrefabs = new List<GameObject>(){bananaPrefab, hamburgerPrefab, cheesePrefab, watermelonPrefab};
        environmentPrefabs = new List<GameObject>(){treePrefab, mushroomPrefab, signPrefab, rockPrefab};
        
    }
 

    public void NewShape(string type)
    {
        var transform1 = mainCamera.transform;
        Instantiate(Utils.GetPrefabFromString(type, shapePrefabs), transform1.position + transform1.forward*3, 
            transform1.rotation).transform.parent = interactables.transform;
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
}
