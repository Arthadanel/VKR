using UnityEngine;

namespace AI
{
    public class Unit : MonoBehaviour
    {
        private int _range;
        private int _movement;
        private Coordinates _coordinates;
        public Coordinates Coordinates
        {
            get => _coordinates;
            set
            {
                UnitMovement.Move(this,_coordinates,value);//initiate movement
                _coordinates = value;
            }
        }

        public virtual bool Fight(Unit currentUnit)
        {
            throw new System.NotImplementedException();
        }
    }
}
