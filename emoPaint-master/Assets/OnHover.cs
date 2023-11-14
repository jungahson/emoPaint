using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DilmerGames.Events
{

    /*(public class OnHover : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }*/

    //[RequireComponent(typeof(Image))]
    public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Image image;

        [SerializeField]
        private VRValidColorGradient validColorChanged;
        void Start()
    {
        //image = GetComponent<Image>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
            validColorChanged?.Invoke(true); 
        }

    public void OnPointerExit(PointerEventData eventData)
    {
            validColorChanged?.Invoke(false);
        }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }*/

    /*void OnHoverEnter()
    {
        image.color = Color.gray;
    }

    void OnHoverExit()
    {
        image.color = Color.white;
    }

    void OnClick()
    {
        image.color = Color.blue;
    }*/

}
}


