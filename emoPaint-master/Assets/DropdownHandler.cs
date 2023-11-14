using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DilmerGames.Events;
using DilmerGames.Core.Singletons;
using DilmerGames.Enums;

namespace DilmerGames
{
    public class DropdownHandler : MonoBehaviour
    {
        [SerializeField]
        private VRDropdownChanged dropdownChanged;

        // Start is called before the first frame update
        void Start()
        {
            var dropdown = transform.GetComponent<Dropdown>();
            // dropdown.options.Clear(); 

            dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
        }

        void DropdownItemSelected(Dropdown dropdown)
        {
            int index = dropdown.value;
            string text = dropdown.options[index].text; 
            if (text == "T1")
            {
                dropdownChanged?.Invoke(0);
            }
            else if (text == "T2")
            {
                dropdownChanged?.Invoke(1);
            }
            else if (text == "T3")
            {
                dropdownChanged?.Invoke(2);
            }
            else if (text == "T4")
            {
                dropdownChanged?.Invoke(3);
            }
            else if (text == "T5")
            {
                dropdownChanged?.Invoke(4);
            }
            else if (text == "T6")
            {
                dropdownChanged?.Invoke(5);
            }
            else if (text == "T7")
            {
                dropdownChanged?.Invoke(6);
            }
            else if (text == "T8")
            {
                dropdownChanged?.Invoke(7);
            }
            else if (text == "T9")
            {
                dropdownChanged?.Invoke(8);
            }
            else if (text == "T10")
            {
                dropdownChanged?.Invoke(9);
            }
            else if (text == "T11")
            {
                dropdownChanged?.Invoke(10);
            }
            else if (text == "T12")
            {
                dropdownChanged?.Invoke(11);
            }
            else if (text == "T13")
            {
                dropdownChanged?.Invoke(12);
            }
            else if (text == "T14")
            {
                dropdownChanged?.Invoke(13);
            }
            else if (text == "T15")
            {
                dropdownChanged?.Invoke(14);
            }
            else if (text == "T16")
            {
                dropdownChanged?.Invoke(15);
            }
            else if (text == "T17")
            {
                dropdownChanged?.Invoke(16);
            }
            else if (text == "T18")
            {
                dropdownChanged?.Invoke(17);
            }
        }

    }
}
