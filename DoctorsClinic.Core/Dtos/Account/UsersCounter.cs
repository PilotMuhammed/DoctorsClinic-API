using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class UsersCounter
    {
        public int PatientsCount { get; set; }
        public int DoctorsCount { get; set; }
        public int ReceptionistsCount { get; set; }
        public int NursesCount { get; set; }
        public int UsersCount { get; set; }
    }
}

