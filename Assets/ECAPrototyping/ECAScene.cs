using System;
using System.Collections;
using System.Security.Cryptography;
using ECAPrototyping.Utils;
using UnityEngine;

namespace ECAPrototyping.RuleEngine
{
    /// <summary>
    /// A <b>Scene</b> is a type of ECAObject.
    /// </summary>
    [DisallowMultipleComponent]
    [ECARules4All("Scene")]
    [RequireComponent(typeof(ECAObject))]

    public class ECAScene : MonoBehaviour
    {
        
    }
}