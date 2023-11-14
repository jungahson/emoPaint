using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DilmerGames
{
    public class Emotion_selection : MonoBehaviour
    {
        RectTransform m_RectTransform;
        public Image image; 

        // Start is called before the first frame update
        void Start()
        {
            m_RectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            image.enabled = false;
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Happy(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f);
                image.enabled = true; 
            }
            else
            {
                image.enabled = false; 
            }
        }

        public void Sad(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void Disgust(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 2*55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void Fear(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 3 * 55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void Surprise(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 4 * 55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void Anger(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 5 * 55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void Nameless(bool hover)
        {
            if (hover)
            {
                m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 6 * 55f);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        public void HappyClicked() 
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f);
            image.enabled = true;
        }

        public void SadClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 55f);
            image.enabled = true;
        }

        public void DisgustClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 2 * 55f);
            image.enabled = true;
        }

        public void FearClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 3 * 55f);
            image.enabled = true;
        }

        public void SurpriseClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 4 * 55f);
            image.enabled = true;
        }

        public void AngerClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 5 * 55f);
            image.enabled = true;
        }

        public void NamelessClicked()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, 143.8f - 6 * 55f);
            image.enabled = true;
        }
    }
}