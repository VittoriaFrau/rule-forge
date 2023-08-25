using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using Microsoft.MixedReality.Toolkit.UX;
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
        private Light directionalLight;
        [StateVariable("intensity", ECARules4AllType.Float)] 
        private float _intensity = 1f;
        private Slider light_slider;
       
       
        private void Awake()
        {
            directionalLight = gameObject.GetComponent<Light>();
        }
        
        [Action(typeof(ECALight), "changes", "intensity", "to", typeof(float))]
        public void UpdateIntensity(float intensity)
        {
            _intensity = intensity;
            directionalLight.intensity = intensity;
        }
         
    }
}