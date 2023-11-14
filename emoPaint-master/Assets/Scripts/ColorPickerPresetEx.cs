using UnityEngine;

namespace DilmerGames
{
    public class ColorPickerPresetsEx : MonoBehaviour
    {
        void Start()
        {
        }

        public void SelectedEmotion(int val)
        {
            VRStats.Instance.firstText.text = $"Emotion";
        }
    }
}