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
        private float _intensity;
        private Slider light_slider;
        //Light light;
        private void Awake()
        {
            directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            _intensity = directionalLight.intensity;
        }
        

        public void Update()
        {
            if (GameObject.Find("LightSlider"))
            {
                UpdateIntensity();
            }

        }

        [Action(typeof(ECALight), "change brightness")]
        public void UpdateIntensity()
        {
            light_slider = GameObject.Find("Light_Slider").GetComponent<Slider>();
            directionalLight.intensity = light_slider.Value;
        }
         
    }
}