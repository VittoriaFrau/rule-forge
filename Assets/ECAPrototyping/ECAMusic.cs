using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// A <b>musicDevice</b> is a type of ECAObject, it represents a piano, a guitar or a vintage radio.
    /// It can be controlled by the player.
    /// </summary>
    [DisallowMultipleComponent]
    [ECARules4All("musicDevice")]
    [RequireComponent(typeof(ECAObject))]

    public class ECAMusic : MonoBehaviour
    {
        /// <summary>
        /// <b>GameRender</b> is the renderer of the object.
        /// </summary>
        private Renderer[] gameRenderer;
        
        /// <summary>
        /// <b> Color </b> is the color of the object 
        /// </summary>
        [StateVariable("color", ECARules4AllType.Color)] 
        public Color color;
        
        /// <summary>
        /// <b>isPlaying</b> is a boolean that indicates if the object is reproducing music.
        /// </summary>
        [StateVariable("mode", ECARules4AllType.Boolean)] 
        public ECABoolean isPlaying = new ECABoolean(ECABoolean.BoolType.ON);
        

        private void Awake()
        {
            gameRenderer = this.gameObject.GetComponents<Renderer>();
            color = gameRenderer[0].material.color;
            UpdateMode();
            UpdateVolume();
        }
        

        /// <summary>
        /// <b>ModeON</b> turns on the music. 
        /// </summary>
        [Action(typeof(ECAObject), "turnOn")]
        public void TurnON()
        {
            isPlaying.Assign(ECABoolean.BoolType.ON);
            UpdateMode();
        }
        
        /// <summary>
        /// <b>ModeOFF</b> turns off the music. 
        /// </summary>
        [Action(typeof(ECAObject), "turnOff")]
        public void TurnOFF()
        {
            isPlaying.Assign(ECABoolean.BoolType.OFF);
            UpdateMode();
        }
        
        /// <summary>
        /// <b>Volume</b> . 
        /// </summary>
        [Action(typeof(ECAObject), "volume")]
        public void Volume()
        {
            //TODO
            UpdateVolume();
        }


        private void UpdateMode()
        {
            this.gameObject.SetActive(isPlaying);
        }

        private void UpdateVolume()
        {
            //TODO
        }

    }
}