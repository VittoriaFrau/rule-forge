using System;
using UnityEngine;
using ECARules4All.RuleEngine;
using Action = ECARules4All.RuleEngine.Action;
using Behaviour = ECARules4All.RuleEngine.Behaviour;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

/// <summary>
/// <b>Interactable</b> is a <see cref="Behaviour">Behaviour</see> that can be attached to an object in order to make it
/// interactable with the player collison. If the action is not player initiated, then refer to <see cref="Trigger"/>
/// </summary>
[ECARules4All("interactable")]
[RequireComponent(typeof(Behaviour))]
[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
//TODO: update to mrtk3
public class Interactable : MonoBehaviour //, IMixedRealityPointerHandler
{
    //private LogManager logManager;

    /*private void Start()
    {
        logManager = GameObject.Find("Player").GetComponent<LogManager>();
    }*/

    /*
    private void OnTriggerEnter(Collider other)
    {
        //PROVE
        EventBus.GetInstance().Publish(new Action(other.gameObject, "interacts with", this.gameObject));
        
    }

    private void OnCollisionEnter(Collision other)
    {
        EventBus.GetInstance().Publish(new Action(other.gameObject, "interacts with", this.gameObject));
    }
    
    private void OnTriggerExit(Collider other)
    {
        //PROVE
        EventBus.GetInstance().Publish(new Action(other.gameObject, "stops-interacting with", this.gameObject));
        
    }

    private void OnCollisionExit(Collision other)
    {
        EventBus.GetInstance().Publish(new Action(other.gameObject, "stops-interacting with", this.gameObject));
    }
    */


    /*public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        GameObject player = GameObject.FindObjectOfType<InputManager>().gameObject;
        EventBus.GetInstance().Publish(new Action(player, "interacts with", this.gameObject));

        logManager.WriteOnLog("Player:interacts with:GameObject->" + this.gameObject.name);
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        GameObject player = GameObject.FindObjectOfType<InputManager>().gameObject;
        EventBus.GetInstance().Publish(new Action(player, "stops-interacting with", this.gameObject));

        logManager.WriteOnLog("Player:stops-interacting with:GameObject->" + this.gameObject.name);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    { 
    }*/
}