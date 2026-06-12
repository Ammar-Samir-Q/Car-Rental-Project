using CarRental_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_BLL
{
    public class clsRentalTransaction
    {
        enum enMode { Addnew=1,Update=2}

        public int TransactionID { get; set; }

        public int BookingID { get; set; }

        public  clsRentalBooking BookingInfo { get; set; }
        
        public int ReturnID { get; set; }

        public clsVehicleReturns ReturnInfo { get; }

        public string PaymentsDetails { get; set; }

        public float PaidInitialTotalDueAmount {  get; set; }

        public float ActualTotalDueAmount { get; set; }

        public float TotalRefundedAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime UpdatedTransactionDate {  get; set; }
        
        private enMode _Mode= enMode.Addnew;    


        //------------------------------------

        public clsRentalTransaction()
        {
            this.TransactionID = -1;
            this.BookingID = -1;
            this.ReturnID = -1;
            this.PaymentsDetails = "";
            this.PaidInitialTotalDueAmount = 0;
            this.ActualTotalDueAmount = 0;
            this.TotalRefundedAmount = 0;
            this.TransactionDate = DateTime.Now;
            this.UpdatedTransactionDate = TransactionDate;
            this._Mode= enMode.Addnew;  
        }
        
        public clsRentalTransaction(int TransactionID, int BookingID,int ReturnID,string PaymentDetails,float PaidInitialTotalDueAmount,float ActualTotalDueAmount,float TotalRefundedAmount,DateTime TransactionDate,DateTime UpdatedTransactionDate)
        {
            this.TransactionID=TransactionID;
            this.BookingID=BookingID;
            this.BookingInfo=clsRentalBooking.GetRentalByID(BookingID);
            this.ReturnID=ReturnID;
            if(ReturnID>0)
                this.ReturnInfo=clsVehicleReturns.GetReturnByID(ReturnID);
            this.PaymentsDetails=PaymentDetails;
            this.PaidInitialTotalDueAmount=PaidInitialTotalDueAmount;
            this.ActualTotalDueAmount=ActualTotalDueAmount;
            this.TotalRefundedAmount=TotalRefundedAmount;
            this.TransactionDate=TransactionDate;
            this.UpdatedTransactionDate=UpdatedTransactionDate;
            this._Mode = enMode.Update;

        }

        //-----------------------------------

        private bool _AddTransaction()
        {
            this.TransactionID = clsRentalTransactionData.AddTransaction(BookingID, ReturnID, PaymentsDetails, PaidInitialTotalDueAmount, ActualTotalDueAmount, TotalRefundedAmount, TransactionDate, UpdatedTransactionDate);
            return this.TransactionID > 0;
        }

        private bool _UpdateTransaction()
        {
            return clsRentalTransactionData.UpdateTransaction(TransactionID, BookingID, ReturnID, PaymentsDetails, PaidInitialTotalDueAmount, ActualTotalDueAmount, TotalRefundedAmount, TransactionDate, UpdatedTransactionDate);
        }

        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.Addnew:
                    _Mode = enMode.Update;
                    return _AddTransaction();
                case enMode.Update:
                       return _UpdateTransaction(); 

            }
            return false; 

        }

        public static clsRentalTransaction GetTransactionByID(int TransactionID)
        {
            int BookingID = -1, ReturnID = -1;
            string PaymentDetails = "";
            float PaidInitialTotalDueAmount=0,ActualTotalDueAmount=0,TotalRefundedAmount=0; 
            DateTime TransactionDate = DateTime.Now;
            DateTime UpdatedTransactionDate = DateTime.Now;
            if (clsRentalTransactionData.GetTransactionByID(TransactionID, ref BookingID, ref ReturnID, ref PaymentDetails, ref PaidInitialTotalDueAmount, ref ActualTotalDueAmount, ref TotalRefundedAmount, ref TransactionDate, ref UpdatedTransactionDate))
            {
                return new clsRentalTransaction(TransactionID, BookingID, ReturnID, PaymentDetails, PaidInitialTotalDueAmount, ActualTotalDueAmount, TotalRefundedAmount, TransactionDate, UpdatedTransactionDate);
            }
            else
                return null;
        }

        public static clsRentalTransaction GetTransactionByBookingID(int BookingID)
        {
            int TransactionID = -1, ReturnID = -1;
            string PaymentDetails = "";
            float PaidInitialTotalDueAmount = 0, ActualTotalDueAmount = 0, TotalRefundedAmount = 0;
            DateTime TransactionDate = DateTime.Now;
            DateTime UpdatedTransactionDate = DateTime.Now;
            if (clsRentalTransactionData.GetTransactionByBookingID(BookingID, ref TransactionID, ref ReturnID, ref PaymentDetails, ref PaidInitialTotalDueAmount, ref ActualTotalDueAmount, ref TotalRefundedAmount, ref TransactionDate, ref UpdatedTransactionDate))
            {
                return new clsRentalTransaction(TransactionID, BookingID, ReturnID, PaymentDetails, PaidInitialTotalDueAmount, ActualTotalDueAmount, TotalRefundedAmount, TransactionDate, UpdatedTransactionDate);
            }
            else
                return null;
        }

        public static clsRentalTransaction GetTransactionByReturnID(int ReturnID)
        {
            int TransactionID = -1, BookingID = -1;
            string PaymentDetails = "";
            float PaidInitialTotalDueAmount = 0, ActualTotalDueAmount = 0, TotalRefundedAmount = 0;
            DateTime TransactionDate = DateTime.Now;
            DateTime UpdatedTransactionDate = DateTime.Now;
            if (clsRentalTransactionData.GetTransactionByReturnID(ReturnID ,ref TransactionID,ref BookingID, ref PaymentDetails, ref PaidInitialTotalDueAmount, ref ActualTotalDueAmount, ref TotalRefundedAmount, ref TransactionDate, ref UpdatedTransactionDate))
            {
                return new clsRentalTransaction(TransactionID, BookingID, ReturnID, PaymentDetails, PaidInitialTotalDueAmount, ActualTotalDueAmount, TotalRefundedAmount, TransactionDate, UpdatedTransactionDate);
            }
            else
                return null;
        }

        public static DataTable GetAllTransactions()
        {
            return clsRentalTransactionData.GetAllTransactions();
        }

        public static bool DeleteTransaction(int TransactionID)
        {
            return clsRentalTransactionData.DeleteTransaction(TransactionID);
        }

        public bool Delete()
        {
            return clsRentalTransactionData.DeleteTransaction(this.TransactionID);
        }

        public static bool IsTransactionExists(int TransactionID)
        {
            return clsRentalTransactionData.IsTransactionExists(TransactionID);
        }

        public  bool IsExists()
        {
            return clsRentalTransactionData.IsTransactionExists(this.TransactionID);
        }
    
        public static DataTable GetDailyReportData()
        {
            return clsRentalTransactionData.GetDailyReport();
        }

        public static DataTable GetRentReportData()
        {
            return clsRentalTransactionData.GetRentReport();
        }
    }
}
