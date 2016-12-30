using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class AccountManager
    {
        /*
         * Creates a new `Accounts` row and inserts it in the table
         */
        public bool UpdateUserAccount(AccountViewModel account)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Account accRow = new Account();
                accRow.UserId = int.Parse(DateTime.Now.ToString("yyMMdd")) + new Random().Next(999) + new Random().Next(999); // To be changed soon
                accRow.Username = account.Username;
                if (account.Password == null || account.Password.Equals(""))
                    accRow.Password = GetPassword(account.OldUsername);
                else
                    accRow.Password = account.Password;
                accRow.UserAccess = "";
                accRow.UserAccess = new AccountManager().GetUserAccessString(account);
                if (!accRow.UserAccess.Contains("True"))
                    accRow.UserAccess = account.UserAccess;
                if (account.AllExceptUAC)
                    accRow.UserAccess = "True,True,True,True,True,True,True,False,True,True,True,True,True,True,True,True,True,True"; 
                try
                {
                    if (db.Accounts.Where(o => o.Username.Equals(account.OldUsername)).Any())
                    {
                        Account RowToDelete = db.Accounts.Single(o => o.Username.Equals(account.OldUsername));
                        Console.WriteLine("PASSED: Searched existing row");
                        db.Accounts.Remove(RowToDelete);
                        Console.WriteLine("PASSED: Deleted existing row");
                        db.Accounts.Add(accRow);
                        Console.WriteLine("PASSED: Added update row");
                    }
                    else
                    {
                        db.Accounts.Add(accRow);
                        Console.WriteLine("PASSED: Added new row");
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    foreach (var eve in ((DbEntityValidationException)e).EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    return false;
                }
            }
        }
        public bool DeleteUserAccount(AccountViewModel AViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Account ARow = new Account();
                if (db.Accounts.Where(o => o.Username.Equals(AViewModel.Username)).Any())
                    ARow = db.Accounts.Single(o => o.Username.Equals(AViewModel.Username));
                else
                    return false;
                // Try...Catch to produce appropriate warnings if ever
                // insertion is unsuccessful
                try
                {
                    db.Accounts.Remove(ARow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        } 
        /*
         * Used in AccountController to check whether given
         * username is available
         * 
         * Returns false if available (Username does not exist)
         */ 
        public bool IsUsernameExist(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                return db.Accounts.Where(o => o.Username.Equals(username)).Any();
            }
        }
        /*
         * Gets password for the username parameter
         * 
         * Returns empty string if username does not exist
         */
        public string GetPassword(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                // Get `Username` that matches the specified username parameter
                var user = db.Accounts.Where(o => o.Username.Equals(username));
                if (user.Any())
                    return user.FirstOrDefault().Password;
                else
                    return "";
            }
        }

        public List<AccountViewModel> SearchAccount (string username)
        {
            using ( CFMMCDEntities db = new CFMMCDEntities())
            {
                List<AccountViewModel> AList = new List<AccountViewModel>();
                List<Account> ARowList;
                if (db.Accounts.Where( o => o.Username.Equals(username)).Any())
                {
                    ARowList = db.Accounts.Where(o => o.Username.Equals(username)).ToList();
                }
                else
                {
                    return null;
                }
                try
                {
                    foreach ( Account a in ARowList )
                    {
                        AccountViewModel vm = new AccountViewModel();
                        vm.Username = a.Username.Trim();
                        vm.Password = a.Password.Trim();
                        vm.UserAccess = a.UserAccess.Trim();
                        vm.OldUsername = a.Username.Trim();

                        bool[] UserAccessArr = GetUserAccessArray(a.Username);
                        vm.MIMInput = UserAccessArr[0];
                        vm.RIMInput = UserAccessArr[1];
                        vm.MERInput = UserAccessArr[2];
                        vm.STPInput = UserAccessArr[3];
                        vm.SCMInput = UserAccessArr[4];
                        vm.VEMInput = UserAccessArr[5];
                        vm.VAMInput = UserAccessArr[6];
                        vm.UAPInput = UserAccessArr[7];
                        vm.MIPInput = UserAccessArr[8];
                        vm.RIPInput = UserAccessArr[9];
                        vm.AULInput = UserAccessArr[10];
                        vm.REGInput = UserAccessArr[11];
                        vm.TEGInput = UserAccessArr[12];
                        vm.TIPInput = UserAccessArr[13];
                        vm.BUEInput = UserAccessArr[14];
                        vm.OWNInput = UserAccessArr[15];
                        vm.PRCInput = UserAccessArr[16];
                        vm.LOCInput = UserAccessArr[17];
                        AList.Add(vm);
                    }
                    if (AList == null || AList.ElementAt(0) == null)
                        return null;
                    return AList;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return null;
                }
            }
        }



        public int GetUserID(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                // Get `Username` that matches the specified username parameter
                var user = db.Accounts.Where(o => o.Username.Equals(username));
                if (user.Any())
                    return user.FirstOrDefault().UserId;
                else
                    return -1;
            }
        }

        /*
* Creates a string list of the current logged account's accessible pages
* listed as boolean delimited by commas.
* 
* Returns the string created
*/
        private string GetUserAccessString(AccountViewModel account)
        {
            return (account.MIMInput.ToString() + "," + account.RIMInput.ToString() + "," + account.MERInput.ToString() + "," + account.STPInput.ToString() + "," + account.SCMInput.ToString() + "," + account.VEMInput.ToString() + "," + account.VAMInput.ToString() + "," + account.UAPInput.ToString() + "," + account.MIPInput.ToString() + "," + account.RIPInput.ToString() + "," + account.AULInput.ToString() + "," + account.REGInput.ToString() + "," + account.TEGInput.ToString() + "," + account.TIPInput.ToString() + "," + account.BUEInput.ToString() + "," + account.OWNInput.ToString() + "," + account.PRCInput.ToString() + "," + account.LOCInput.ToString());
        }
        /*
         * Gets `UserAcces` string from `Accounts` and converts it into
         * boolean array.
         * 
         * Returns array with value all false if username parameter does not match
         * any `Username`
         */
        public bool[] GetUserAccessArray(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                bool[] userAccessArray;
                var user = db.Accounts.Where(o => o.Username.Equals(username));
                if (user.Any())
                {
                    string userAccessString = user.FirstOrDefault().UserAccess;
                    string[] userAccessStringArray = userAccessString.Split(',');
                    userAccessArray = new bool[userAccessStringArray.Length];
                    for (int i = 0; i < userAccessStringArray.Length; i++)
                        userAccessArray[i] = Boolean.Parse(userAccessStringArray[i]);
                }
                else
                {
                    userAccessArray = new bool[18];
                    for (int i = 0; i < 18; i++)
                        userAccessArray[i] = false;
                }
                return userAccessArray;
            }
        }
        public UserAccessSession SetUserAccess(string username)
        {
            bool[] UAArray = GetUserAccessArray(username);
            UserAccessSession UASession = new UserAccessSession();
            UASession.MIM = UAArray[0];
            UASession.RIM = UAArray[1];
            UASession.MER = UAArray[2];
            UASession.STP = UAArray[3];
            UASession.SCM = UAArray[4];
            UASession.VEM = UAArray[5];
            UASession.VAM = UAArray[6];
            UASession.UAP = UAArray[7];
            UASession.MIP = UAArray[8];
            UASession.RIP = UAArray[9];
            UASession.AUL = UAArray[10];
            UASession.REG = UAArray[11];
            UASession.TEG = UAArray[12];
            UASession.TIP = UAArray[13];
            UASession.BUE = UAArray[14];
            UASession.OWN = UAArray[15];
            UASession.PRC = UAArray[16];
            UASession.LOC = UAArray[17];
            return UASession;
        }
    }
}