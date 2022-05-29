using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace UI
{
    public class ButtonFunctions:MonoBehaviour
    {
        [SerializeField] private bool isMainMenu;
        [SerializeField] private List<GameObject> mainPanels;

        [SerializeField] private InputField seed;
        [SerializeField] private Dropdown difficulty;
        [SerializeField] private Dropdown cameraControls;

        private void Start()
        {
            if(!isMainMenu) return;
            InitSettings();
            InitSaveSlots();
        }

        private void InitSettings()
        {
            seed.text = SaveData.SEED.ToString();
            difficulty.value = SaveData.IS_DIFFICULT ? 1 : 0;
            cameraControls.value =
                !SaveData.MOUSE_NAVIGATION_ENABLED ? 1 : !SaveData.KEYBOARD_NAVIGATION_ENABLED ? 2 : 0;
        }

        private void InitSaveSlots()
        {
            
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

        public void Play(GameObject savePanel)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                LoadScene("Map");
            }
            else
            {
                DisplayPanel(savePanel);
            }
        }

        public void SelectDifficulty(Dropdown dropdown)
        {
            SaveData.IS_DIFFICULT = dropdown.value == 1;
        }
        
        public void SelectCameraControls(Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    SaveData.KEYBOARD_NAVIGATION_ENABLED = true;
                    SaveData.MOUSE_NAVIGATION_ENABLED = true;
                    break;
                case 1:
                    SaveData.KEYBOARD_NAVIGATION_ENABLED = true;
                    SaveData.MOUSE_NAVIGATION_ENABLED = false;
                    break;
                case 2:
                    SaveData.KEYBOARD_NAVIGATION_ENABLED = false;
                    SaveData.MOUSE_NAVIGATION_ENABLED = true;
                    break;
            }
        }

        public void ChangeSeed(InputField inputField)
        {
            if (!inputField.interactable)
            {
                inputField.interactable=true;
                return;
            }
            inputField.interactable = false;
            int seed;
            try
            {
                seed = Int32.Parse(inputField.text);
            }
            catch (FormatException)
            {
                seed = Random.Range(0, int.MaxValue);
                inputField.text = seed.ToString();
            }

            SaveData.SEED = seed;
        }
    }
}