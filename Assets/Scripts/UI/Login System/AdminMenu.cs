using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EPRA.Utilities
{
    public class AdminMenu : MenuController
    {
        [SerializeField] private Button _checkEmployeeScores;
        [SerializeField] private Button _addNewEmployee;
        [SerializeField] private Button _backButton;

        [Header("ID sub menu")]
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private TextMeshProUGUI _subMenuTitle;
        [SerializeField] private TextMeshProUGUI _employeeIDPrefix;
        [SerializeField] private TextMeshProUGUI _subMenuFeedback;
        [SerializeField] private TMP_InputField _employeeIDInput;
        [SerializeField] private Button _confirmIDButton;
        [SerializeField] private Button _closeSubMenu;
        
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
            _addNewEmployee.onClick.AddListener(NewEmployee);
            _backButton.onClick.AddListener(() => CanvasManager.Instance.CloseMenu(Menu));
            _closeSubMenu.onClick.AddListener(CloseSubMenu);

            CloseSubMenu();
        }

        private void Finish()
        {
            _checkEmployeeScores.onClick.RemoveAllListeners();
            _addNewEmployee.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _closeSubMenu.onClick.RemoveAllListeners();
        }


        private void CheckEmployeeScores()
        {
            _subMenuTitle.text = LanguageManager.GetTranslation("checkEmployeeScore");
            _employeeIDPrefix.text = FirebaseHandler.GetCompanyPrefix();

            _confirmIDButton.onClick.AddListener(GetEmployeeScore);

            OpenSubMenu();
        }

        private void NewEmployee()
        {
            _subMenuTitle.text = LanguageManager.GetTranslation("registerNewEmployee");
            _employeeIDPrefix.text = FirebaseHandler.GetCompanyPrefix();

            _confirmIDButton.onClick.AddListener(AddNewEmployee);

            OpenSubMenu();
        }

        private async void AddNewEmployee()
        {
            int employeeCount = await FirebaseHandler.GetCompanyEmployeeCount();
            int maxEmployeeCount = await FirebaseHandler.GetCompanyMaxEmployeeCount();
            if(employeeCount >= maxEmployeeCount)
            {
                //_subMenuFeedback.text = "Max employee number already reached";
                _subMenuFeedback.text = LanguageManager.GetTranslation("maxEmployeeNumberReached");
                return;
            }
            if (await FirebaseHandler.GetEmployeeExists(_employeeIDPrefix.text + _employeeIDInput.text))
            {
                //_subMenuFeedback.text = "Employee code already exists";
                _subMenuFeedback.text = LanguageManager.GetTranslation("employeeCodeAlreadyExists");
            }
            else
            {
                if (await FirebaseHandler.AddNewEmployee(_employeeIDPrefix.text + _employeeIDInput.text))
                {
                    //_subMenuFeedback.text = "New employee created successfully";
                    _subMenuFeedback.text = LanguageManager.GetTranslation("employeeCreatedSuccessfully");
                }
                else
                {
                    //_subMenuFeedback.text = "Failed to create new employee";
                    _subMenuFeedback.text = LanguageManager.GetTranslation("failedToCreateEmployee");
                }
            }
        }

        private async void GetEmployeeScore()
        {
            if (!await FirebaseHandler.GetEmployeeExists(_employeeIDPrefix.text + _employeeIDInput.text))
            {
                //_subMenuFeedback.text = "employeeDoesNotExist";
                _subMenuFeedback.text = LanguageManager.GetTranslation("employeeDoesNotExist");
            }
            else
            {
                int score = await FirebaseHandler.GetEmployeeScore(_employeeIDPrefix.text + _employeeIDInput.text);

                //_subMenuFeedback.text = "Employee score is " + score.ToString();
                _subMenuFeedback.text = LanguageManager.GetTranslation("employeeReport");
            }
        }

        private void OpenSubMenu()
        {
            _subMenuFeedback.text = string.Empty;
            _subMenu.SetActive(true);
        }

        private void CloseSubMenu()
        {
            _subMenuTitle.text = string.Empty;
            _subMenuFeedback.text = string.Empty;
            _confirmIDButton.onClick.RemoveAllListeners();
            _subMenu.SetActive(false);
        }


        public override void SelectUI()
        {
            _backButton.Select();
        }
    }
}
