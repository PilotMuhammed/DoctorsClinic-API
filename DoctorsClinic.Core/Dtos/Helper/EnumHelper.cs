using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Helper
{
    public static class EnumHelper
    {
        public static Dictionary<string, Dictionary<string, int>> GetAllEnums()
        {
            var enums = new Dictionary<string, Dictionary<string, int>>();

            AddEnum<DoctorsClinic.Domain.Enums.AppointmentStatus>(enums);
            AddEnum<DoctorsClinic.Domain.Enums.Gender>(enums);
            AddEnum<DoctorsClinic.Domain.Enums.InvoiceStatus>(enums);
            AddEnum<DoctorsClinic.Domain.Enums.PaymentMethod>(enums);
            AddEnum<DoctorsClinic.Domain.Enums.UserRole>(enums);

            return enums;
        }

        private static void AddEnum<TEnum>(Dictionary<string, Dictionary<string, int>> enums)
            where TEnum : Enum
        {
            enums[typeof(TEnum).Name] = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToDictionary(e => e.ToString(), e => Convert.ToInt32(e));
        }
    }
}
