using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionPanel: MonoBehaviour
    {
        [SerializeField] private Text moveCost;
        [SerializeField] private Text attackCost;
        [SerializeField] private Text specialActionCost;
        [SerializeField] private GameObject moveBtn;
        [SerializeField] private GameObject attackBtn;
        [SerializeField] private GameObject specialBtn;
        [SerializeField] private GameObject cancelBtn;

        public void InitializeActionPanel(Unit unit)
        {
            
        }
        
        public void Move()
        {
            
        }
        public void Attack()
        {
            
        }
        public void SpecialAction()
        {
            
        }
    }
}