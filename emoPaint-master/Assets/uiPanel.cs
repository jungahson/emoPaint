using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uiPanel : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Image colorWheel;  

    public GameObject colorPanel;
    public Transform thumb;
    public Image ColorBar;

    [Header("Config")]
    public Transform Picker;

    [Range(0, 5)]
    public float offZ;

    [Header("Freeze posX, posY")]
    public bool fixX;
    public bool fixY;

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(Picker.position, Picker.forward);
        /*if (Physics.Raycast(ray, out hit))
            SetThumbPosition(hit.point);*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetThumbPosition(eventData.position); 
    }

    private void SetThumbPosition(Vector3 point)
    {
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z + offZ);
        getImageColor(thumb.localPosition);

        showImageColor(getImageColor(thumb.localPosition));
    }

    private Color getImageColor(Vector2 point)
    {
        Vector2 rectPostion = mousePosToImagePos(point); 
        Sprite _sprite = colorPanel.GetComponent<Image>().sprite;
        Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
        Color imageColor = _sprite.texture.GetPixel(Mathf.FloorToInt(rectPostion.x * _sprite.texture.width / (rect.width)),
                                                     Mathf.FloorToInt(rectPostion.y * _sprite.texture.height / (rect.height)));
        return imageColor;
    }

    private Vector2 mousePosToImagePos(Vector2 point)
    {
        Vector2 ImagePos = Vector2.zero;
        Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
        ImagePos.x = point.x - colorPanel.transform.position.x + rect.width * 0.5f;
        ImagePos.y = point.y - colorPanel.transform.position.y + rect.height * 0.5f;
        return ImagePos;
    }

    private void showImageColor(Color _Color)
    {
        print(_Color.ToString("F2"));
        ColorBar.color = _Color;
    }
}
