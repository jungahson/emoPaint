using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DilmerGames.Events;

namespace DilmerGames
{
    public class UIInteractionEventHandler_Emotions : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private VREmotionChanged emotionChanged;

        public int index;
        private Button myselfButton;

        // Start is called before the first frame update
        void Start()
        {
            //myselfButton = GetComponent<Button>();
            //myselfButton.onClick.AddListener(() => ButtonClickFunction(index));
        }

       

        public void OnPointerDown(PointerEventData eventData)
        {
            
            emotionChanged?.Invoke(index); 
            /*GameObject drawline = GameObject.Find("VRSelectedPalette/DrawLine").GetComponent<GameObjHandler>().Activator();
            GameObject.Find("VRSelectedPalette/DrawShape").GetComponent<GameObjHandler>().Activator();
            GameObject.Find("Presets").GetComponent<ColorPickerPre>().SelectedEmotion(index);
            GameObject.Find("SPresets").GetComponent<ColorPickerPre>().SelectedEmotion(index);
            GameObject.Find("VRDrawRight").GetComponent<VRDraw>().LineMaterialBasedonEmotion(index);
            GameObject.Find("VRDrawRight").GetComponent<VRDraw>().ShapeParameterBasedonEmotion(index);*/
        }

    }
}
