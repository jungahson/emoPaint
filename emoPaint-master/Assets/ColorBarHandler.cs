using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBarHandler : MonoBehaviour
{
    public Image ColorBarImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ColorChanged(Image sender)
    {
        ColorBarImage.color = sender.color; 
    }
}