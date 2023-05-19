using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Color originalColor;
    public Color hoverColor; 
    public Image image; 
    void Start()
    {
        originalColor = image.GetComponent<Image>().color;
        Debug.Log("Normal color impostato");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.GetComponent<Image>().color = hoverColor;
        Debug.Log("Hover Color");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.GetComponent<Image>().color = originalColor;
        Debug.Log("Normal Color back");
    }
    
}
