using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.AirlockJam
{
    public class AirlockButton
    {
        // TODO: Link the necessary visuals to the script
        
        public bool IsTurnedOn;

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
            
            IsTurnedOn = false;
        }

        public void TurnOn()
        {
            // TODO: Implement animations and sound
            
            TurnOnSilently();
        }

        public void TurnOnSilently()
        {
            // TODO: Implement visual turning on

            IsTurnedOn = true;
        }
        
        public IEnumerator Blink(float timePerBlink, int totalBlinkCount)
        {
            for (int blinkCount = 0; blinkCount < totalBlinkCount; blinkCount++)
            {
                float currentTime = 0f;

                while (currentTime < timePerBlink)
                {
                    if (currentTime < timePerBlink / 3 && IsTurnedOn == true)
                    {
                        TurnOffSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 && 
                             currentTime < timePerBlink / 3 * 2 && 
                             IsTurnedOn == false)
                    {
                        TurnOnSilently();
                    }
                    else if (currentTime < timePerBlink / 3 && IsTurnedOn == true)
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