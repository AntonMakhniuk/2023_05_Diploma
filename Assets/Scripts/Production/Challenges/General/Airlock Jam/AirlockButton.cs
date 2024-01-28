using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Airlock_Jam
{
    public class AirlockButton : MonoBehaviour
    {
        // TODO: Change the visuals to more than just the color 

        [SerializeField] private Image assignedImage;
        
        // TODO: Link the necessary visuals to the script
        
        private bool _isTurnedOn = true;
        private bool _isInteractable = true;
        private GenAirlockJam _airlockJam;

        private static readonly Color32 OnColor = new(24, 132, 62, 255);
        private static readonly Color32 OffColor = new(224, 18, 0, 255);

        private void Start()
        {
            _airlockJam = GetComponentInParent<GenAirlockJam>();

            if (_airlockJam == null)
            {
                throw new Exception($"'{nameof(_airlockJam)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenAirlockJam");
            }
        }

        // The methods are split into normal and silent so that programmatic
        // disabling of the buttons can be done without playing a sound or an
        // animation to the player, to avoid overloading the senses

        public delegate void ButtonStatusHandler(AirlockButton button);
        public event ButtonStatusHandler OnButtonTurnedOff;

        // TODO: fix this darn structure
        
        public void ChangeState()
        {
            if (!_isInteractable)
            {
                return;
            }
            
            switch (_isTurnedOn)
            {
                case true:
                {
                    TurnOff();
                    
                    break;
                }
                case false:
                {
                    TurnOn();

                    break;
                }
            }
        }
        
        public void TurnOff()
        {
            // TODO: Implement animations and sound
            
            if (!_isTurnedOn || !_isInteractable)
            {
                return;
            }
            
            TurnOffSilently();
            
            OnButtonTurnedOff?.Invoke(this);
        }
        
        public void TurnOffSilently()
        {
            // TODO: Implement visual turning off
            
            _isTurnedOn = false;
            
            assignedImage.color = OffColor;
        }

        public event ButtonStatusHandler OnButtonTurnedOn;
        
        public void TurnOn()
        {
            // TODO: Implement animations and sound

            if (_isTurnedOn || !_isInteractable)
            {
                return;
            }
            
            TurnOnSilently();
            
            OnButtonTurnedOn?.Invoke(this);
        }

        public void TurnOnSilently()
        {
            // TODO: Implement visual turning on

            _isTurnedOn = true;
            
            assignedImage.color = OnColor;
        }
        
        public IEnumerator Blink(float timePerBlink, int totalBlinkCount)
        {
            _isInteractable = false;
            
           var currentBlinkCount = 0;
            
            while (currentBlinkCount < totalBlinkCount)
            {
                float startTime = Time.time;

                while (Time.time - startTime < timePerBlink)
                {
                    float currentTime = Time.time - startTime;
                    
                    if (currentTime < timePerBlink / 3 && _isTurnedOn)
                    {
                        TurnOffSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 && 
                             currentTime < timePerBlink / 3 * 2 && 
                             !_isTurnedOn)
                    {
                        TurnOnSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 * 2 && _isTurnedOn)
                    {
                        TurnOffSilently();
                    }

                    yield return null;
                }

                currentBlinkCount++;
            }
            
            TurnOnSilently();

            _isInteractable = true;
            
            yield return null;
        }
    }
}