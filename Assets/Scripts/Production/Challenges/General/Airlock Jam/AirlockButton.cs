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
        
        public bool isTurnedOn = true;

        private GenAirlockJam _airlockJam;

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

        public void ChangeState()
        {
            switch (isTurnedOn)
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
            
            if (!isTurnedOn)
            {
                return;
            }
            
            TurnOffSilently();
            
            OnButtonTurnedOff?.Invoke(this);
        }
        
        public void TurnOffSilently()
        {
            // TODO: Implement visual turning off
            
            isTurnedOn = false;
            
            assignedImage.color = Color.red;
        }

        public event ButtonStatusHandler OnButtonTurnedOn;
        
        public void TurnOn()
        {
            // TODO: Implement animations and sound

            if (isTurnedOn)
            {
                return;
            }
            
            TurnOnSilently();
            
            OnButtonTurnedOn?.Invoke(this);
        }

        public void TurnOnSilently()
        {
            // TODO: Implement visual turning on

            isTurnedOn = true;
            
            assignedImage.color = Color.white;
        }
        
        public IEnumerator Blink(float timePerBlink, int totalBlinkCount)
        {
            for (int blinkCount = 0; blinkCount < totalBlinkCount; blinkCount++)
            {
                float currentTime = 0f;

                while (currentTime < timePerBlink)
                {
                    if (currentTime < timePerBlink / 3 && isTurnedOn)
                    {
                        TurnOffSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 && 
                             currentTime < timePerBlink / 3 * 2 && 
                             isTurnedOn == false)
                    {
                        TurnOnSilently();
                    }
                    else if (currentTime < timePerBlink / 3 && isTurnedOn)
                    {
                        TurnOffSilently();
                    }
                    
                    currentTime += Time.deltaTime;
                }
            }
            
            TurnOnSilently();
            
            yield return null;
        }
    }
}