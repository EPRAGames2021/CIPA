using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using Firebase.Auth;

namespace EPRA.Utilities
{
    public class LoginMenu : MenuController
    {
        [SerializeField] private Button _confirmButton;

        [Header("Inputs")]
        [SerializeField] private TMP_InputField _idInput;
        [SerializeField] private TextMeshProUGUI _idInputFeedback;
        [SerializeField] private SimpleTranslate _idInputFeedbackTranslate;

        [SerializeField] private TextMeshProUGUI _passwordTip;
        [SerializeField] private SimpleTranslate _passwordTipTranslate;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TextMeshProUGUI _passwordFeedback;
        [SerializeField] private SimpleTranslate _passwordFeedbackTranslate;

        [SerializeField] private TMP_InputField _confirmPasswordInput;
        [SerializeField] private TextMeshProUGUI _confirmPasswordFeedback;
        [SerializeField] private SimpleTranslate _confirmPasswordFeedbackTranslate;

        [Header("Other")]
        [SerializeField] private GameObject _idInputContainer;
        [SerializeField] private GameObject _passwordInputContainer;
        [SerializeField] private GameObject _confirmPasswordInputContainer;

        [SerializeField] private string _company;        

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            Menu = MenuType.LoginMenu;

            _company = string.Empty;

            _idInputFeedbackTranslate.Clear();
            _passwordFeedbackTranslate.Clear();
            _confirmPasswordFeedbackTranslate.Clear();

            _idInputContainer.SetActive(true);
            _passwordInputContainer.SetActive(false);
            _confirmPasswordInputContainer.SetActive(false);

            _confirmButton.onClick.AddListener(CheckCredentials);

            Authtentication();
        }

        private async void Authtentication()
        {
            var auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser == null) {
                await auth.SignInAnonymouslyAsync();
            }
        }

        private void Finish()
        {
            _confirmButton.onClick.RemoveAllListeners();
        }


        private async void CheckCredentials()
        {
            //done
            //check whether its organization id or user id
            //if its organization id check if its valid
            //if valid, check if admin account has been created
            //if so, prompt user to insert password to login
            //if not, prompt user to create admin account (create password)
            //if admin account exists and password is correct, go to main menu
            //if account does not exist (no password has been saved) and password is valid, save password and go to main menu

            //todo
            //if its user id check if its valid
            //if valid, check if it is its first time logging in
            //if so, prompt user to create password
            //if not, ask for password

            //IMPORTANT:
            //if password doesn't exist and account fails to be created for some reason, overwrite old password rather than append new when hitting confirm again
            //also passwords should be encrypted

            string idInput = _idInput.text;

            if (FirebaseHandler.GetIsCompanyID(idInput))
            {
                _company = idInput;

                if (await FirebaseHandler.GetCompanyExists(_company))
                {
                    if (await FirebaseHandler.GetAdminAccountCreated(_company))
                    {
                        SetFeedback(_passwordTipTranslate, "insertPassword");
                        SetPasswordFieldEnabled(true);

                        if (_passwordInput.text.Length > 0)
                        {
                            if (await FirebaseHandler.GetPasswordIsCorrect(_company, _passwordInput.text))
                            {
                                FirebaseHandler.Instance.IsAdminAccount = true;

                                FirebaseHandler.SetCompany(_company);

                                GoToMainMenu();
                            }
                            else
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordOrIDIncorrect");
                            }
                        }
                    }
                    else
                    {
                        SetFeedback(_passwordTipTranslate, "createPasswordRequirements");
                        SetNewPasswordFieldsEnabled(true);

                        if (!FirebaseHandler.GetNewPasswordIsValid(_passwordInput.text, _confirmPasswordInput.text))
                        {                            
                            if (_passwordInput.text == _confirmPasswordInput.text)
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordIsNotValid");
                            }
                            else
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordsDoNotMatch");
                            }
                        } 
                        else
                        {
                            if (await FirebaseHandler.SetNewAdminAccount(_company, _passwordInput.text))
                            {
                                GoToMainMenu();
                            }
                        }
                    }
                }
                else
                {
                    SetFeedback(_idInputFeedbackTranslate, "organizationDoesNotExist");
                }
            }
            else if (FirebaseHandler.GetIsEmployeeID(idInput))
            {
                _company = await FirebaseHandler.GetEmployeeCompany(idInput);

                if (_company != null)
                {
                    if (await FirebaseHandler.GetIsEmployeeFirstLogin(idInput))
                    {
                        SetFeedback(_passwordTipTranslate, "createPasswordRequirements");
                        SetNewPasswordFieldsEnabled(true);

                        if (!FirebaseHandler.GetNewPasswordIsValid(_passwordInput.text, _confirmPasswordInput.text))
                        {
                            if (_passwordInput.text == _confirmPasswordInput.text)
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordIsNotValid");
                            }
                            else
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordsDoNotMatch");
                            }
                        }
                        else
                        {
                            if (await FirebaseHandler.SetPassword(idInput, _passwordInput.text))
                            {
                                GoToMainMenu();
                            }
                        }
                    }
                    else
                    {
                        SetFeedback(_passwordTipTranslate, "insertPassword");
                        SetPasswordFieldEnabled(true);

                        if (_passwordInput.text.Length > 0)
                        {
                            if (await FirebaseHandler.GetPasswordIsCorrect(idInput, _passwordInput.text))
                            {
                                FirebaseHandler.SetCompany(_company);

                                GoToMainMenu();
                            }
                            else
                            {
                                SetFeedback(_passwordFeedbackTranslate, "passwordOrIDIncorrect");
                            }
                        }
                    }
                }
                else
                {
                    SetFeedback(_passwordFeedbackTranslate, "employeeDoesNotExist");
                }
            }
            else
            {
                SetFeedback(_passwordFeedbackTranslate, "organizationDoesNotExist");
                SetNewPasswordFieldsEnabled(false);
            }
        }



        private void SetPasswordFieldEnabled(bool enable)
        {
            _passwordInputContainer.SetActive(enable);
        }

        private void SetNewPasswordFieldsEnabled(bool enable)
        {
            _passwordInputContainer.SetActive(enable);
            _confirmPasswordInputContainer.SetActive(enable);
        }

        private void SetFeedback(SimpleTranslate simpleTranslate, string feedback)
        {
            simpleTranslate.SetKey(feedback);

            //Debug.Log(simpleTranslate.Text);
        }

        private void GoToMainMenu()
        {
            CanvasManager.Instance.SwitchMenu(MenuType.MainMenu);
        }


        public override void SelectUI()
        {
            _confirmButton.Select();
        }
    }
}
