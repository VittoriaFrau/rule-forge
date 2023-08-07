using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// <b>ECASkybox</b> is the class for the skyboxes.
    /// </summary>
    [DisallowMultipleComponent]
    [ECARules4All("skybox")]
    [RequireComponent(typeof(ECAScene))]
    
    public class ECASkybox : MonoBehaviour
    {
        /// <summary>
        /// <b>Material</b> is the material of the skybox.
        /// </summary>
        private Material _material;

        private void Awake()
        {
            _material = RenderSettings.skybox;
            
        }
        
        // <summary>
        /// <b>SkyboxMaterial</b> sets the material of the skybox, defined by a parameter.
        /// </summary>
        /// <param name="material">The new material to set for the skybox. </param>
        [Action(typeof(ECAObject), "changes", "skybox", "to", typeof(Material))]
        public void SkyboxMaterial(Material material)
        {
           RenderSettings.skybox = material;
        }
        
    }
}