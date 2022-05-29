using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public static class SaveDataStorage
    {
        public static int SAVE_SLOT;
        public static int SEED = 4283579; //4283579
        //gameplay settings
        public static bool IS_DIFFICULT = false;
        public static bool MOUSE_NAVIGATION_ENABLED = true;
        public static bool KEYBOARD_NAVIGATION_ENABLED = true;
        //game progress
        public static HashSet<int> OPEN_LEVELS = new HashSet<int>() {1};
        public static List<int> LEVEL_COMPLETION = new List<int>() {0,0};

        public static bool AddOpenLevel(int levelNum)
        {
            bool levelAdded = OPEN_LEVELS.Add(levelNum);
            if (levelAdded)
            {
                Debug.Log(LEVEL_COMPLETION.Count);
                while (levelNum > LEVEL_COMPLETION.Count - 1)
                {
                    LEVEL_COMPLETION.Add(0);
                }
            }

            return levelAdded;
        }
    }
}