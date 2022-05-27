using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    public class ButtonFunctions:MonoBehaviour
    {
        [SerializeField] private List<GameObject> mainPanels;

        private void Start()
        {
            //todo: init save slots
        }

        public void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void DisplayPanel(GameObject panel)
        {
            foreach (var mainPanel in mainPanels)
            {
                mainPanel.SetActive(false);
            }
            panel.SetActive(true);
        }

        public void CopySeedToClipboard(Text textField)
        {
            GUIUtility.systemCopyBuffer = textField.text.Substring(6);
        }

        public void DeleteSaveSlot(int slotIndex)
        {
            
        }
        public void CreateSaveSlot(int slotIndex)
        {
            
        }

        public void SelectSaveSlot(int slotIndex)
        {
            LoadScene("Map");
        }
        
    }
}