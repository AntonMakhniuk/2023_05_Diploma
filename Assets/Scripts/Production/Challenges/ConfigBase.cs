using System;

namespace Production.Challenges
{
    [Serializable]
    public abstract class ConfigBase
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