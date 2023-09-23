using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// <b>ECAObject</b> is the base class for all objects that can be used in the rule engine.
    /// All the other classes in this package inherit from this class or one of its subclasses.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ECAObject))]
    [ECARules4All("effect")]
    public class ECAEffect : MonoBehaviour
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

        private ParticleSystem[] effect;
        private void Awake()
        {
            gameRenderer = this.gameObject.GetComponents<Renderer>();
            color = gameRenderer[0].material.color;
            effect = gameObject.GetComponents<ParticleSystem>();
        }
        
        [Action(typeof(ECAEffect), "changes intensity", typeof(float))]
        public void UpdateIntensity(float intensity)
        {
            foreach (var particleSystem in effect)
            {
                var main = particleSystem.main;
                main.startSize = intensity;
            }
            
        }
    }
}