using Data;
using Level;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Tiles
{
    public class TransitionTile:Tile
    {
        public int ConnectedLevel { get; private set; }

        private void UnlockLevel()
        {
            bool newLevel = SaveDataStorage.AddOpenLevel(ConnectedLevel);
            if (newLevel)
                LevelController.DisplayMessage("New area unlocked",true);
        }
    
        public new void InitializeTilePrefab(Coordinates coordinates,int connectedLevel)
        {
            base.InitializeTilePrefab(coordinates);
            ConnectedLevel = connectedLevel;
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (LevelController.IsUnitSelected)
            {
                if (SaveDataStorage.LEVEL_COMPLETION[SelectedLevelData.GetLevelPolygon().LevelNumber] == 0)
                {
                    LevelController.DisplayMessage("Transition locked, defeat all enemies first",true);
                }
                else
                {
                    bool canInteract = LevelController.GetSelectedUnit().Coordinates.NextTo(Coordinates);
                    if(canInteract)
                        UnlockLevel();
                    else
                    {
                        LevelController.DisplayMessage("You must be near the door to open it",true);
                    }
                }
            }
        }
    }
}