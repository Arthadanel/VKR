using Data;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType type = TileType.GRASS;
        [SerializeField] private bool isObstruction = false;
        [SerializeField] private int movementCost = 1;
        [SerializeField] private int attackHindrance = 0;
        [SerializeField] private Sprite swapSprite;
    
        private Coordinates _coordinates;
        private GameObject _highlighter;
        public int TileInteractionCost { get; set; } = GameSettings.MOVE_LIMIT;
        public Edge Edge = null;
        public Coordinates Coordinates
        {
            get => _coordinates;
            set => _coordinates = value;
        }
        private TileType _tileType;
        private bool _isObstruction;
        public bool IsObstruction()
        {
            return _isObstruction;
        }
        public bool IsOccupied { get; private set; }
        private int _movementCost;

        public int GetMovementCost()
        {
            return IsObstruction() || IsOccupied ? GameSettings.MOVE_LIMIT : _movementCost;
        }
        private int _attackHindrance;
        public int GetAttackHindrance()
        {
            return _attackHindrance;
        }

        private Unit _currentUnit;

        // public void InitializeTile(Coordinates coordinates, TileType type, bool isObstruction = false)
        // {
        //     _coordinates = coordinates;
        //     _tileType = type;
        //     //todo: set move cost based on tile type
        //     _movementCost = 1;
        //     _isObstruction = isObstruction;
        //     //todo: set move cost based on tile type and obstruction property
        //     _attackHindrance = 0;
        // }
        public void InitializeTilePrefab(Coordinates coordinates)
        {
            _coordinates = coordinates;
            _tileType = type;
            _isObstruction = isObstruction;
            _movementCost = movementCost;
            _attackHindrance = attackHindrance;
        }

        public Unit GetCurrentUnit()
        {
            return _currentUnit;
        }

        public void MoveTo(Unit unit)
        {
            LevelController.GetTileAtCoordinates(unit.Coordinates).GetTileData().Vacate();
            IsOccupied = true;
            _currentUnit = unit;
            unit.Coordinates = _coordinates;
        }

        public void Vacate()
        {
            IsOccupied = false;
            _currentUnit = null;
        }

        public void AddHighlighter(GameObject highlighter)
        {
            var position = transform.position;
            _highlighter = Instantiate(highlighter, new Vector3(position.x,position.y,-1), Quaternion.identity, transform);
        }
    
        public void ClearHighlighter()
        {
            Destroy(_highlighter);
            _highlighter = null;
            TileInteractionCost = GameSettings.MOVE_LIMIT;
        }

        public void PlaceUnit(Unit unit)
        {
            unit.SetInitialCoordinates(Coordinates.X, Coordinates.Y);
            IsOccupied = true;
            _currentUnit = unit;
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            LevelController.PositionSelector(_coordinates);
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Unit unit = LevelController.GetSelectedUnit();
            if (_tileType == TileType.WOOD && unit.SelectedAction == ActionType.ATTACK)
            {
                _isObstruction = false;
                _attackHindrance = 0;
                _tileType = TileType.GRASS;
                gameObject.GetComponent<SpriteRenderer>().sprite = swapSprite;
                LevelController.ConsumeActionPoints(Unit.ATTACK_COST);
                LevelController.DeactivateUnitSelection();
                return;
            }
            
            if(_isObstruction) return;
            if (LevelController.IsUnitSelected)
            {
                if (_highlighter is null) return;
                unit.InteractWith(this);
            }
        }

        public void CallOnMouseDown()
        {
            OnMouseDown();
        }
    }
}
