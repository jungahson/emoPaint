using UnityEngine;
using UnityEngine.UI;
using DilmerGames.Events;

namespace DilmerGames
{
    public class DropDownHandlerEmotion : MonoBehaviour
    {
        [SerializeField]
        private VRDropdownChanged dropdownChanged;

        void Start()
        {
            //var selectEmotion = transform.GetComponent<Dropdown>();

            // assigned on value changed event 
            /*selectEmotion.onValueChanged.AddListener(delegate
            {
                selectEmotionValueChangeHappened(selectEmotion);
            });*/

            var dropdown = transform.GetComponent<Dropdown>();
            // dropdown.options.Clear(); 

            dropdown.onValueChanged.AddListener(delegate { selectEmotionValueChangeHappened(dropdown); });
        }

        void selectEmotionValueChangeHappened(Dropdown dropdown) 
        {
            int index = dropdown.value;
            string text = dropdown.options[index].text;
            if (text == "Happy")
            {
                dropdownChanged?.Invoke(0);
            }
            else if (text == "Sad")
            {
                //VRStats.Instance.firstText.text = $"Sad button";
                dropdownChanged?.Invoke(1);
            }
            else if (text == "Disgust")
            {
                //VRStats.Instance.firstText.text = $"Disgust button";
                dropdownChanged?.Invoke(2);
            }
            else if (text == "Fear")
            {
                dropdownChanged?.Invoke(3);
            }
            else if (text == "Surprise")
            {
                dropdownChanged?.Invoke(4);
            }
            else if (text == "Anger")
            {
                dropdownChanged?.Invoke(5);
            }
            else
            {
                dropdownChanged?.Invoke(6);
            }
        }
    }
}