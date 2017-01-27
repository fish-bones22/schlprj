using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class DecryptController : Controller
    {
        // GET: Decrypt
        public ActionResult Index()
        {
            Session["CurrentPage"] = new CurrentPageSession("DEV", "HOME", "LOG");
            return View();
        }
        [HttpPost]
       public ActionResult Index(string key)
        {
            if (key != null)
            {
                 if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files["FileUploaded"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string path = @"C:/SMS/SMSWEBCFM/";

                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);


                        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                        //A 64 bit key and IV is required for this provider.
                        //Set secret key For DES algorithm.
                        DES.Key = Encoding.ASCII.GetBytes(key);
                        //Set initialization vector.
                        DES.IV = Encoding.ASCII.GetBytes(key);

                        //Create a file stream to read the encrypted file back.
                        FileStream fsread = new FileStream(path + file.FileName, FileMode.Open, FileAccess.Read);
                        //Create a DES decryptor from the DES instance.
                        ICryptoTransform desdecrypt = DES.CreateDecryptor();
                        //Create crypto stream set to read and do a 
                        //DES decryption transform on incoming bytes.
                        CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
                        //Print the contents of the decrypted file.
                        StreamWriter fsDecrypted = new StreamWriter(path + "dec-" + file.FileName);
                        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                        fsDecrypted.Flush();
                        fsDecrypted.Close();
                    }
                }
            }
            return View();
        }
    }
}