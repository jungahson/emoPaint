using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ButttonHandler : MonoBehaviour
{
    public Image ButtonImage;

    [SerializeField]
    private bool isButtonPressed = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        isButtonPressed = !isButtonPressed;
        if (isButtonPressed)
            ButtonImage.color = new Color(104f/255,231f/255,110f/255); 
        else
            ButtonImage.color = Color.white;
    }

    public void ButtonOff()
    {
        isButtonPressed = false; 
        ButtonImage.color = Color.white;
    }

}
