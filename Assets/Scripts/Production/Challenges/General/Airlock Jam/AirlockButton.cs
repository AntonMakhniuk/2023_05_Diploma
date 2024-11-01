﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Airlock_Jam
{
    public class AirlockButton : MonoBehaviour
    {
        public bool isTurnedOn = true;
        
        // TODO: Change the visuals to more than just the color 

        [SerializeField] private Image assignedImage;
        
        // TODO: Link the necessary visuals to the script
        
        private bool _isInteractable = true;
        private GenAirlockJam _airlockJam;

        public Color32 onColor;
        public Color32 offColor;

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

        // TODO: fix this darn structure
        
        public void ChangeState()
        {
            if (!_isInteractable)
            {
                return;
            }
            
            if (isTurnedOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
        
        public void TurnOff()
        {
            // TODO: Implement animations and sound
            
            if (!isTurnedOn || !_isInteractable)
            {
                return;
            }
            
            TurnOffSilently();
        }
        
        public void TurnOffSilently()
        {
            // TODO: Implement visual turning off
            
            isTurnedOn = false;
            
            assignedImage.color = offColor;
        }
        
        public void TurnOn()
        {
            // TODO: Implement animations and sound

            if (isTurnedOn || !_isInteractable)
            {
                return;
            }
            
            TurnOnSilently();
        }

        public void TurnOnSilently()
        {
            // TODO: Implement visual turning on

            isTurnedOn = true;
            
            assignedImage.color = onColor;
        }
        
        public IEnumerator Blink(float timePerBlink, int totalBlinkCount)
        {
            _isInteractable = false;
            
            // TODO: the phase lengths are unequal?
            
           var currentBlinkCount = 0;
            
            while (currentBlinkCount < totalBlinkCount)
            {
                float startTime = Time.time;

                while (Time.time - startTime < timePerBlink)
                {
                    float currentTime = Time.time - startTime;
                    
                    if (currentTime < timePerBlink / 3 && isTurnedOn)
                    {
                        TurnOffSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 && 
                             currentTime < timePerBlink / 3 * 2 && 
                             !isTurnedOn)
                    {
                        TurnOnSilently();
                    }
                    else if (currentTime >= timePerBlink / 3 * 2 && isTurnedOn)
                    {
                        TurnOffSilently();
                    }

                    yield return null;
                }

                currentBlinkCount++;
            }
            
            TurnOnSilently();

            _isInteractable = true;
        }
    }
}