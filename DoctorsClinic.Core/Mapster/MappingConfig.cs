using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Mapster
{
    public static class MappingConfig
    {
        public static void ConfigureMappings()
        {
     
            UserMap.Configure();
            PatientMap.Configure();
            DoctorMap.Configure();
            AppointmentMap.Configure();
            SpecialtyMap.Configure();
            MedicalRecordMap.Configure();
            PrescriptionMap.Configure();
            MedicineMap.Configure();
            InvoiceMap.Configure();
            PaymentMap.Configure();
            PrescriptionMedicineMap.Configure();
        }
    }
}

