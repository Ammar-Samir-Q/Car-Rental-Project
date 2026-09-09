using CarRental_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CarRentalDTO;
namespace CarRental_BLL
{
    public class clsUsers
    {
        enum enMode { AddNew=1,Update=2}
        public enum enRole { Admin=1,User=2,Customer=3}

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPeople PersonInfo {get; set;  }

        public string Username { get; set; }
        public string Password { get; set; }

        public enRole Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }  

        private enMode _Mode = enMode.AddNew;
        
        public string GetRoleTitle
        {
            get
            {
                switch (this.Role)
                {
                    case enRole.Admin:
                        return "Admin";
                    case enRole.User:
                        return "User";
                    case enRole.Customer:
                        return "Customer";
                 }
                return "User";
            }
        }

        //---------------------------------------
        public clsUsers()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.Username = "";
            this.Password = "";
            this.Role = enRole.Admin;
            this.IsActive = false;
            this.CreationDate = DateTime.Now;
            this._Mode = enMode.AddNew;
        }

        public clsUsers(int UserID,int PersonID,string Username,string Password,byte Role,bool IsActive,DateTime CreationDate)
        {
            this.UserID=UserID;
            this.PersonID=PersonID;
            this.PersonInfo = clsPeople.GetPerson(PersonID);
            this.Username=Username;
            this.Password=Password;
            this.Role = (enRole)Role;
            this.IsActive = IsActive;
            this.CreationDate = CreationDate;
            this._Mode = enMode.Update;
        }

        private bool _AddUser()
        {
            
            this.UserID = clsUserData.AddUser(PersonID, Username, Util.Security.SecurityHelper.HashData(Password), (byte)Role, IsActive);
            return this.UserID > 0;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(UserID,PersonID,Username, Util.Security.SecurityHelper.HashData(Password), (byte)Role,IsActive,CreationDate);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddUser();
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static clsUsers GetUserByID(int UserID)
        {
            int PersonID = -1;
            string Username = "", Password = "";
            byte Role = 0;
            bool IsActive = false;
            DateTime CreationDate = DateTime.Now;  
            if (clsUserData.GetUserByID(UserID, ref PersonID, ref Username, ref Password, ref Role, ref IsActive,ref CreationDate))
            {
                return new clsUsers(UserID, PersonID, Username, Password, Role, IsActive,CreationDate);
            }
            else
                return null;
        }
        public async static Task<clsUsers> GetUserByIDAsync(int UserID)
        {
            clsUsersDTO usersDTO = new clsUsersDTO();
            usersDTO=await clsUserData.GetUserByIDAsync(UserID);
            if (!string.IsNullOrEmpty(usersDTO.Username))
            {
                return new clsUsers(usersDTO.UserID, usersDTO.PersonID, usersDTO.Username, usersDTO.Password, usersDTO.Role, usersDTO.IsActive, usersDTO.CreationDate);
            }
            else
                return null;
        }

        public async static Task<clsUsers> GetUserByUsernameAndPasswordAsync(string Username,string Password)
        {
            clsUsersDTO usersDTO = new clsUsersDTO();
            usersDTO = await clsUserData.GetUserByUsernameAndPasswordAsync(Username,Util.Security.SecurityHelper.HashData(Password));
            if (!string.IsNullOrEmpty(usersDTO.Username))
            {
                return new clsUsers(usersDTO.UserID, usersDTO.PersonID, usersDTO.Username, usersDTO.Password, usersDTO.Role, usersDTO.IsActive, usersDTO.CreationDate);
            }
            else
                return null;
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public bool Delete()
        {
            return clsUserData.DeleteUser(this.UserID);
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();   
        }

        public static bool IsUserExists(int UserID)
        {
            return clsUserData.IsUserExists(UserID);
        }

        public static bool IsUserExists(string UserName,string Password)
        {
            return clsUserData.IsUserExists(UserName,Util.Security.SecurityHelper.HashData(Password));
        }

        public bool IsExists()
        {
            return clsUserData.IsUserExists(this.UserID);
        }


        public static bool IsUserExistsByPersonID(int PersonID)
        {
            return clsUserData.IsUserExistsByPersonID(PersonID);
        }

    }
}
