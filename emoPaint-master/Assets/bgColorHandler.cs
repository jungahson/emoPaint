using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

namespace DilmerGames
{
    public class bgColorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField]
        private GameObject Ground; 

        [SerializeField]
        private GameObject Wall1; 

        [SerializeField]
        private GameObject Wall2;

        [SerializeField]
        private GameObject Wall3;

        [SerializeField]
        private GameObject Wall4; 

        public GameObject colorPanel;
        public Transform thumb;
        public Image ColorBar;
        public Color currentBGColor;  

        public Vector3 panelRelative;

        GameObject currentHover;

        [Range(0, 5)]
        public float offZ;

        [Header("Freeze posX, posY")]
        public bool fixX;
        public bool fixY;

        void Start()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
                currentHover = eventData.pointerCurrentRaycast.gameObject;
            }
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            currentHover = null;
            VRStats.Instance.thirdText.text = $"PointerExit";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 result; 
            
            Vector3 clickPosition = eventData.pointerCurrentRaycast.worldPosition; 
            RectTransform thisRect = GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, clickPosition, null, out result);
            //result += thisRect.sizeDelta / 2;

            Vector3 panelRelative = transform.InverseTransformPoint(clickPosition); 
            
            SetThumbPosition(panelRelative); 
        }

        void Update()
        {
            
        }

        private void SetThumbPosition(Vector3 point)
        {
            Vector3 temp = thumb.localPosition;

            //thumb.position = point;
            thumb.localPosition = point; 
            //thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z + offZ);
            currentBGColor = getImageColor(thumb.localPosition);

            showImageColor(getImageColor(thumb.localPosition));
            changeBGColor(getImageColor(thumb.localPosition)); 
        }

        private Color getImageColor(Vector2 point)
        {
            Sprite _sprite = colorPanel.GetComponent<Image>().sprite;
            Rect rect = colorPanel.GetComponent<RectTransform>().rect;
            
            Vector2 rectPosition = mousePosToImagePos(point);
            
            VRStats.Instance.thirdText.text = $"{Mathf.FloorToInt(rectPosition.x * _sprite.texture.width / (rect.width))}";
            

            Color imageColor = _sprite.texture.GetPixel(Mathf.FloorToInt(rectPosition.x * _sprite.texture.width / (rect.width)),
                                                         Mathf.FloorToInt(rectPosition.y * _sprite.texture.height / (rect.height)));
            VRStats.Instance.fourthText.text = $"{imageColor}";
            return imageColor;
        }

        private Vector2 mousePosToImagePos(Vector2 point)
        {
            Vector2 ImagePos = Vector2.zero;
            Rect rect = colorPanel.GetComponent<RectTransform>().rect;
            ImagePos.x = point.x + rect.width * 0.5f;
            ImagePos.y = point.y + rect.height * 0.5f;
            return ImagePos;
        }

        private void showImageColor(Color _Color)
        {
            print(_Color.ToString("F2"));
            ColorBar.color = _Color;
        }

        private void changeBGColor(Color _Color)
        {
            Ground.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", _Color);
            Ground.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", _Color);

            Wall1.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", _Color);
            Wall1.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", _Color);

            Wall2.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", _Color);
            Wall2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", _Color);

            Wall3.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", _Color);
            Wall3.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", _Color);

            Wall4.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", _Color);
            Wall4.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", _Color);

        }
    }
}