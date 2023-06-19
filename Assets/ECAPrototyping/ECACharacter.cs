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
        [StateVariable("rotation", ECARules4AllType.Rotation)] 
        public Rotation rotation;
        
        
        [Action(typeof(ECAObject), "life")]
        public void Life()
        {
            Debug.Log("life function");
        }
    }
}