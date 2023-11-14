using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DilmerGames.Events;
using System.Collections;
using UnityEngine.UI;
using DilmerGames;


public class ClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//public class ClickHandler : MonoBehaviour
{
    public UnityEvent upEvent;
    public UnityEvent downEvent;

    public GameObject blueButton;  

    public void OnPointerDown(PointerEventData eventData){
    //public void OnMouseDown()
    //{
        Debug.Log("Down");
        blueButton.GetComponent<Image>().color = new Color(255,0,0);

        downEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData){
    //public void OnMouseUp()
    //{
        Debug.Log("up");
        blueButton.GetComponent<Image>().color = new Color(0,156,209);

        upEvent?.Invoke();
    }
}
