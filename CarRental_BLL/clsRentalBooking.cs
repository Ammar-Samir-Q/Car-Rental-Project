using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental_DAL;
namespace CarRental_BLL
{
    public class clsRentalBooking
    {
        enum enMode { AddNew = 1, Update =2}

        public int BookingID { get; set; }

        public int CustomerID { get; set; }

        public clsCustomers CustomerInfo {  get; set; }

        public int VehicleID { get; set; }

        public clsVehicle VehicleInfo { get; set; }

        public DateTime RentalStartDate { get; set; }

        public DateTime RentalEndDate { get; set; }

        public string PickupLocation { get; set; }

        public string DropOffLocation { get; set; }

        public float RentalPricePerDay { get; set; }

        public string InitialCheckNotes {  get; set; }

        public int CreatedByUserID { get; set; }

        public int TotalDays { get { return (RentalEndDate.Date - RentalStartDate.Date).Days; } }

        public float InitialTotalDueAmount { get { return RentalPricePerDay * TotalDays; } }

        private enMode _Mode=enMode.AddNew; 

        //------------------------------------------------
        public clsRentalBooking()
        {
            this.BookingID = -1;
            this.CustomerID = -1;
            this.VehicleID = -1;
            this.RentalStartDate = DateTime.Now;
            this.RentalEndDate = DateTime.Now;
            this.PickupLocation = "";
            this.DropOffLocation = "";
            this.RentalPricePerDay = 0;
            this.InitialCheckNotes = "";
            this.CreatedByUserID = -1;
            this._Mode = enMode.AddNew;
        }
        
        public clsRentalBooking(int BookingID,int CustomerID,int VehicleID,DateTime RentalStartDate,DateTime RentalEndDate,string PickupLocation,string DropOffLocation,float RentalPricePerDay,string InitialCheckNotes,int CreatedByUserID)
        {
            this.BookingID=BookingID;
            this.CustomerID=CustomerID;
            this.CustomerInfo=clsCustomers.GetCustomerByID(CustomerID);
            this.VehicleID=VehicleID;
            this.VehicleInfo=clsVehicle.GetVehicleByID(VehicleID);
            this.RentalStartDate=RentalStartDate;
            this.RentalEndDate=RentalEndDate;
            this.PickupLocation=PickupLocation;
            this.DropOffLocation=DropOffLocation;
            this.RentalPricePerDay=RentalPricePerDay;
            this.InitialCheckNotes=InitialCheckNotes;
            this.CreatedByUserID=CreatedByUserID;

            this._Mode = enMode.Update;

        }
    
        //------------------------------------------------
    
        private bool _AddRent()
        {
            this.BookingID = clsRentalBookingData.AddRent(CustomerID, VehicleID, RentalStartDate, RentalEndDate, PickupLocation, DropOffLocation, RentalPricePerDay, InitialCheckNotes, CreatedByUserID);
            return this.BookingID > 0;
        }
    
        private bool _UpdateRent()
        {
            return clsRentalBookingData.UpdateRentalBooking(BookingID, CustomerID, VehicleID, RentalStartDate, RentalEndDate, PickupLocation, DropOffLocation, RentalPricePerDay, InitialCheckNotes, CreatedByUserID);
        }
    
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddRent();
                case enMode.Update:
                    return _UpdateRent();

            }
            return false;

        }
    
        public static clsRentalBooking GetRentalByID(int BookingID)
        {
            int CustomerID = -1, VehicleID = -1, CreatedByUserID = -1;
            string PickupLocation = "", DropOffLocation = "",InitialCheckNotes="";
            DateTime RentalStartDate=DateTime.Now,RentalEndDate=DateTime.Now;
            float RentalPricePerDay = 0;

            if (clsRentalBookingData.GetRentalBookByID(BookingID, ref CustomerID, ref VehicleID, ref RentalStartDate, ref RentalEndDate, ref PickupLocation, ref DropOffLocation, ref RentalPricePerDay, ref InitialCheckNotes, ref CreatedByUserID))
            {
                return new clsRentalBooking(BookingID, CustomerID, VehicleID, RentalStartDate, RentalEndDate, PickupLocation, DropOffLocation, RentalPricePerDay, InitialCheckNotes, CreatedByUserID);
            }
            else
                return null;




        }

        public static bool DeleteRent(int BookingID)
        {
            return clsRentalBookingData.DeleteRent(BookingID);
        }
        public  bool Delete()
        {
            return clsRentalBookingData.DeleteRent(this.BookingID);
        }

        public static DataTable GetAllRents()
        {
            return clsRentalBookingData.GetAllRentalBooking();
        }

        public static bool IsRentExists(int BookingID)
        {
            return clsRentalBookingData.IsRentExists(BookingID);
        }

        public  bool IsExists()
        {
            return clsRentalBookingData.IsRentExists(this.BookingID);
        }

        public bool IsReturned()
        {
            return clsVehicleReturnsData.IsReturnExistsByBookingID(this.BookingID);
        }

        public static bool IsBookingReturned(int BookingID)
        {
            return clsVehicleReturnsData.IsReturnExistsByBookingID(BookingID);
        }

        public static int GetTotalBookings()
        {
            return clsRentalBookingData.GetTotalBookings();
        }

        public static int GetTotalCompletedRents()
        {
            return clsRentalBookingData.GetTotalCompletedRents();
        }

        public static DataTable GetTotalRentTimesVehicleReport()
        {
            return clsRentalBookingData.GetVehiclesRentTimesReport();
        }
        public static DataTable GetVehiclesCountReport()
        {
            return clsRentalBookingData.GetVehiclesCountReport();
        }
    }
}
