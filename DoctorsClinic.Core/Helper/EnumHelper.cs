using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Helper
{
    public static class EnumHelper
    {
        public static Dictionary<string, Dictionary<string, int>> GetAllEnums()
        {
            var enums = new Dictionary<string, Dictionary<string, int>>();

            AddEnum<Domain.Enums.AppointmentStatus>(enums);
            AddEnum<Domain.Enums.Gender>(enums);
            AddEnum<Domain.Enums.InvoiceStatus>(enums);
            AddEnum<Domain.Enums.PaymentMethod>(enums);
            AddEnum<Domain.Enums.UserRole>(enums);

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
