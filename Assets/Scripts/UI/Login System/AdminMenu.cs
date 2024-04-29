using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EPRA.Utilities
{
    public class AdminMenu : MenuController
    {
        [SerializeField] private Button _checkEmployeeScores;
        [SerializeField] private Button _addNewEmployee;

        //[SerializeField] private TextMeshProUGUI _


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            _checkEmployeeScores.onClick.AddListener(CheckEmployeeScores);
            _addNewEmployee.onClick.AddListener(AddNewEmployee);
        }

        private void Finish()
        {
            _checkEmployeeScores.onClick.RemoveAllListeners();
            _addNewEmployee.onClick.RemoveAllListeners();
        }


        private void CheckEmployeeScores()
        {

        }

        private void AddNewEmployee()
        {

        }

    }
}
