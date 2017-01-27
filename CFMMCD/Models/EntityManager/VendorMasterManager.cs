using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class VendorMasterManager
    {
        /*
         * Combined Create and Update Menu Item method.
         * Creates a INVVEMP0 instance (which will be a new table row)
         * and instantiates each VMViewModel property to the respective property of the former.
         * Also checks if the given Vendor Number is already in the table,
         * if true, the method performs an update, otherwise, creation. 
         *
         * Returns true if the operation is successful.
         * */
        public static bool UpdateVendor( VendorMasterViewModel VMViewModel, string user )
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                INVVEMP0 VMRow;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any())
                    VMRow = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));

                else
                    VMRow = new INVVEMP0();

                if (VMViewModel.VEMVEN != null)
                    VMRow.VEMVEN = int.Parse(VMViewModel.VEMVEN);
                else return false;

                if (db.INVVEMP0.Where(o => (!o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN) &&  o.VEMDS1.Equals(VMViewModel.VEMDS1))).Any())
                    return false;

                if (VMViewModel.VEMDS1 != null)
                    VMRow.VEMDS1 = VMViewModel.VEMDS1.Trim();     // Trim() makes sure no additional whitespace at the start end end of the string
                else return false;

                if (VMViewModel.VEMLOC != null)
                    VMRow.VEMLOC = VMViewModel.VEMLOC.Trim();

                if (VMViewModel.Region != null)
                    VMRow.Region = VMViewModel.Region;

                if (VMViewModel.Province != null)
                    VMRow.Province = VMViewModel.Province;

                if (VMViewModel.City != null)
                    VMRow.City = VMViewModel.City;

                VMRow.Store = VMViewModel.Store;
                if (VMViewModel.SelectAllCb)
                {
                    VMRow.Store = "ALL";
                }
                if (VMViewModel.SelectExceptCb)
                {
                    VMRow.Store = "ALL";
                    VMRow.Except_Store = VMViewModel.Store.Trim();
                }
                else
                {
                    VMRow.Except_Store = null;
                }

                VMRow.VEMDAT = DateTime.Now;

                VMRow.VEMUSR = user.Substring(0, 3).ToUpper();

                try
                {    // Perform an update if Vendor number already exists
                    if ( db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any() )
                    {
                        VMRow.STATUS = "E";
                    }
                    else
                    {
                        VMRow.STATUS = "A";
                        db.INVVEMP0.Add(VMRow);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
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
         * Deletes the specified row given in the ViewModel properties.
         * 
         * Returns true if operation is successful.
         * */
        public static bool DeleteVendor( VendorMasterViewModel VMViewModel )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                try
                {
                    if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any())
                    {
                        INVVEMP0 rowToDelete = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));
                        db.INVVEMP0.Remove(rowToDelete);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch ( Exception e )
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

        public static List<VendorMasterViewModel> SearchVendors ( string SearchItem )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<VendorMasterViewModel> VMList = new List<VendorMasterViewModel>();
                List<INVVEMP0> VMRowList;

                if ( SearchItem == null || SearchItem.Equals("") )
                    return null;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).ToList();
                else if (SearchItem.ToUpper().Equals("ALL"))
                    VMRowList = db.INVVEMP0.ToList();
                else if (db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(SearchItem)).ToList();
                else
                    return null;
                foreach ( INVVEMP0 VMRow in VMRowList )
                {
                    VendorMasterViewModel vm = SearchSingleVendor(VMRow.VEMVEN.ToString());
                    if (vm != null)
                        VMList.Add(vm);
                }
                if (VMList == null || VMList.ElementAt(0) == null)
                    return null;
                return VMList;
            }
        }

        public static VendorMasterViewModel SearchSingleVendor( string SearchItem )
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                INVVEMP0 VMRow;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).Any())
                    VMRow = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(SearchItem));
                else
                    return null;
                VendorMasterViewModel vm = new VendorMasterViewModel();
                if (VMRow.VEMVEN != 0)
                    vm.VEMVEN = VMRow.VEMVEN.ToString();
                if (VMRow.VEMDS1 != null)
                    vm.VEMDS1 = VMRow.VEMDS1.Trim();
                if (VMRow.VEMLOC != null)
                    vm.VEMLOC = VMRow.VEMLOC;
                if (VMRow.Region != null)
                    vm.Region = VMRow.Region;
                if (VMRow.Province != null)
                    vm.Province = VMRow.Province;
                if (VMRow.City != null)
                    vm.City = VMRow.City;

                vm.Store = VMRow.Store;
                if ((VMRow.Store != null) && VMRow.Store.Equals("ALL"))
                {
                    vm.SelectAllCb = true;
                    vm.Store = "";
                }
                if ((VMRow.Except_Store != null) && !(VMRow.Except_Store.Equals("")))
                {
                    vm.SelectExceptCb = true;
                    vm.SelectAllCb = false;
                    vm.Store = VMRow.Except_Store;
                }
                return vm;
            }
        }

        public static ReportViewModel ImportExcel(Stream file, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                ReportViewModel error = new ReportViewModel();
                XLWorkbook workBook;
                try
                {
                    workBook = new XLWorkbook(file);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    error.Result = false;
                    error.Message = "File format not supported";
                    error.ErrorLevel = 3;
                    return error;
                }
                IXLWorksheet workSheet = workBook.Worksheet(1);
                var MIMRowList = new List<CSHMIMP0>();
                bool IsFirstRow = true;
                int succesfulRows = 0;
                int blankCounter = 0;
                IXLRow FirstRow = workSheet.Rows().ElementAt(0);
                if (FirstRow == null || FirstRow.CellCount() <= 0)
                {
                    error.Result = false;
                    error.ErrorLevel = 3;
                    error.Message = "File has incorrect or unsupported format";
                    return error;
                }
                int index = 1;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (row == null)
                        break;

                    if (IsFirstRow)
                    {
                        FirstRow = row;
                        IsFirstRow = false;
                    }
                    else
                    {
                        if (row.Cells() == null || row.CellCount() <= 0)
                            break;

                        INVVEMP0 VRow = new INVVEMP0();
                        int errorLevel = 0;
                        for (int i = 1; i < row.CellCount(); i++)
                        {
                            System.Diagnostics.Debug.WriteLine("Cell count: " + i);
                            System.Diagnostics.Debug.WriteLine("Cell header: " + FirstRow.Cell(i).Value.ToString().ToUpper());
                            System.Diagnostics.Debug.WriteLine("Cell data: " + row.Cell(i).Value.ToString());
                            System.Diagnostics.Debug.WriteLine("Row: " + index);

                            if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMVEN") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("VENDOR NUMBER") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("VENDOR #"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMVEN = int.Parse(row.Cell(i).Value.ToString());
                                else
                                {
                                    error.Message += "Vendor number [" + row.Cell(i).Value.ToString() + "] at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                    break;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMDS1") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DESCRIPTION 1"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMDS1 = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Result = false;
                                    error.Message += "Description 1 [" + row.Cell(i).Value.ToString() + "]  at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMDS2") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DESCRIPTION 2"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMDS2 = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMWSI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("INTERNATIONAL WSI-NUMBER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMWSI = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMCCD") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("COUNTRY CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMCCD = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMZIP") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ZIP CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMZIP = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMCTY") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CITY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMCTY = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMSTR") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STREET"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMSTR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMTEL") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("TELEPHONE NUMBER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMTEL = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMSTN") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SHORT TELEPHONE NUMBER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMSTN = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMLOC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("LOCAL VENDOR"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMLOC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMDAY") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MINIMUM STOCK BUFFER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMDAY = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMTID") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("TELEPHONE ID"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMTID = row.Cell(i).Value.ToString();
                            }


                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMSTA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STATUS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMSTA = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMDAT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CREATION/CHANGE DATE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMDAT = DateTime.Parse(row.Cell(i).Value.ToString());
                                else
                                    VRow.VEMDAT = DateTime.Now;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMUSR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMUSR = row.Cell(i).Value.ToString();
                                else
                                    VRow.VEMUSR = user.ToUpper().Substring(0, 3);
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMADE") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("AUTO DELIVERY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMADE = row.Cell(i).Value.ToString(); ;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("VEMDEL") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SUPPLY CHAIN VENDOR"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.VEMDEL = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("REGION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.Region = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("PROVINCE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.Province = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("CITY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.City = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("STORE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    VRow.Store = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value == null ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains(""))
                            {
                                blankCounter++;
                                if (blankCounter > 20) break;
                                else continue;
                            }

                        }

                        if (VRow.VEMVEN == 0 || VRow.VEMDS1 == null)
                        {
                            error.Result = false;
                            error.Message += "{Row " + index + "} has incorrect format | ";
                            errorLevel = 2;
                            continue;
                        }

                        if (db.INVVEMP0.Where(o => o.VEMVEN == VRow.VEMVEN).Any())
                        {
                            error.Result = false;
                            error.Message += "Vendor [" + VRow.VEMVEN + "] at {Row " + index + "} is already defined | ";
                            errorLevel = 2;
                        }

                        if (VRow.VEMLOC != null && !VRow.VEMLOC.Equals("") && !db.LOCATIONs.Where(o => o.Id.ToString().Equals(VRow.VEMLOC)).Any())
                        {
                            error.Result = false;
                            error.Message += "Location with Id [" + VRow.VEMLOC + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (VRow.Store != null && !VRow.Store.Equals("") && !db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(VRow.Store)).Any())
                        {
                            error.Result = false;
                            error.Message += "Store with Id [" + VRow.VEMLOC + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (errorLevel >= 2)
                            error.Message += "{Row " + index + "} not inserted | ";

                        if (errorLevel < 2)
                        {
                            db.INVVEMP0.Add(VRow);
                            try
                            {
                                db.SaveChanges();
                                // Special case for logging import 
                                succesfulRows++;
                                new AuditLogManager().Audit(user, DateTime.Now, "Vendor Master", "Import", VRow.VEMVEN.ToString(), VRow.VEMDS1);
                                System.Diagnostics.Debug.WriteLine(VRow.VEMVEN);
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
                                error.Result = false;
                                error.Message += "{Row " + index + "} failed to insert. | \n";
                                errorLevel = 2;
                            }
                        }
                        error.ErrorLevel = errorLevel;
                        index++;
                    }

                }
                if (succesfulRows <= 0)
                {
                    error.Result = false;
                    error.Message += "No rows imported | ";
                    error.ErrorLevel = 3;
                }
                else if (succesfulRows >= index)
                {
                    error.ErrorLevel = 0;
                    error.Result = true;
                }
                error.Message += "Imported " + index + " rows. ";
                return error;
            }
        }
    }
}