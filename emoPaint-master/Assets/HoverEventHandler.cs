using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class HoverEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Text text; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.enabled = true;
        text.enabled = true; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.enabled = false;
        text.enabled = false;
    }
}
