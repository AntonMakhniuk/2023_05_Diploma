using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.AirlockJam
{
    public class AirlockButton : MonoBehaviour
    {
        // TODO: Link the necessary visuals to the script
        
        public bool isTurnedOn;

        // The methods are split into normal and silent so that programmatic
        // disabling of the buttons can be done without playing a sound or an
        // animation to the player, to avoid overloading the senses
        
        public void TurnOff()
        {
            // TODO: Implement animations and sound
            
            TurnOffSilently();
        }
        
        public void TurnOffSilently()
        {
            // TODO: Implement visual turning off
            
            isTurnedOn = false;
        }

        public void TurnOn()
        {
            // TODO: Implement animations and sound
            
            TurnOnSilently();
        }

        public void TurnOnSilently()
        {
            // TODO: Implement visual turning on

            isTurnedOn = true;
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