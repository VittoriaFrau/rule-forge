using System.Collections.Generic;
using ECAPrototyping.Utils;
using UnityEngine;

namespace UI
{
    public class Utils
    {
        public static GameObject GetPrefabFromString(string s, List<GameObject> prefabs)
        {
            foreach (var prefab in prefabs)
            {
                if (prefab.name == s)
                {
                    return prefab;
                }
            }

            return null;
        }

        public static Color GetColorFromString(string s)
        {
            if (ECAColor.colorDict.ContainsKey(s))
            {
                return ECAColor.colorDict[s];
            }

            return Color.white;
        }
        
        
        

        
    }
}