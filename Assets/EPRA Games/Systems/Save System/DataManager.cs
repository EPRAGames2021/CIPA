using ES3Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class DataManager
    {
        public static void SaveData<T>(string saveName, T saveData)
        {
            ES3.Save<T>(saveName, saveData);
        }

        public static bool HasData(string loadName)
        {
            return ES3.KeyExists(loadName);
        }

        public static T LoadData<T>(string loadName)
        {
            if (HasData(loadName))
            {
                return ES3.Load<T>(loadName);
            }
            else
            {
                return default;
            }
        }

        public static void ResetAllSaveData()
        {
            if (ES3.FileExists())
            {
                ES3.DeleteFile();
            }
        }
    }
}