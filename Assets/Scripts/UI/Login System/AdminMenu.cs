using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            _subMenuTitle.text = "Check employee score";

            OpenSubMenu();
        }

        private void NewEmployee()
        {
            _subMenuTitle.text = "Add new employee";
            _employeeIDPrefix.text = FirebaseHandler.GetCompanyPrefix();

            _confirmIDButton.onClick.AddListener(AddNewEmployee);

            OpenSubMenu();
        }

        private async void AddNewEmployee()
        {
            if (await FirebaseHandler.GetEmployeeExists(_employeeIDPrefix.text + _employeeIDInput.text))
            {
                _subMenuFeedback.text = "Employee code already exists";
            }
            else
            {
                if (await FirebaseHandler.AddNewEmployee(_employeeIDPrefix.text + _employeeIDInput.text))
                {
                    _subMenuFeedback.text = "New employee created successfully";
                }
                else
                {
                    _subMenuFeedback.text = "Failed to create new employee";
                }
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
