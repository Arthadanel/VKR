using System;
using UI;
using UnityEngine.EventSystems;
using Utility;

public class TransitionTile:Tile
{
    public int ConnectedLevel { get; private set; }

    private void UnlockLevel()
    {
        bool newLevel = SaveData.OPEN_LEVELS.Add(ConnectedLevel);
        if (newLevel)
            LevelController.DisplayMessage("New area unlocked");
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
            bool canInteract = LevelController.GetSelectedUnit().Coordinates.NextTo(Coordinates);
            if(canInteract)
                UnlockLevel();
        }
    }
}