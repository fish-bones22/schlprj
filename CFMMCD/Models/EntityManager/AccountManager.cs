using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class AccountManager
    {
        /*
         * Creates a new `Accounts` row and inserts it in the table
         */
        public void CreateUserAccount(CreateAccountViewModel account)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Account accRow = new Account();

                accRow.UserId = (int) DateTime.Now.Ticks;
                accRow.Username = account.Username;
                accRow.Password = account.Password;
                accRow.UserAccess = GetUserAccessString(account);

                db.Accounts.Add(accRow);
                db.SaveChanges();
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
        private string GetUserAccessString(CreateAccountViewModel account)
        {
            return account.MIMInput + "," +
                   account.RIMInput + "," +
                   account.MERInput + "," +
                   account.STPInput + "," +
                   account.SCMInput + "," +
                   account.VENInput + "," +
                   account.VAMInput + "," +
                   account.UAPInput + "," +
                   account.MIPInput + "," +
                   account.RIPInput + "," +
                   account.AULInput + "," +
                   account.REGInput + "," +
                   account.TEGInput + "," +
                   account.TIPInput + "," +
                   account.BUEInput + "," +
                   account.OWNInput + "," +
                   account.PRCInput + "," +
                   account.LOCInput;
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
            UASession.VEN = UAArray[5];
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