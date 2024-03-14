using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Production.Challenges.General.Rod_Positioning
{
    public class HandleHoldRestriction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public RodLever lever;
        public Slider slider;
        public RectTransform handleTransform;
        
        private bool _isHeldDown;

        private void Start()
        {
            slider.interactable = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(handleTransform, 
                    eventData.position, eventData.enterEventCamera) || !lever.isInteractable)
            {
                return;
            }
            
            slider.interactable = true;
            
            UpdateSliderValue();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            slider.interactable = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!lever.isInteractable)
            {
                slider.interactable = false;
                
                return;
            }
            
            UpdateSliderValue();
        }

        private void UpdateSliderValue()
        {
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, slider.normalizedValue);
            lever.ChangeCurrentPosition(slider.value);
        }
    }
}