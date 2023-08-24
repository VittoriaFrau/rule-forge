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
    
    public class ECAFloor :MonoBehaviour
    {
        /// <summary>
        /// <b>Renderer</b> is the material of the plane.
        /// </summary>
        new Renderer renderer;

        private void Awake()
        {
            renderer = gameObject.GetComponent<Renderer>();
        }
        
        /// <summary>
        /// <b>PlaneMaterial</b> sets the material of the plane, defined by a parameter.
        /// </summary>
        /// <param name="material">The new material to set for the skybox. </param>
        [Action(typeof(ECAFloor), "changes", "floor", "to", typeof(string))]
        public void PlaneMaterial(String material)
        {
            switch (material)
            {
              case "Grass":
                  renderer.material = Resources.Load<Material>("Grass_planeTexture/Materials/Stylize_Grass");
                  Debug.Log(renderer.material.name);
                  break;
              case "Rocks":
                  renderer.material = Resources.Load<Material>("Rocks_planeTexture/Hand Painted Rocks Road (Blocky)");
                  Debug.Log(renderer.material.name);
                  break;
              case "Wood":
                  renderer.material = Resources.Load<Material>("Wood_planeTexture/Materials/Planks/Planks");
                  Debug.Log(renderer.material.name);
                  break;
            }
        }
        
    }
}