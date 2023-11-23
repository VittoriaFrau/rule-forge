using System;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
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
    public float radius = 0.067f;

    public float radius2 = 0.08f;

    public float startingAngle = 0.0f;

    [SerializeField]
    [HideInInspector]
    private float[] angleList;

    public GameObject recordButton;
    
    

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
        float xPos, yPos;
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

        Vector3 center = transform.position;
        float zPosition = center.z;
        for(int i = 0; i < numberOfObjects; i++){
            xPos = center.x + (numberOfObjects > 8 ? radius2 : radius) * Mathf.Cos(angle * Mathf.Deg2Rad);
            yPos = center.y + (numberOfObjects > 8 ? radius2 : radius) * Mathf.Sin(angle * Mathf.Deg2Rad);

            pressableButtons[i].transform.position = new Vector3(xPos, yPos, zPosition);
            /*pressableButtons[i].transform.localPosition = 
                new Vector3(pressableButtons[i].transform.localPosition.x, pressableButtons[i].transform.localPosition.y, 0.0f);*/
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

    
    public void getListButtons(List<GameObject> buttons, Boolean isRecording = false, Boolean addRecordButton = false)
    {
        //TODO ripristinare insieme agli spicchi
        Close();
        
        //Remove duplicates from buttons gameobjects with hashset
        HashSet<GameObject> setSenzaDuplicati = new HashSet<GameObject>(buttons);
        // Converti nuovamente il set in una lista se necessario
        List<GameObject> listaSenzaDuplicati = new List<GameObject>(setSenzaDuplicati);
        
        pressableButtons = listaSenzaDuplicati;
        if (recordButton != null && !pressableButtons.Contains(recordButton) && !isRecording && addRecordButton)
        {
            pressableButtons.Add(recordButton);
        }

        {
        if (isRecording)
            pressableButtons.Remove(recordButton);
        }
        
        Open();
    }

    public void AddSingleButtonToList(GameObject button)
    {
        if(pressableButtons.Contains(button)) return;
        Close();
        pressableButtons.Add(button);
        Open();
       
    }

    public void RemoveSingleButtonToList(GameObject button)
    {
        Close();
        button.SetActive(false);
        pressableButtons.Remove(button);
        Open();
    }
    
    
}
