using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CarRental_DAL;
namespace CarRental_BLL
{
    public class clsVehicle
    {
        enum enMode { AddNew = 1, Update = 2 }
        public enum enFuelType { Petrol = 1, Diesel = 2,Gas=3, Electric = 4, Hybird = 5 }
        public enum enVehicleCategory { Micro = 1, Sedan = 2, HatchBack = 3, Universal = 4, LiftBack= 5,MuscleCar=6,SportCar=7,SUV=8,Crossover=9,Pickup=10,Van=11 }

        public int VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public int Year { get; set; }

        public int CurrentMilage { get; set; }

        public enFuelType FuelType { get; set; }

        public enVehicleCategory VehicleCategory { get; set; }

        public string PlateNumber { get; set; }

        public float RentalPricePerDay { get; set; }

        public bool IsAvailableForRent { get; set; }

        public string ImagePath { get; set; }

        private enMode _Mode;

        //--------------------------------------------------

        public clsVehicle()
        {
            this.VehicleID = -1;
            this.Make = "";
            this.Model = "";
            this.Year = -1;
            this.CurrentMilage = -1;
            this.FuelType = enFuelType.Petrol;
            this.VehicleCategory = enVehicleCategory.Sedan;
            this.PlateNumber = "";
            this.RentalPricePerDay = 0;
            this.IsAvailableForRent = false;
            this.ImagePath = "";
            this._Mode = enMode.AddNew;

        }

        public clsVehicle(int VehicleID, string Make, string Model,
            int Year, int CuurentMilage, int FuelType, int VehicleCategory, string PlateNumber, float RentalPricePerDay, bool IsAvailableForRent, string ImagePath)
        {
            this.VehicleID = VehicleID;
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            this.CurrentMilage = CuurentMilage;
            this.FuelType = (enFuelType)FuelType;
            this.VehicleCategory = (enVehicleCategory)VehicleCategory;
            this.PlateNumber = PlateNumber;
            this.RentalPricePerDay = RentalPricePerDay;
            this.IsAvailableForRent = IsAvailableForRent;
            this.ImagePath = ImagePath;
            this._Mode = enMode.Update;
        }
    
        //----------------------------------------------------
        
        private bool _AddVehicle()
        {
            this.VehicleID = clsVehicleData.AddVehicle(Make, Model, Year, CurrentMilage, (int)FuelType, (int)VehicleCategory, PlateNumber, RentalPricePerDay, IsAvailableForRent, ImagePath);
            return this.VehicleID > 0;
        }
        
        private bool _UpdateVehicle()
        {
            return clsVehicleData.UpdateVehicle(VehicleID, Make, Model, Year, CurrentMilage, (int)FuelType, (int)VehicleCategory, PlateNumber, RentalPricePerDay, IsAvailableForRent, ImagePath);
        }
    
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddVehicle();
                case enMode.Update:
                    return _UpdateVehicle();
            }
            return false;
        }
    
        public static clsVehicle GetVehicleByID(int VehicleID)
        {
            string Make = "", Model = "", PlateNumber = "",ImagePath="";
            int FuelType = 1,VehicleCategory=1,CurrentMilage=0,Year=0;
            float RentalPricePerDay = 0;
            bool IsAvailableForRent = false;
            if (clsVehicleData.GetVehicleByID(VehicleID, ref Make, ref Model, ref Year, ref CurrentMilage, ref FuelType, ref VehicleCategory, ref PlateNumber, ref RentalPricePerDay, ref IsAvailableForRent, ref ImagePath))
            {
                return new clsVehicle(VehicleID, Make, Model, Year, CurrentMilage, FuelType, VehicleCategory, PlateNumber, RentalPricePerDay, IsAvailableForRent, ImagePath);
            }
            else
                return null;
        }
    
        public static bool DeleteVehicle(int VehicleID)
        {
            return clsVehicleData.DeleteVehicle(VehicleID);
        }
        
        public bool Delete()
        {
            return clsVehicleData.DeleteVehicle(this.VehicleID);
        }

        public static DataTable GetAllVehicles()
        {
            return clsVehicleData.GetAllVehicles();
        }

        public static DataTable GetAllAvailableVehicles()
        {
            return clsVehicleData.GetAllAvailableVehicles();
        }

        public static bool IsVehicleExists(int VehicleID)
        {
            return clsVehicleData.IsVehicleExists(VehicleID);
        }

        public static bool IsVehicleExists(string PlateNumber)
        {
            return clsVehicleData.IsVehicleExists(PlateNumber);
        }


        public bool IsExists()
        {
            return clsVehicleData.IsVehicleExists(this.VehicleID);
        }

        public string GetFuelTypeTitle()
        {
            switch(FuelType)
            {
                case enFuelType.Petrol:
                    return "Petrol";
                case enFuelType.Diesel:
                    return "Diesel";
                case enFuelType.Gas:
                    return "Gas";
                case enFuelType.Electric:
                    return "Electric";
                case enFuelType.Hybird:
                    return "Hybird";
            }
            return "";

        }

        public string GetVehicleCategoryTitle()
        {
            switch(VehicleCategory)
            {
                case enVehicleCategory.Micro:
                    return "Micro";
                case enVehicleCategory.Sedan:
                    return "Sedan";
                case enVehicleCategory.HatchBack:
                    return "Hatch Back";
                case enVehicleCategory.Universal:
                    return "Universal";
                case enVehicleCategory.LiftBack:
                    return "LiftBack";
                case enVehicleCategory.MuscleCar:
                    return "Muscle Car";
                case enVehicleCategory.SportCar:
                    return "Sport Car";
                case enVehicleCategory.SUV:
                    return "SUV";
                case enVehicleCategory.Crossover:
                    return "Crossover";
                case enVehicleCategory.Pickup:
                    return "Pickup";
                case enVehicleCategory.Van:
                    return "Van";

            }
            return "";


        }

        public bool SetVehicleAvilable(bool IsAvailable)
        {
            return clsVehicleData.SetVehicleAvilable(this.VehicleID, IsAvailable);
        }
        public static bool SetVehicleAvilable(int VehicleID,bool IsAvailable)
        {
            return clsVehicleData.SetVehicleAvilable(VehicleID, IsAvailable);
        }

        public static bool UpdateMilage(int VehicleID,int Milage)
        {
            return  clsVehicleData.UpdateMilage(VehicleID, Milage);
        }

        public bool UpdateMilage(int Milage)
        {
            return clsVehicleData.UpdateMilage(this.VehicleID, Milage);
        }

        public static int GetTotalRentTimes(int VehicleID)
        {
            return clsVehicleData.GetTotalRentTimes(VehicleID);
        }
        public int GetTotalRentTimes()
        {
            return clsVehicleData.GetTotalRentTimes(this.VehicleID);
        }

        public static DataTable GetRentHistory(int VehicleID)
        {
           return clsVehicleData.GetVehicleRentHistory(VehicleID);
        }

        public static int GetTotalVehicles()
        {
            return clsVehicleData.GetTotalVehicles();
        }

    }
}
