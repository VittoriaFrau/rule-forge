using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// <b>ECALight</b> is the class for the lights.
    /// </summary>
    [DisallowMultipleComponent]
    [ECARules4All("light")]
    [RequireComponent(typeof(ECAScene))]
    
    public class ECALight : MonoBehaviour
    {
        public Light directionalLight;
        private float _intensity;
        
        private void Awake()
        {
            _intensity = directionalLight.intensity;
        }

        [Action(typeof(ECAObject), "add", "intensity", "to", typeof(float))]
        public void AddIntensity()
        {
            
        }
        public void UpdateIntensity(float intensity)
        {
            //TODO
        }
        
    }
}