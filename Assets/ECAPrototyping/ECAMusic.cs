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
        /// <b>isPlaying</b> is a boolean that indicates if the object is reproducing music.
        /// </summary>
        [StateVariable("onOff", ECARules4AllType.Boolean)] 
        public ECABoolean isPlaying = new ECABoolean(ECABoolean.BoolType.YES);
        

        private void Awake()
        {
            gameRenderer = this.gameObject.GetComponents<Renderer>();
            UpdateModality();
        }
        

        /// <summary>
        /// <b>onOff</b> turns on and off the object. 
        /// </summary>
        [Action(typeof(ECAObject), "on/off")]
        public void onOff()
        {
            isPlaying.Assign(ECABoolean.BoolType.YES);
            UpdateModality();
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


        private void UpdateModality()
        {
            this.gameObject.SetActive(isPlaying);
        }

        private void UpdateVolume()
        {
            //TODO
        }

    }
}