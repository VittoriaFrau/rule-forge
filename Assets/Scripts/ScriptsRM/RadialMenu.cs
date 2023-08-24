using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using UI;
using UnityEngine;
using UnityEngine.UI;


public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pressableButtons;

    public List<GameObject> PressableButtons
    {
        get => pressableButtons;
        set => pressableButtons = value;
    }

    public Image highlight;

    [Header("Slice Data")]
    [SerializeField]
    List<Image> slices;
    [SerializeField]
    public float radius = 4.0f;

    public float startingAngle = 0.0f;

    [SerializeField]
    [HideInInspector]
    private float[] angleList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        //Scalo la dimensione degli spicchi 
        foreach (Image spicchi in slices)
        {
            if (spicchi.transform.localScale.x > 2 || spicchi.transform.localScale.y > 2 || spicchi.transform.localScale.z > 2)
            {
                spicchi.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
            }
        }
    }
    */

    public void Open()
    {
        slices = new List<Image>();
        foreach (GameObject button in pressableButtons)
        {
            button.gameObject.SetActive(true);
        }
        Rearrange();
        //InstanceHighlight();
    }

    public void Close()
    {
        foreach (Image spicchi in slices)
        {
            spicchi.gameObject.SetActive(false);
            if (spicchi == null)
            {
                slices.Clear();
                break;
            }
            DestroyImmediate(spicchi.gameObject);
        }
        slices.Clear();


    }

    public void Rearrange()
    {
        int numberOfObjects = pressableButtons.Count;
        float angleStep = 360f / numberOfObjects;
        float angle = startingAngle;
        //Da attivare, in caso di utilizzo dell'Highlight
        /*
        if (numberOfObjects == 1) angle = startingAngle;
         
        else if (numberOfObjects % 2 == 0)
        {
            angle += 90 / (numberOfObjects / 2);
            if(numberOfObjects == 2 || numberOfObjects == 6)
            {
                angle = startingAngle;
            }
        }
        else
        {
            angle += 90 / numberOfObjects;
            if(numberOfObjects == 5)
            {
                angle += 180 / numberOfObjects;
            }
        }*/

        for (int i = 0; i < numberOfObjects; i++)
        {
            float xPos = transform.position.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            float yPos = transform.position.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

            pressableButtons[i].transform.position = new Vector3(xPos, yPos, transform.position.z);

            angle += angleStep;
        }
    }

    public void InstanceHighlight()
    {
        
        highlight.fillAmount = 1.0f / pressableButtons.Count; //calcolo la grandezza dello spicchio 
        
        int sliceCount = pressableButtons.Count;
        float lastRot = 0;
        angleList = new float[sliceCount];
        for (int i = 0; i < sliceCount; i++)
        {
            Image currentSlice = Instantiate(highlight);
            Image currentImage = currentSlice.GetComponent<Image>();
            
            float fillPercentage = (1f / sliceCount);
            float angle = fillPercentage * 360;
            currentImage.fillAmount = fillPercentage;
            angle = (angle + 360) % 360;
           
            if (angle == 0)
            {
                angle = 360;
            }
            
            currentSlice.transform.SetParent(transform);
            int rot = Mathf.Clamp((int)(angle + lastRot), 0, 360);
            
            if (rot == 360)
            {
                rot = 0;
            }
            currentSlice.transform.rotation = Quaternion.Euler(0, 0, rot);
            lastRot += angle;
            angleList[i] = rot;
            
            slices.Add(currentSlice);
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z + 0.01f;
            slices[i].transform.position = new Vector3(x, y, z);
            
            slices[i].gameObject.SetActive(true);
            
        }

        

    }

    
    public void getListButtons(List<GameObject> buttons, GameObject recordButton = null)
    {
        //TODO ripristinare insieme agli spicchi
        Close();
        pressableButtons = buttons;
        if (recordButton != null)
        {
            pressableButtons.Add(recordButton);
        }
        Open();
        
    }

    public void AddSingleButtonToList(GameObject button)
    {
        Close();
        pressableButtons.Add(button);
        Open();
    }
    
}
