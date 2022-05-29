using System.Collections.Generic;

namespace Data
{
    public static class SaveData
    {
        public static int SEED = 4283579; //4283579
        //gameplay settings
        public static bool IS_DIFFICULT = false;
        public static bool MOUSE_NAVIGATION_ENABLED = true;
        public static bool KEYBOARD_NAVIGATION_ENABLED = true;
        //game progress
        public static HashSet<int> OPEN_LEVELS = new HashSet<int>() {1};
        public static List<int> LEVEL_COMPLETION = new List<int>();

        public static bool AddOpenLevel(int levelNum)
        {
            bool levelAdded = OPEN_LEVELS.Add(levelNum);
            if (levelAdded)
            {
                LEVEL_COMPLETION.Add(0);
            }

            return levelAdded;
        }
    }
}