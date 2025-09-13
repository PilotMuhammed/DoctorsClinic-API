using DoctorsClinic.Domain.Enums;
using System.ComponentModel;

namespace DoctorsClinic.Core.Helper
{
    public static class EnumHelper
    {
        public static Dictionary<string, Dictionary<string, int>> GetAllEnums()
        {
            var enums = new Dictionary<string, Dictionary<string, int>>();

            AddEnum<AppointmentStatus>(enums);
            AddEnum<EPermission>(enums);
            AddEnum<Gender>(enums);
            AddEnum<InvoiceStatus>(enums);
            AddEnum<PaymentMethod>(enums);
            return enums;
        }

        private static void AddEnum<TEnum>(Dictionary<string, Dictionary<string, int>> enums)
            where TEnum : Enum
        {
            enums[typeof(TEnum).Name] = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToDictionary(e => e.ToString(), e => Convert.ToInt32(e));
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? value.ToString();
        }
    }
}
