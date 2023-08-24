using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using Microsoft.MixedReality.Toolkit.Editor;
using Microsoft.MixedReality.Toolkit.UX;
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
        public ECABoolean isPlaying = new ECABoolean(ECABoolean.BoolType.OFF);
        

        
        private void Awake()
        {
            gameRenderer = this.gameObject.GetComponents<Renderer>();
            color = gameRenderer[0].material.color;
        }
/*
        private void Update()
        {
            if (GameObject.Find("Volume_Slider"))
            {
                Volume();
            }
        }*/

        /// <summary>
        /// <b>TurnON</b> turns on the audio source of the selected object. 
        /// </summary>
        [Action(typeof(ECAMusic), "TurnON")]
        public void TurnON()
        {
            Debug.Log("TURN ON");
            //isPlaying.Assign(ECABoolean.BoolType.TRUE);
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.enabled = true;
            audioSource.Play();
        }
        
        /// <summary>
        /// <b>TurnOFF</b> turns off the audio source of the selected object. 
        /// </summary>
        [Action(typeof(ECAMusic), "TurnOFF")]
        public void TurnOFF()
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.enabled = true;
            audioSource.Pause();
        }

        /// <summary>
        /// <b>Volume</b> . 
        /// </summary>
        [Action(typeof(ECAMusic), "volume")]
        public void Volume()
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            Slider volume_slider = GameObject.Find("Volume_Slider").GetComponent<Slider>();
            audioSource.volume = volume_slider.Value;
        }
        

    }
}