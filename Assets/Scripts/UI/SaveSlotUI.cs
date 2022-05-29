using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SaveSlotUI:MonoBehaviour
    {
        [SerializeField] private int saveSlot;
        [SerializeField] private GameObject deleteButton;
        [SerializeField] private GameObject createButton;
        [SerializeField] private GameObject saveInfo;
        [SerializeField] private Text seed;
        [SerializeField] private Text difficulty;

        private void Start()
        {
            #if (UNITY_WEBGL)
            return;
            #endif
            InitSaveSlot();
        }

        private void InitSaveSlot()
        {
            SaveData saveData = SaveLoader.GetFileData(saveSlot);
            if (saveData is null)
            {
                deleteButton.SetActive(false);
                saveInfo.SetActive(false);
                createButton.SetActive(true);
                return;
            }
            deleteButton.SetActive(true);
            saveInfo.SetActive(true);
            createButton.SetActive(false);

            seed.text = "Seed: " + saveData.seed;
            difficulty.text = saveData.isDifficult ? "Hard" : "Normal";
        }

        public void CopySeedToClipboard(Text textField)
        {
            GUIUtility.systemCopyBuffer = textField.text.Substring(6);
        }

        public void SelectSaveSlot()
        {
            SaveLoader.LoadFile(saveSlot);
            SaveDataStorage.SAVE_SLOT = saveSlot;
            SceneManager.LoadScene("Map");
        }
        
        public void CreateSaveSlot()
        {
            SaveLoader.SaveFile(saveSlot);
            InitSaveSlot();
        }

        public void DeleteSaveSlot()
        {
            SaveLoader.DeleteFile(saveSlot);
            InitSaveSlot();
        }
    }
}