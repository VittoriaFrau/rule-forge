using System.Collections.Generic;
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

        /*/*
         * Function to repositioning canvas because the PiUI component transform the canvas Idk why
         #1#
        public static void RepositionCanvas(GameObject canvas)
        {
            canvas.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            canvas.transform.localPosition = new Vector3(0, 1.6f, 0.3f);
        }

        public static void RepositionPiMenu(GameObject piMenu)
        {
            piMenu.transform.localPosition = new Vector3(104, 40f, 0f);
            piMenu.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }*/

        
    }
}