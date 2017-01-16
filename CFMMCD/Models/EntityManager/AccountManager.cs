using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                if (account.Username == null || account.Username.Equals(""))
                    return false;
                if (account.AccountId != null && !account.AccountId.Equals(""))
                {
                    accRow = db.Accounts.Single(o => o.UserId.ToString().Equals(account.AccountId));
                    // Password
                    if (account.Password == null || account.Password.Equals("")) // If password is unchanged
                        accRow.Password = db.Accounts.Single(o => o.UserId.ToString().Equals(account.AccountId)).Password;
                    else
                    {
                        string keys = GenerateKeys(15);
                        string pass = keys + EncodePassword(account.Password, keys);
                        accRow.Password = pass;
                    }
                }
                else
                {
                    accRow.UserId = int.Parse(DateTime.Now.ToString("yyMMdd")) + new Random().Next(999) + new Random().Next(999); // To be changed soon
                    // Create new password
                    string keys = GenerateKeys(15);
                    string pass = keys + EncodePassword(account.Password, keys);
                    accRow.Password = pass;
                }
                // Set user name
                accRow.Username = account.Username;
                // Set User access
                accRow.UserAccess = "";
                accRow.UserAccess = new AccountManager().GetUserAccessString(account);
                if (!accRow.UserAccess.Contains("True"))
                    accRow.UserAccess = account.UserAccess;
                if (account.AllExceptUAC)
                    accRow.UserAccess = "True,True,True,True,True,True,True,False,True,True,True,True,True,True,True,True,True,True";

                try
                {
                    if (!db.Accounts.Where(o => o.Username.Equals(account.OldUsername)).Any())
                    {
                        db.Accounts.Add(accRow);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
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
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        } 
        /*
         * Check if given
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
        public bool CheckPassword(string username, string password)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                // Get `Username` that matches the specified username parameter
                var user = db.Accounts.Where(o => o.Username.Equals(username));
                if (user.Any())
                {
                    string pass = user.FirstOrDefault().Password;
                    if (user.FirstOrDefault().Password.Equals(password))
                        return true;
                    string key = pass.Substring(0, 15);
                    pass = pass.Substring(15);
                    string encodedPass = EncodePassword(password, key);
                    if (encodedPass.Equals(pass))
                        return true;
                    else return false;
                }
                else
                    return false;
            }
        }

        public static bool LogDateTime(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (db.Accounts.Where(o => o.Username.Trim().Equals(username)).Any())
                {
                    Account ac = db.Accounts.SingleOrDefault(o => o.Username.Trim().Equals(username));
                    ac.TimeLastLogged = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<string> SearchAccounts (string username)
        {
            using ( CFMMCDEntities db = new CFMMCDEntities())
            {
                List<string> AList = new List<string>();
                List<string> AIdList = new List<string>();
                List<Account> ARowList;
                if (username.ToUpper().Equals("ALL"))
                {
                    ARowList = db.Accounts.ToList();
                }
                else if (db.Accounts.Where( o => o.Username.Equals(username)).Any())
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
                        AList.Add(a.Username);
                        AIdList.Add(a.UserId.ToString());

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
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return null;
                }
            }
        }

        public AccountViewModel SearchSingleAccount(string Username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Account ARow = new Account();
                if (db.Accounts.Where(o => o.Username.Equals(Username)).Any())
                {
                    ARow = db.Accounts.Single(o => o.Username.Equals(Username));
                }
                else
                {
                    return null;
                }
                AccountViewModel vm = new AccountViewModel();
                vm.Username = ARow.Username.Trim();
                vm.Password = ARow.Password.Trim();
                vm.UserAccess = ARow.UserAccess.Trim();
                vm.OldUsername = ARow.Username.Trim();
                vm.AccountId = ARow.UserId.ToString();

                bool[] UserAccessArr = GetUserAccessArray(ARow.Username);
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
                return vm;
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


        // Encryption
        // From http://www.c-sharpcorner.com/UploadFile/145c93/save-password-using-salted-hashing/
        public static string GenerateKeys(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i <= length - 1; i++)
            {
                int ind = Convert.ToInt32((allowedChars.Length) * randNum.NextDouble());
                while (ind >= allowedChars.Length)
                    ind = Convert.ToInt32((allowedChars.Length) * randNum.NextDouble());
                chars[i] = allowedChars[ind];
            }
            return new string(chars);
        }

        public static string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);

            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }

        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }

        public static string base64Encode(string sData) // Encode    
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                Exception f = e.InnerException;
                while (f != null)
                {
                    System.Diagnostics.Debug.WriteLine("INNER:");
                    System.Diagnostics.Debug.WriteLine(f.Message);
                    System.Diagnostics.Debug.WriteLine(f.Source);
                    f = f.InnerException;
                }
                System.Diagnostics.Debug.WriteLine(e.Data);
                return null;
            }
        }

        public static string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                Exception f = e.InnerException;
                while (f != null)
                {
                    System.Diagnostics.Debug.WriteLine("INNER:");
                    System.Diagnostics.Debug.WriteLine(f.Message);
                    System.Diagnostics.Debug.WriteLine(f.Source);
                    f = f.InnerException;
                }
                System.Diagnostics.Debug.WriteLine(e.Data);
                return null;
            }
        }
    }
}