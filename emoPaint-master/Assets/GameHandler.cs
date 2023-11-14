using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DilmerGames
{
    public class GameHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Button_onClick()
        {
            ScreenShot.TakeScreenshot_Static(500, 500); 
        }
    }
}
