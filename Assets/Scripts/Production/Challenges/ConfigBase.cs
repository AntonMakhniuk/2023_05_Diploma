using System;
using UnityEngine;

namespace Production.Challenges
{
    [Serializable]
    public abstract class ConfigBase : MonoBehaviour
    {
        public Difficulty difficulty;
        public float resetWaitingTime;
    }
}