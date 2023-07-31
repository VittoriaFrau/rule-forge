using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using TMPro;
using UI;
using UnityEngine;

public class RepositionRulePlate : MonoBehaviour
{
    public enum PlateState
    {
        Default, Moving
    }
    
    private PlateState _plateState;
    public TextMeshProUGUI debugPlateText;
    public ObjectManipulator rulePlateManipulator;
    private string previousDebugText;
    private BoundsControl _boundsController;
    
    // Start is called before the first frame update
    void Start()
    {
        _plateState = PlateState.Default;
        if(debugPlateText==null) debugPlateText = GameObject.FindGameObjectsWithTag("RuleUtils")
            .ToList().Find(x=>x.name=="RuleDebugText").GetComponent<TextMeshProUGUI>();
        GameObject ruleEditorPlate = GameObject.FindGameObjectsWithTag("RuleUtils")
            .ToList().Find(x=>x.name=="RuleEditorPlate");
        _boundsController = ruleEditorPlate.GetComponent<BoundsControl>();
        if(rulePlateManipulator==null) rulePlateManipulator = ruleEditorPlate.GetComponent<ObjectManipulator>();
        
    }

    public void MovePlate()
    {
        if (_plateState == PlateState.Default)
        {
            rulePlateManipulator.enabled = true;
            _boundsController.enabled = true;
            _plateState = PlateState.Moving;
            previousDebugText = debugPlateText.text;
            debugPlateText.text = "Moving the plate, click again the moving button to stop moving.";
        }
        else
        {
            StopMoving();
        }
    }

    public void StopMoving()
    {
        if(_plateState == PlateState.Moving)
        {
            rulePlateManipulator.enabled = false;
            _boundsController.enabled = false;
            _plateState = PlateState.Default;
            debugPlateText.text = previousDebugText;
        }
       
    }
}
