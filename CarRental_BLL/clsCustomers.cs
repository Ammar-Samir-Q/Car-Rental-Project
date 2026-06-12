using CarRental_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_BLL
{
    public class clsCustomers
    {
        enum enMode { AddNew=1,Update=2}
        public int CustomerID { get; set; }
        
        public int PersonID { get; set; }

        public clsPeople PersonInfo {  get; }

        public DateTime CreationDate { get; set; }

        public int CreatedByUserID { get; set; }

        public string DriverLicnsesNumber { get; set; }

        private enMode _Mode = enMode.AddNew;   

        //----------------------------------
      public clsCustomers()
        {
            this.CustomerID = -1;
            this.PersonID= -1;
            this.CreationDate = DateTime.Now;
            this.DriverLicnsesNumber = "";
            this.CreatedByUserID = -1;
            this._Mode= enMode.AddNew;
        }

       public  clsCustomers(int CustomerID,int PersonID,DateTime CreationDate,string DriverLicenseNumber,int CreatedByUserID)
        {
            this.CustomerID=CustomerID;
            this.PersonID=PersonID;
            this.PersonInfo = clsPeople.GetPerson(PersonID);
            this.CreationDate=CreationDate;
            this.DriverLicnsesNumber=DriverLicenseNumber;
            this.CreatedByUserID=CreatedByUserID;
            this._Mode = enMode.Update;
        }

        //-----------------------------------

        private bool _AddCustomer()
        {
            this.CustomerID = clsCustomerData.AddCustomer(PersonID, CreationDate, CreatedByUserID, DriverLicnsesNumber);
            return this.CustomerID > 0;
        }

        private bool _UpdateCustomer()
        {
            return clsCustomerData.UpdateCustomer(CustomerID, PersonID, CreationDate, CreatedByUserID, DriverLicnsesNumber);
        }

        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddCustomer();
                case enMode.Update:
                    return _UpdateCustomer();
            }
            return false;   
        }

        public static clsCustomers GetCustomerByID(int CustomerID)
        {
            int PersonID = -1, CreatedByUserID = -1;
            string DriverLicenseNumber = "";
            DateTime CreationDate = DateTime.Now;
            if (clsCustomerData.GetCustomerByID(CustomerID, ref PersonID, ref CreationDate, ref CreatedByUserID, ref DriverLicenseNumber))
            {
                return new clsCustomers(CustomerID, PersonID, CreationDate, DriverLicenseNumber, CreatedByUserID);
            }
            else
                return null;

        }

        public static clsCustomers GetCustomerByPersonID(int PersonID)
        {
            int CustomerID = -1, CreatedByUserID = -1;
            string DriverLicenseNumber = "";
            DateTime CreationDate = DateTime.Now;
            if (clsCustomerData.GetCustomerByPersonID(PersonID, ref CustomerID, ref CreationDate, ref CreatedByUserID, ref DriverLicenseNumber))
            {
                return new clsCustomers(CustomerID, PersonID, CreationDate, DriverLicenseNumber, CreatedByUserID);
            }
            else
                return null;

        }

        public static bool DeleteCustomer(int CustomerID)
        {
            return clsCustomerData.DeleteCustomer(CustomerID);
        }

        public bool Delete()
        {
            return clsCustomerData.DeleteCustomer(this.CustomerID);
        }

        public static DataTable GetAllCustomers()
        {
            return clsCustomerData.GetAllCustomers();
        }

        public static DataTable GetCustomerHistory(int CustomerID)
        {
            return clsCustomerData.GetAllBookingsForCustomer(CustomerID);
        }

        public static bool IsCustomerExists(int CustomerID)
        {
            return clsCustomerData.IsCustomerExists(CustomerID);
        }

        public static bool IsCustomerExists(string DriverLicneseNumber)
        {
            return clsCustomerData.IsCustomerExists(DriverLicneseNumber);
        }


        public static bool IsCustomerExistsByPersonID(int PersonID)
        {
            return clsCustomerData.IsCustomerExistsByPersonID(PersonID);
        }


        public bool IsExists()
        {
            return clsCustomerData.IsCustomerExists(this.CustomerID);
        }

        public static int GetTotalRentTimes(int CustomerID)
        {
            return clsCustomerData.GetTotalRentTimes(CustomerID);
        }

        public  int GetTotalRentTimes()
        {
            return clsCustomerData.GetTotalRentTimes(this.CustomerID);
        }

        public static DateTime GetLastRentDate(int CustomerID)
        {
            return clsCustomerData.GetLastRentDate(CustomerID);
        }

        public  DateTime GetLastRentDate()
        {
            return clsCustomerData.GetLastRentDate(CustomerID);
        }

        public static int GetTotalCustomers()
        {
            return clsCustomerData.GetTotalCustomers();
        }

    }
}
