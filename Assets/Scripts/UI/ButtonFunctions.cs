using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

        [SerializeField] private Button quit;

        private void Start()
        {
            if(!isMainMenu) return;
            #if (UNITY_WEBGL)
            quit.gameObject.SetActive(false);
            #endif
            InitSettings();
        }

        private void InitSettings()
        {
            seed.text = SaveDataStorage.SEED.ToString();
            difficulty.value = SaveDataStorage.IS_DIFFICULT ? 1 : 0;
            cameraControls.value =
                !SaveDataStorage.MOUSE_NAVIGATION_ENABLED ? 1 : !SaveDataStorage.KEYBOARD_NAVIGATION_ENABLED ? 2 : 0;
        }

        public void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
            Application.Quit();
            #endif
        }

        public void LoadScene(string sceneName)
        {
            if (sceneName == "Menu")
            {
                SaveLoader.SaveFile(SaveDataStorage.SAVE_SLOT);
            }
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
            SaveDataStorage.IS_DIFFICULT = dropdown.value == 1;
        }
        
        public void SelectCameraControls(Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    SaveDataStorage.KEYBOARD_NAVIGATION_ENABLED = true;
                    SaveDataStorage.MOUSE_NAVIGATION_ENABLED = true;
                    break;
                case 1:
                    SaveDataStorage.KEYBOARD_NAVIGATION_ENABLED = true;
                    SaveDataStorage.MOUSE_NAVIGATION_ENABLED = false;
                    break;
                case 2:
                    SaveDataStorage.KEYBOARD_NAVIGATION_ENABLED = false;
                    SaveDataStorage.MOUSE_NAVIGATION_ENABLED = true;
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

            SaveDataStorage.SEED = seed;
        }
    }
}