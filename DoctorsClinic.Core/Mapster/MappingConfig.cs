namespace DoctorsClinic.Core.Mapster
{
    public static class MappingConfig
    {
        public static void ConfigureMappings()
        {
            AppointmentMap.Configure();
            DoctorMap.Configure();
            InvoiceMap.Configure();
            MedicalRecordMap.Configure();
            MedicineMap.Configure();
            PatientMap.Configure();
            PaymentMap.Configure();
            PrescriptionMap.Configure();
            PrescriptionMedicineMap.Configure();
            RoleMap.Configure();
            SpecialtyMap.Configure();
            UserMap.Configure();   
        }
    }
}
