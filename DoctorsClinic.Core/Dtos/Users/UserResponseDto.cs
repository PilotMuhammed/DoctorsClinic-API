using DoctorsClinic.Core.Dtos.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Users
{
    public class UserResponseDto
    {
        public required UserDto User { get; set; }
        public DoctorDto? Doctor { get; set; }
    }
}
