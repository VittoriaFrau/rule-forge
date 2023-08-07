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
    [ECARules4All("floor")]
    [RequireComponent(typeof(ECAScene))]
    
    public class ECAFloor
    {
        /// <summary>
        /// <b>Renderer</b> is the material of the plane.
        /// </summary>
        private Renderer renderer;

        private void Awake()
        {
            renderer = GameObject.FindGameObjectWithTag("Plane").GetComponent<Renderer>();
            
        }
        
        /// <summary>
        /// <b>PlaneMaterial</b> sets the material of the plane, defined by a parameter.
        /// </summary>
        /// <param name="material">The new material to set for the skybox. </param>
        [Action(typeof(ECAObject), "changes", "floor", "to", typeof(Material))]
        public void PlaneMaterial(Material material)
        {
            renderer.material = material;
        }
        
    }
}