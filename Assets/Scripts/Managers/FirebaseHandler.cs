using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlTypes;
using System.Linq;
using Firebase.Auth;

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
        [SerializeField] private bool _isAdminAccount;

        [SerializeField] private static EncryptionKey _encryptionKey;


        public FirebaseApp App => _app;
        public DatabaseReference Database => _databaseReference;
        public bool IsConnected => _isConnected;

        public string LoggedID => _loggedID;
        public bool IsAdminAccount { get { return _isAdminAccount; } set { _isAdminAccount = value; } }


        private async void Awake()
        {
            InitSingleton();
            Authtentication();
            AttemptConnection();
            _encryptionKey  = Resources.Load<EncryptionKey>("EncryptionKey");            
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

        
        private async void Authtentication()
        {
            var auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser == null) {
                await auth.SignInAnonymouslyAsync();
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
                    //Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));

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

        private static async Task<T> AddValueToField<T>(string path, T value)
        {
            DatabaseReference database = Instance.Database.Child(path);

            try
            {
                // Push a new child node with a unique key
                await database.Push().SetValueAsync(value);

                return value;
            }
            catch (Exception exception)
            {
                Debug.LogError("Error adding user: " + exception);

                return default; // Return null to indicate failure
            }
        }

        private static async Task<string> AddChildToField(string path, string value)
        {
            DatabaseReference database = Instance.Database.Child(path);
            await database.SetRawJsonValueAsync(value);
            return value;
        }


        public static async Task<bool> GetCompanyExists(string company)
        {
            //Debug.Log("Does " + company + " exist? " + await CheckIfFieldExists("Companies" + "/" + company));

            return await CheckIfFieldExists("Companies" + "/" + company);
        }

        public static async Task<bool> GetAdminAccountCreated(string company)
        {
            if (!await CheckIfFieldExists("Companies" + "/" + company + "/" + "AdminCreated"))
            {
                //Debug.Log("Admin field for " + company + " does not exist. Creating it and setting it to false.");

                await SetAdminCreated(company, false);

                return false;
            }
            else
            {
                //Debug.Log("Has " + company + " admin account been created? " + await GetValueOfField<bool>("Companies" + "/" + company + "/" + "AdminCreated"));

                return await GetValueOfField<bool>("Companies" + "/" + company + "/" + "AdminCreated");
            }
        }

        public static async Task<bool> GetPasswordIsCorrect(string id, string password)
        {
            if (GetIsCompanyID(id))
            {
                if (!await CheckIfFieldExists("Companies" + "/" + id + "/" + "AdminCreated"))
                {
                    //Debug.Log("Password field for " + id + " does not exist. Creating it and setting it to an empty string.");

                    await SetPassword(id, string.Empty);

                    return false;
                }
                else
                {
                    //Debug.Log("Is password of " + id + " correct? " + await GetValueOfField<string>("Companies" + "/" + id + "/" + "Password") == password);

                    return await GetValueOfField<string>("Companies" + "/" + id + "/" + "Password") == CriptographPassword(password);
                }
            }
            else
            {
                if (!await CheckIfFieldExists("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id))
                {
                    //Debug.Log("Password field for " + id + " does not exist. Creating it and setting it to an empty string.");

                    await SetPassword(id, string.Empty);

                    return false;
                }
                else
                {
                    //Debug.Log("Is password of " + id + " correct? " + await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password") == password);

                    return await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password") == CriptographPassword(password);
                }
            }
        }

        public static async Task<bool> GetEmployeeExists(string id)
        {
            if (await GetEmployeeCompany(id) == null)
            {
                //Debug.Log("Company not valid");
                return false;
            }
            else
            {
                return await CheckIfFieldExists("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id);
            }
        }


        public static async Task<bool> SetPassword(string id, string password)
        {
            var criptographedPassword = CriptographPassword(password);
            if (GetIsCompanyID(id))
            {
                return await PushValueToField("Companies" + "/" + id + "/" + "Password", criptographedPassword) != default;
            }
            else
            {
                return await PushValueToField("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password", criptographedPassword) != default;
            }
        }

        public static async Task<bool> SetAdminCreated(string company, bool created)
        {
            return await PushValueToField<bool>("Companies" + "/" + company + "/" + "AdminCreated", created);
        }

        public static async Task<bool> SetNewAdminAccount(string company, string password)
        {
            if (await SetPassword(company, password))
            {
                if (await SetAdminCreated(company, true))
                {
                    //Debug.Log("Success. Admin account created for company " + company);

                    return true;
                }
                else 
                {
                    //Debug.Log("Something went wrong. Couldn't change the status of admin account created to true");

                    return false;
                }
            }
            else
            {
                //Debug.Log("Something went wrong. Couldn't set password");

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
            else if (await AddChildToField("Companies" + "/" + company + "/" + "Employees" + "/" + id,JsonUtility.ToJson( new Employee())) == default) return false;
            
            return true;
        }

        public static async Task<bool> AddNewCompany(string id, string displayName, int expiration, int maxEmployeeCount)
        {
            Company company = new Company()
            { 
                DisplayName = displayName,
                AdminCreated = false,
                Employees = new List<Employee>(),
                ExpirationDate = DateTime.UtcNow.AddMonths(expiration).ToString(),
                MaxEmployeeCount = maxEmployeeCount
            };

            if (!GetIsCompanyID(id)) return false;
            else if (await AddChildToField("Companies" + "/" + id,JsonUtility.ToJson( company )) == default) return false;
            
            return true;
        }


        private static async Task<List<string>> GetAllCompanies()
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
                    //Debug.LogWarning("No companies found in the database.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error fetching companies: " + ex);
            }

            return companyNames;
        }

        public static async Task<int> GetCompanyEmployeeCount()
        {
            DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            int employeeCount = 0;

            try
            {
                // Get the reference to the "Employees" field
                DatabaseReference employeesRef = databaseReference.Child("Companies" + "/" + Instance._companyCode + "/" + "Employees");

                // Retrieve the snapshot of the "Employees" field
                DataSnapshot employeesSnapshot = await employeesRef.GetValueAsync();

                // Check if the snapshot exists and has children
                if (employeesSnapshot != null && employeesSnapshot.Exists && employeesSnapshot.HasChildren)
                {
                    employeeCount = employeesSnapshot.Children.Count();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error fetching employees: " + ex);
            }

            return employeeCount;
        }

        public static async Task<string> GetEmployeeCompany(string id)
        {
            List<string> companies = await GetAllCompanies();

            Instance._companyCode = null;

            foreach (string company in companies)
            {
                string parsedCompanyName = GetPrefixInternal(company);
                string employeePrefix = id[..6];

                if (employeePrefix == parsedCompanyName)
                {
                    //Debug.Log("Employee " + id + " belongs to company " + company);

                    Instance._companyCode = company;

                    break;
                }
            }

            return Instance._companyCode;
        }

        public static async Task<int> GetEmployeeScore(string id)
        {
            return await GetValueOfField<int>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Score");
        }

        public static async Task<bool> SetEmployeeScore(string id, int score)
        {
            if( await GetEmployeeScore(id) > score) return true;
            return await PushValueToField("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Score", score) != default;
        }

        public static async Task<bool> AddEmployeeDayReport(string id, DayReport dayReport)
        {                                 
            return await AddChildToField("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "DayReports" + "/" + dayReport.Day, JsonUtility.ToJson( dayReport )) != default;
        }

        public static async Task<bool> AddAllEmployeeDayReports(string id, EmployeeSO employeeSO)
        {   
            foreach(DayReport dayReport in employeeSO.DayReportList)
            {
                if(! await AddEmployeeDayReport(id, dayReport)) return false;                
            }
            
            return true;
        }

        public static async Task<List<DayReport>> GetEmployeeDayReports(string id)
        {        
            DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            
            List<DayReport> dayReports = new List<DayReport>();

            try
            {
                // Get the reference to the "Employees" field
                DatabaseReference dayReportsRef = databaseReference.Child("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "DayReports");

                // Retrieve the snapshot of the "Employees" field
                DataSnapshot dayReportsSnapshot = await dayReportsRef.GetValueAsync();

                // Check if the snapshot exists and has children
                if (dayReportsSnapshot != null && dayReportsSnapshot.Exists && dayReportsSnapshot.HasChildren)
                {
                     foreach(var dayReportSnapshot in dayReportsSnapshot.Children)
                    {
                        int day;
                        Int32.TryParse(dayReportSnapshot.Key, out day);

                        int score;
                        Int32.TryParse(dayReportSnapshot.Child("_score").Value.ToString(), out score);                        

                        List<TrackableAction> actions = new List<TrackableAction>();

                        foreach(var actionSnapshot in dayReportSnapshot.Child("_actions").Children)
                        {
                            string action = actionSnapshot.Child("_action").Value.ToString();
                            
                            bool performed = actionSnapshot.Child("_performed").Value.ToString() == "True";

                            actions.Add(new TrackableAction(action, performed));
                        }

                        dayReports.Add(new DayReport(day, score, actions));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error fetching employees: " + ex);
            }

            //return JsonUtility.FromJson<List<DayReport>>( await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "DayReports"));
            return dayReports;
        }

        public static async Task<bool> GetIsEmployeeFirstLogin(string id)
        {
            //Debug.Log(await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id));
            return (await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password") == null ||
                await GetValueOfField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password") == "");
        }

        public static async Task<bool> GetIsCompanyExpired(string id)
        {
            DateTime expirationDate = DateTime.Parse(await GetValueOfField<string>("Companies" + "/" + id + "/" + "ExpirationDate"));
            
            return DateTime.Compare(expirationDate, DateTime.UtcNow) < 0;
        }

        public static async Task<int> GetCompanyMaxEmployeeCount()
        {
            return await GetValueOfField<int>("Companies" + "/" + Instance._companyCode + "/" + "MaxEmployeeCount");
        }

        public static async Task<bool> SetEmployeePassword(string id, string password)
        {
            var criptographedPassword = CriptographPassword(password);
            return await PushValueToField<string>("Companies" + "/" + Instance._companyCode + "/" + "Employees" + "/" + id + "/" + "Password", criptographedPassword) != default;
        }


        public static bool GetIsCompanyID(string code)
        {
            //14 CNPJ            //3 letters            
            string pattern = @"^\d{14}[A-Za-z]{3}";

            //Debug.Log("Is " + code + " a company valid id? " + Regex.IsMatch(code, pattern));

            return Regex.IsMatch(code, pattern);
        }

        public static bool GetIsEmployeeID(string code)
        {
            bool valid = false;

            if (code.Length < 6) return valid;

            for (int i = 0; i < 3; i++)
            {
                if (!char.IsDigit(code[i])) return valid;
            }

            for (int i = 3; i < 6; i++)
            {
                if (!char.IsLetter(code[i])) return valid;
            }

            valid = true;

            //Debug.Log("Is " + code + " an employee valid id? " + valid);
            return valid;
        }

        public static string GetCompanyPrefix()
        {
            //Debug.Log(GetPrefixInternal(Instance._companyCode));
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


        public static bool GetIsValidPassword(string password)
        {
            //at least 1 letter, 1 number, 1 special character, 8 characters long at least and no whitespaces
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^a-zA-Z\d])(?!.*\s).{8,}$";

            //Debug.Log("Is " + password + " a valid password? " + Regex.IsMatch(password, pattern));

            return Regex.IsMatch(password, pattern);
        }

        public static bool GetNewPasswordIsValid(string password, string confirmPassword)
        {
            if (!GetIsValidPassword(password) || !GetIsValidPassword(confirmPassword))
            {
                //Debug.Log("Password is NOT valid");

                return false;
            }
            else if (password != confirmPassword)
            {
                //Debug.Log("Passwords do NOT match");

                return false;
            }

            return true;
        }

        public static string CriptographPassword(string password)
        {
            var key = _encryptionKey.Key;
            
            return AesOperation.EncryptString(key, password);            
        }

        public static string DecriptographPassword(string password)
        {
            var key = _encryptionKey.Key;

            return AesOperation.DecryptString(key, password);;
        }
    }
}

[Serializable]
public class Employee
{
    public string DisplayName;
    public string Password;    
    public int Score;
}

[Serializable]
public class Company
{
    public bool AdminCreated;
    public string DisplayName;
    public string Password;    
    public List<Employee> Employees;
    public string ExpirationDate;
    public int MaxEmployeeCount;
}