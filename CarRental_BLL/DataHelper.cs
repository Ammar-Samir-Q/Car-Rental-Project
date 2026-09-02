using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental_DAL;
namespace CarRental_BLL
{
    public class DataHelper
    {
        public static bool TestConnection()
        {
            return CarRental_DAL.DataHelper.CheckConnection();
        }


        public static bool BackupDatabase(string Path)
        {
            return CarRental_DAL.DataHelper.BackupDatabase(Path);
        }

    }
}
