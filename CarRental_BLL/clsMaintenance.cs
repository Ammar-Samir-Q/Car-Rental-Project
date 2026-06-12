using CarRental_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_BLL
{
    public class clsMaintenance
    {
        enum enMode { AddNew=1,Update=2}

        public int MaintenanceID { get; set; }  
        public int VehicleID { get; set; }

        public clsVehicle VehicleInfo { get; set; }

        public string Description { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public float Cost { get; set; }

        private enMode _Mode= enMode.AddNew;


        //-----------------------------------

        public clsMaintenance()
        {
            this.MaintenanceID = -1;
            this.VehicleID = -1;
            this.Description = "";
            this.MaintenanceDate= DateTime.Now;
            this.Cost = 0;
            this._Mode = enMode.AddNew;
        }

        public clsMaintenance(int MainteanceID,int VehicleID,string Description, DateTime MaintenanceDate, float Cost)
        {
            this.MaintenanceID = MainteanceID;
            this.VehicleID = VehicleID;
            this.VehicleInfo=clsVehicle.GetVehicleByID(VehicleID);  
            this.Description = Description;
            this.MaintenanceDate = MaintenanceDate;
            this.Cost = Cost;
            this._Mode= enMode.Update;
        }

        //----------------------------------

        private bool _AddMaintenance()
        {
            this.MaintenanceID = clsMaintenanceData.AddMaintenance(VehicleID, Description, MaintenanceDate, Cost); ;
            return this.MaintenanceID > 0;
        }

        private bool _UpdateMaintenance()
        {
            return clsMaintenanceData.UpdateMainteance(MaintenanceID, VehicleID, Description, MaintenanceDate, Cost);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    this._Mode = enMode.Update;
                    return _AddMaintenance();
                case enMode.Update:
                    return _UpdateMaintenance();
            }
            return false;

        }

        public static clsMaintenance GetMainteanceByID(int MaintenanceID)
        {
            int VehicleID = -1;
            string Description = "";
            DateTime MaintenanceDate = DateTime.Now;
            float Cost = 0;
            if (clsMaintenanceData.GetMaintenanceByID(MaintenanceID, ref VehicleID, ref Description, ref MaintenanceDate, ref Cost))
            {
                return new clsMaintenance(MaintenanceID, VehicleID, Description, MaintenanceDate, Cost);
            }
            else
                return null;

        }

        public static DataTable GetAllMaintenances()
        {
            return clsMaintenanceData.GetAllMaintenances();
        }

        public static DataTable GetAllMaintenancesForVehicle(int VehicleID)
        {
            return clsMaintenanceData.GetAllMaintenancesForVehicle(VehicleID);
        }


        public static bool DeleteMaintenance(int MaintenanceID)
        {
            return clsMaintenanceData.DeleteMaintenance(MaintenanceID);
        }

        public bool Delete()
        {
            return clsMaintenanceData.DeleteMaintenance(this.MaintenanceID);
        }

        public static bool IsMaintenanceExists(int MaintenanceID)
        {
            return clsMaintenanceData.IsMaintenanceExists(MaintenanceID);
        }

        public bool IsExists()
        {
            return clsMaintenanceData.IsMaintenanceExists(this.MaintenanceID);
        }

    }
}
