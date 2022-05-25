using UnityEngine;
using UnityEngine.SceneManagement;
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

        public void ExitToMap()
        {
            SceneManager.LoadScene("Map");
        }

        public ActionPanel GetActionPanel()
        {
            return actionPanel;
        }
    }
}