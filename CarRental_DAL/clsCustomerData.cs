using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_DAL
{
    public class clsCustomerData
    {

        public static int AddCustomer(int PersonID,DateTime CreationDate,int CreatedByUserID,string DriverLicenseNumber)
        {
            int CustomerID = -1;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"Insert Into Customers
(PersonID,CreationDate,CreatedByUserID,DriverLicenseNumber)
VALUES
(@PersonID,@CreationDate,@CreatedByUserID,@DriverLicenseNumber)
SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreationDate", CreationDate);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@DriverLicenseNumber",DriverLicenseNumber);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ID))
                    CustomerID = ID;



            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }

            return CustomerID;
        }

        public static bool UpdateCustomer(int CustomerID, int PersonID, DateTime CreationDate, int CreatedByUserID, string DriverLicenseNumber)
        {
            bool IsUpdated = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"UPDATE Customers 
Set PersonID=@PersonID,
CreationDate=@CreationDate,
CreatedByUserID=@CreatedByUserID,
DriverLicenseNumber=@DriverLicenseNumber
WHERE CustomerID=@CustomerID;";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreationDate", CreationDate);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@DriverLicenseNumber", DriverLicenseNumber);
            try
            {
                connection.Open();
                IsUpdated = command.ExecuteNonQuery() > 0;
            }
            catch (System.Exception ex)
            { }
            finally { connection.Close(); }
            return IsUpdated;

        }

        public static bool GetCustomerByID(int CustomerID,ref int PersonID,ref DateTime CreationDate,ref int CreatedByUserID,ref string DriverLicenseNumber)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT TOP 1 Customers.* FROM Customers WHERE CustomerID=@CustomerID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    CreationDate = (DateTime)reader["CreationDate"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    DriverLicenseNumber = (string)reader["DriverLicenseNumber"];
                    IsFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }
            return IsFound;


        }

        public static bool GetCustomerByPersonID(int PersonID, ref int CustomerID, ref DateTime CreationDate, ref int CreatedByUserID, ref string DriverLicenseNumber)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT TOP 1 Customers.* FROM Customers WHERE PersonID=@PersonID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CustomerID = (int)reader["CustomerID"];
                    CreationDate = (DateTime)reader["CreationDate"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    DriverLicenseNumber = (string)reader["DriverLicenseNumber"];
                    IsFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }
            return IsFound;


        }

        public static bool DeleteCustomer(int CustomerID)
        {
            bool IsDeleted = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"DELELT FROM Customers WHERE CustomerID=@CustomerID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            try
            {
                connection.Open();
                IsDeleted = command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return IsDeleted;
        }

        public static DataTable GetAllCustomers()
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT * FROM Customer_View";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }
            return dataTable;
            {
            }
        }

        public static DataTable GetAllBookingsForCustomer(int CustomerID)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT BookingID,RentalBooking_View.Model,RentalStartDate,RentalEndDate,
RentalPricePerDay,CreatedBy FROM RentalBooking_View WHERE CustomerID=@CustomerID;";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID",CustomerID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex)
            { }
            finally { connection.Close(); }
            return dataTable;
            {
            }
        }


        public static bool IsCustomerExists(int CustomerID)
        {
            bool IsExists = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT 1 FROM Customers WHERE CustomerID=@CustomerID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsExists = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return IsExists;



        }

        public static bool IsCustomerExists(string DriverLicneseNumber)
        {
            bool IsExists = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT 1 FROM Customers WHERE DriverLicneseNumber=@DriverLicneseNumber";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverLicenseNumber", DriverLicneseNumber);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsExists = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return IsExists;



        }

        public static bool IsCustomerExistsByPersonID(int PersonID)
        {
            bool IsExists = false;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT 1 FROM Customers WHERE PersonID=@PersonID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsExists = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return IsExists;



        }

        public static int  GetTotalRentTimes(int CustomerID)
        {
            int TotalRentTimes = 0;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT Count(RentalBooking.BookingID) As TotalRentTimes FROM RentalBooking
WHERE CustomerID=@CustomerID;
";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            try 
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int Count))
                    TotalRentTimes = Count;
          
            }
            catch(Exception ex)
            {

            }
            finally { connection.Close(); }
            return TotalRentTimes;
        }

        public static DateTime GetLastRentDate(int CustomerID)
        {
            DateTime LastRentDate=DateTime.MinValue;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT TOP 1 RentalBooking.RentalStartDate FROM RentalBooking
WHERE CustomerID=3
ORDER BY RentalStartDate DESC
";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && DateTime.TryParse(result.ToString(), out DateTime LastDate))
                    LastRentDate = LastDate;

            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return LastRentDate;
        }

        public static int GetTotalCustomers()
        {
            int TotalCustomers = 0;
            SqlConnection connection = new SqlConnection(DataHelper.ConnectionString);
            string Query = @"SELECT COUNT(CustomerID) FROM Customers";
            SqlCommand command = new SqlCommand(Query, connection);
            
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int Count))
                    TotalCustomers = Count;

            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return TotalCustomers;
        }


    }
}
