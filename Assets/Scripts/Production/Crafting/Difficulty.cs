using System;
using Production.Challenges;

namespace Production.Crafting
{
    [Serializable]
    public class DifficultyConfig : ConfigBase
    {
        public int productionLengthInSeconds;
        public Difficulty recipeDifficulty;
    }
    
    public enum Difficulty
    {
        Automatic, Low, Normal, High
    }
}