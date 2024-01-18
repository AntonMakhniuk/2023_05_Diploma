using System;
using UnityEngine;

namespace Production.Challenges
{
    [Serializable]
    public abstract class ConfigBase : MonoBehaviour
    {
        public Difficulty difficulty;
    }

    [Serializable]
    public abstract class GeneralConfigBase : ConfigBase
    {
        public float resetWaitingTime;
        public int warningThreshold;
        public int failThreshold;
    }
}