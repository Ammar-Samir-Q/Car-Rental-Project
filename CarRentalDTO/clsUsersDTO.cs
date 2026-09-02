using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTO
{
    public class clsUsersDTO
    {

        public int UserID { get; set; }
        public int PersonID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public byte Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }



    }
}
