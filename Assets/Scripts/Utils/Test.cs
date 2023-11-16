using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int taskNumber =1;

    public TextMeshProUGUI taskLabel;

    public GameObject RulePlate;
    
    private List<Vector3> localPositions = new ();
    private int lastPosition = -1;

    public GameObject feather, door, torch, featherPlate;
    private Vector3 posizioneIniziale;
    private float velocitaFluttuazione = 1.0f;  // Regola la velocità di fluttuazione
    private float ampiezzaFluttuazione = 0.5f;  // Regola l'ampiezza della fluttuazione
    private bool fluttua = false;

   
    // Start is called before the first frame update
    void Start()
    {
        localPositions.Add(new Vector3(-15.0f, 90.0f, -13.61364f));
        localPositions.Add(new Vector3(-15.0f, 30.0f, -13.61364f));
        localPositions.Add(new Vector3(-15.0f, -30.0f, -13.61364f));
        localPositions.Add(new Vector3(-15.0f, -90.0f, -13.61364f));
        UpdateTaskLabel();
        HideGameObjects();
    }

    public void AddMicrogestureTask(List<ECAEvent> _modalityEvents)
    {
        if (taskNumber == 1)
        {
            _modalityEvents.Add(new ECAEvent(null, InteractionCreationController.Modalities.Microgesture, "index-tip", 
                Utils.LoadPNG("Assets/Resources/Icons/Modalities/tip.png")));
        }
    }

    public void HideGameObjects()
    {
        switch (taskNumber)
        {
            case 1:
                torch.SetActive(false);
                feather.SetActive(false);
                featherPlate.SetActive(false);
                break;
            case 2:
            case 3:
                feather.SetActive(false);
                featherPlate.SetActive(false);
                door.SetActive(false);
                break;
            case 4:
                door.SetActive(false);
                torch.SetActive(false);
                break;
        }
    }

    public void UpdateTaskLabel()
    {
        switch (taskNumber)
        {
            case 1:
                taskLabel.text = "When the user performs the index-tip microgesture, Then the door opens";
                break;
            case 2:
                taskLabel.text =
                    "When the user laser points the torch AND THEN says \"Incendio\", THEN the torch turns on";
                break;
            case 3:
                taskLabel.text = "When the user laser points the torch OR says \"Incendio\", THEN the torch turns on";
                break;
            case 4:
                taskLabel.text =
                    "When the user says Leviosa MEANWHILE laser-points the door, THEN the feather gravity is off";
                break;
        }
    }

    /*public void FixedUpdate()
    {
        if (RulePlate.activeSelf)
        {
            foreach (Transform child in RulePlate.transform)
            {
                if (!child.name.Contains("Barrier"))
                {
                    Debug.Log("Name: " + child.name + " world pos: " + child.position + " local pos: " + child.localPosition);
                }
            }
        }
    }*/

    //ho dovuto fare questa cagata per il test, riposizionando il ruleplate le posizioni dei cubi erano tutte sfalsate
    public Vector3 GetFixedPosition()
    {
        lastPosition++;
        return localPositions[lastPosition];
        
    }

    //Metodo per reset delle posizioni di cubi e ruleplate apposito per il test
    public void ResetForTest(Transform rulePlateTransform, List<GameObject> cubes)
    {
        rulePlateTransform.localPosition = new Vector3(-179.0f, 952.0f, 783.0f);
        for (int i=0;i<cubes.Count;i++)
        {
            GameObject cube = cubes[i];
            cube.transform.localPosition = localPositions[i];
        }
    }

    public void StartLeviosa()
    {
        posizioneIniziale = feather.transform.position;
        fluttua = true;

        StartCoroutine(Fluttua());
    }

  
    public void StopLeviosa()
    {
        fluttua = false;
        //Stop invokerepeating
        StopCoroutine(Fluttua());
    }
    
    IEnumerator Fluttua()
    {
        float tempoPassato = 0f;

        while (tempoPassato < 1f)
        {
            // Calcola la fluttuazione usando la funzione sinusoidale
            float fluttuazione = ampiezzaFluttuazione * Mathf.Sin(velocitaFluttuazione * tempoPassato);

            // Aggiorna la posizione verticale dell'oggetto
            feather.transform.position = new Vector3(posizioneIniziale.x, posizioneIniziale.y + fluttuazione, posizioneIniziale.z);

            // Incrementa il tempo passato
            tempoPassato += Time.deltaTime;

            yield return null;
        }

        // Assicurati che l'oggetto sia alla posizione finale
        feather.transform.position = new Vector3(posizioneIniziale.x, posizioneIniziale.y + ampiezzaFluttuazione, posizioneIniziale.z);
    }
    
    /*void FixedUpdate()
    {
        if (fluttua)
        {
            Debug.Log("Fluttuo");
            // Calcola la fluttuazione usando la funzione sinusoidale
            float fluttuazione = Mathf.Abs(ampiezzaFluttuazione * Mathf.Sin(velocitaFluttuazione * Time.time));

            // Aggiorna la posizione verticale dell'oggetto solo se la fluttuazione è positiva
            feather.transform.position = new Vector3(posizioneIniziale.x, posizioneIniziale.y + fluttuazione, posizioneIniziale.z);
        }
        
    }*/
}
