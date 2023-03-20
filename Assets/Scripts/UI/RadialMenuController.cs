using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RadialMenuController: MonoBehaviour
    {
        Image _image;
        
        void Start()
        {
            _image = GetComponent<Image>();
            _image.alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}