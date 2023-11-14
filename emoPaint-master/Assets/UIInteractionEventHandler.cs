using UnityEngine;
using DilmerGames.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

namespace DilmerGames
{
    public class UIInteractionEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private VRValidColorGradient validColorChanged;

        private void Awake()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            validColorChanged?.Invoke(true);
            VRStats.Instance.secondText.text = $"Hover: White";
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            validColorChanged?.Invoke(false);
            VRStats.Instance.secondText.text = $"Hover: Red";
        }

    }
}
