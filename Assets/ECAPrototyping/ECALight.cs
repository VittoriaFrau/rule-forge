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
        
        public void UpdateIntensity(float intensity)
        {
            //TODO
        }
        
    }
}