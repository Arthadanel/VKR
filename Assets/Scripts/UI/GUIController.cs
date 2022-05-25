using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GUIController: MonoBehaviour
    {
        [SerializeField] private ActionPanel actionPanel;
        [SerializeField] private GameObject turnPanel;

        public void SetEnemyTurn(bool isEnemyTurn)
        {
            turnPanel.SetActive(isEnemyTurn);
        }
    }
}