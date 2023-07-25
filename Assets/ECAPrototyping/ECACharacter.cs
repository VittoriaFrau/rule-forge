using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// A <b>Character</b> is a type of ECAObject, it represents an animal, a humanoid, a robot or a generic creature.
    /// A Character can be autonomous or controlled by the player.
    /// </summary>
    [DisallowMultipleComponent]
    [ECARules4All("character")]
    [RequireComponent(typeof(ECAObject), typeof(Animator))]

    public class ECACharacter : MonoBehaviour
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
        /// <b>isVisible</b> is a boolean that indicates if the object is visible.
        /// If the object is invisible, it will not be rendered but it will still collide with other objects.
        /// </summary>
        [StateVariable("visible", ECARules4AllType.Boolean)] 
        public ECABoolean isVisible = new ECABoolean(ECABoolean.BoolType.YES);
        

        private void Awake()
        {
            gameRenderer = this.gameObject.GetComponents<Renderer>();
            color = gameRenderer[0].material.color;
            UpdateVisibility();
        }
        

        /// <summary>
        /// <b>Shows</b> makes the object visible. It makes it visible if it is not already.
        /// </summary>
        [Action(typeof(ECAObject), "shows")]
        public void Shows()
        {
            isVisible.Assign(ECABoolean.BoolType.YES);
            UpdateVisibility();
        }
        
        /// <summary>
        /// <b>Hides</b> makes the object invisible. It makes it invisible if it is not already.
        /// </summary>
        [Action(typeof(ECAObject), "hides")]
        public void Hides()
        {
            isVisible.Assign(ECABoolean.BoolType.NO);
            UpdateVisibility();
        }
        
        
        
        /// <summary>
        /// <b>ShowsHides</b> sets the visibility of the object, defined by a parameter.
        /// </summary>
        /// <param name="yesNo">The new visibility state for the object. </param>
        [Action(typeof(ECAObject), "changes", "visible", "to", typeof(YesNo))]
        public void ShowsHides(ECABoolean yesNo)
        {
            isVisible = yesNo;
            UpdateVisibility();
        }
        
        /// <summary>
        /// <b>SetsColor</b> sets the color of the light source to the given value.
        /// </summary>
        /// <param name="inputColor">The new color value.</param>
        [Action(typeof(ECAObject), "changes","color", "to", typeof(ECAColor))]
        public void ChangesColor(ECAColor inputColor)
        {
            color = inputColor.Color;
            gameRenderer[0].material.color = color;
        }
        
        [Action(typeof(ECAObject), "waves","hand", "to", typeof(YesNo))]
        public void WaveHand(Animator animator)
        {
            animator.SetBool("isWaving", true);
        }
        
        [Action(typeof(ECAObject), "dance", typeof(YesNo))]
        public void Dancing(Animator animator)
        {
            animator.SetBool("isDancing", true);
        }
        
        [Action(typeof(ECAObject), "get dressed", typeof(YesNo))]
        public void getDressed()
        {
            
        }


        private void UpdateVisibility()
        {
            this.gameObject.SetActive(isVisible);
        }
        
        
        public void ChangeColor(string newColor)
        {
            //convert string to color
            ColorUtility.TryParseHtmlString(newColor, out color);
            gameRenderer[0].material.color = color;
        }
        
    }
}