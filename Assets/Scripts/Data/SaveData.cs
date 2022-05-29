using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class SaveData
    {
        public int seed;
        public bool isDifficult;
        public List<int> openLevels;
        public List<int> levelCompletion;

        public SaveData(int seed, bool isDifficult, HashSet<int> openLevels, List<int> levelCompletion)
        {
            this.seed = seed;
            this.isDifficult = isDifficult;
            this.levelCompletion = levelCompletion;
            List<int> openLevelsSet = new List<int>();
            foreach (var level in openLevels)
            {
                openLevelsSet.Add(level);
            }
            this.openLevels = openLevelsSet;
        }
    }
}