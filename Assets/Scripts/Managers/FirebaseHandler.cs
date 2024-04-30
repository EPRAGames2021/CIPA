using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using ES3Types;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine.Windows;

namespace EPRA.Utilities
{
    public class FirebaseHandler : MonoBehaviour
    {
        public static FirebaseHandler Instance;

        [SerializeField] private FirebaseApp _app;
        [SerializeField] private DatabaseReference _databaseReference;
        [SerializeField] private bool _isConnected;

        [SerializeField] private string _loggedID;
        [SerializeField] private string _companyCode;
        [SerializeField] private string _companyDisplayName;
        [SerializeField] private bool _isAdminAccount;


        public FirebaseApp App => _app;
        public DatabaseReference Database => _databaseReference;
        public bool IsConnected => _isConnected;

        public string LoggedID => _loggedID;
        public string CompanyDisplayName => _companyDisplayName;
        public bool IsAdminAccount { get { return _isAdminAccount; } set { _isAdminAccount = value; } }


        private void Awake()
        {
            InitSingleton();
            AttemptConnection();
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void AttemptConnection()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
            {
                var dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
                    _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                    _isConnected = true;
                }
                else
                {
                    Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));

                    _isConnected = false;
                }
            });
        }


        private static async Task<bool> CheckIfFieldExists(string path)
        {
            DatabaseReference database = Instance.Database.Child(path);

            try
            {
                DataSnapshot snapshot = await database.GetValueAsync();

                return snapshot.Exists;
            }
            catch (AggregateException aggregateException)
            {
                Debug.LogError("Error fetching data: " + aggregateException);

                return false;
            }
        }

        private static async Task<T> GetValueOfField<T>(string path)
        {
            DatabaseReference database = Instance.Database.Child(path);

            try
            {
                DataSnapshot snapshot = await database.GetValueAsync();

                if (snapshot.Exists)
                {
                    // Convert the value to the specified type
                    return snapshot.Value != null ? (T)Convert.ChangeType(snapshot.Value, typeof(T)) : default;
                }
                else
                {
                    return default;
                }
            }
            catch (AggregateException aggregateException)
            {
                Debug.LogError("Error fetching data: " + aggregateException);

                return default;
            }
        }

        //split this between SetValueOfField() and AppendValueToField()
        //this will be extremely useful when adding users under the umbrulla of a given company
        private static async Task<T> PushValueToField<T>(string path, T value)
        {
            DatabaseReference database = Instance.Database.Child(path);

            try
            {
                await database.SetValueAsync(value);

                return value;
            }
            catch (Exception exception)
            {
                Debug.LogError("Error pushing data: " + exception);

                return default;
            }
        }


        public static async Task<bool> GetCompanyExists(string company)
        {
            Debug.Log("Does " + company + " exist? " + await CheckIfFieldExists("Companies" + "/" + company));

            return await CheckIfFieldExists("Companies" + "/" + company);
        }

        public static async Task<bool> GetAdminAccountCreated(string company)
        {
            if (!await CheckIfFieldExists("Companies" + "/" + company + "/" + "AdminCreated"))
            {
                Debug.Log("Admin field for " + company + " does not exist. Creating it and setting it to false.");

                await SetAdminCreated(company, false);

                return false;
            }
            else
            {
                Debug.Log("Has " + company + " admin account been created? " + await GetValueOfField<bool>("Companies" + "/" + company + "/" + "AdminCreated"));

                return await GetValueOfField<bool>("Companies" + "/" + company + "/" + "AdminCreated");
            }
        }

        public static async Task<bool> GetPasswordIsCorrect(string company, string password)
        {
            if (!await CheckIfFieldExists("Companies" + "/" + company + "/" + "AdminCreated"))
            {
                Debug.Log("Password field for " + company + " does not exist. Creating it and setting it to an empty string.");

                await SetCompanyPassword(company, string.Empty);

                return false;
            }
            else
            {
                Debug.Log("Is password of " + company + " correct? " + await GetValueOfField<string>("Companies" + "/" + company + "/" + "Password") == password);

                return await GetValueOfField<string>("Companies" + "/" + company + "/" + "Password") == password;
            }
        }

        public static async Task<bool> GetEmployeeExists(string id)
        {
            if (await GetEmployeeCompany(id) == null)
            {
                return false;
            }

            return true;
        }


        public static async Task<bool> SetCompanyPassword(string company, string password)
        {
            return await PushValueToField<string>("Companies" + "/" + company + "/" + "Password", password) != default;
        }

        public static async Task<bool> SetAdminCreated(string company, bool created)
        {
            return await PushValueToField<bool>("Companies" + "/" + company + "/" + "AdminCreated", created);
        }

        public static async Task<bool> SetNewAdminAccount(string company, string password)
        {
            if (await SetCompanyPassword(company, password))
            {
                if (await SetAdminCreated(company, true))
                {
                    Debug.Log("Success. Admin account created for company " + company);

                    return true;
                }
                else 
                {
                    Debug.Log("Something went wrong. Couldn't change the status of admin account created to true");

                    return false;
                }
            }
            else
            {
                Debug.Log("Something went wrong. Couldn't set password");

                return false;
            }
        }

        public static async Task<bool> AddNewEmployee(string id)
        {
            return await AddNewEmployee(Instance._companyCode, id);
        }

        public static async Task<bool> AddNewEmployee(string company, string id)
        {
            if (await GetCompanyExists(company) == default) return false;
            else if (await PushValueToField<string>("Companies" + "/" + company + "/" + "Employees" + "/", id) == default) return false;
            else if (await PushValueToField<string>("Companies" + "/" + company + "/" + "Employees" + "/" + id, "Password") == default) return false;
            
            return true;
        }



        public static bool GetIsCompanyID(string code)
        {
            //14 CNPJ            //3 letters            
            string pattern = @"^\d{14}[A-Za-z]{3}";

            Debug.Log("Is " + code + " a company valid id? " + Regex.IsMatch(code, pattern));

            return Regex.IsMatch(code, pattern);
        }

        public static string GetCompanyPrefix()
        {
            Debug.Log(GetPrefixInternal(Instance._companyCode));
            return GetPrefixInternal(Instance._companyCode);
        }

        private static string GetPrefixInternal(string name)
        {
            string companyFirstThree = name[..3];
            string companyLastThree = name[^3..];
            
            return companyFirstThree + companyLastThree;
        }

        public static void SetCompany(string company)
        {
            Instance._companyCode = company;
        }

        public static async Task<List<string>> GetAllCompanies()
        {
            DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            List<string> companyNames = new List<string>();

            try
            {
                // Get the reference to the "Companies" field
                DatabaseReference companiesRef = databaseReference.Child("Companies");

                // Retrieve the snapshot of the "Companies" field
                DataSnapshot companiesSnapshot = await companiesRef.GetValueAsync();

                // Check if the snapshot exists and has children
                if (companiesSnapshot != null && companiesSnapshot.Exists && companiesSnapshot.HasChildren)
                {
                    // Iterate through the child nodes under "Companies"
                    foreach (DataSnapshot companySnapshot in companiesSnapshot.Children)
                    {
                        // Get the name of each company and add it to the list
                        string companyName = companySnapshot.Key;
                        companyNames.Add(companyName);
                    }
                }
                else
                {
                    Debug.LogWarning("No companies found in the database.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error fetching companies: " + ex);
            }

            return companyNames;
        }

        public static async Task<string> GetEmployeeCompany(string id)
        {
            List<string> companies = await GetAllCompanies();

            Instance._companyCode = null;

            foreach (string company in companies)
            {
                Debug.Log(company);

                string parsedCompanyName = GetPrefixInternal(company);
                string employeePrefix = id[..6];

                if (employeePrefix == parsedCompanyName)
                {
                    Debug.Log("Employee " + id + " belongs to company " + company);

                    Instance._companyCode = company;

                    break;
                }
            }

            return Instance._companyCode;
        }

        public static bool GetIsValidPassword(string password)
        {
            //at least 1 letter, 1 number, 1 special character, 8 characters long at least and no whitespaces
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^a-zA-Z\d])(?!.*\s).{8,}$";

            Debug.Log("Is " + password + " a valid password? " + Regex.IsMatch(password, pattern));

            return Regex.IsMatch(password, pattern);
        }

        public static bool GetNewPasswordIsValid(string password, string confirmPassword)
        {
            if (!GetIsValidPassword(password) || !GetIsValidPassword(confirmPassword))
            {
                Debug.Log("Password is NOT valid");

                return false;
            }
            else if (password != confirmPassword)
            {
                Debug.Log("Passwords do NOT match");

                return false;
            }

            return true;
        }

    }
}
