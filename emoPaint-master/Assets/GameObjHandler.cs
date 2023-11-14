using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DilmerGames
{
    public class GameObjHandler : MonoBehaviour
    {
        public bool enabled = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activator(int idx) 
        {
            gameObject.SetActive(true);
        }
    }
}