using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Permission
{
    public static class Permissions
    {
        public const string Patients_View = "Patients.View";
        public const string Patients_Create = "Patients.Create";
        public const string Patients_Update = "Patients.Update";
        public const string Patients_Delete = "Patients.Delete";

        public const string Doctors_View = "Doctors.View";
        public const string Doctors_Create = "Doctors.Create";
        public const string Doctors_Update = "Doctors.Update";
        public const string Doctors_Delete = "Doctors.Delete";

        public const string Appointments_View = "Appointments.View";
        public const string Appointments_Create = "Appointments.Create";
        public const string Appointments_Update = "Appointments.Update";
        public const string Appointments_Delete = "Appointments.Delete";

        public const string Specialties_View = "Specialties.View";
        public const string Specialties_Create = "Specialties.Create";
        public const string Specialties_Update = "Specialties.Update";
        public const string Specialties_Delete = "Specialties.Delete";

        public const string MedicalRecords_View = "MedicalRecords.View";
        public const string MedicalRecords_Create = "MedicalRecords.Create";
        public const string MedicalRecords_Update = "MedicalRecords.Update";
        public const string MedicalRecords_Delete = "MedicalRecords.Delete";

        public const string Prescriptions_View = "Prescriptions.View";
        public const string Prescriptions_Create = "Prescriptions.Create";
        public const string Prescriptions_Update = "Prescriptions.Update";
        public const string Prescriptions_Delete = "Prescriptions.Delete";

        public const string Medicines_View = "Medicines.View";
        public const string Medicines_Create = "Medicines.Create";
        public const string Medicines_Update = "Medicines.Update";
        public const string Medicines_Delete = "Medicines.Delete";

        public const string Invoices_View = "Invoices.View";
        public const string Invoices_Create = "Invoices.Create";
        public const string Invoices_Update = "Invoices.Update";
        public const string Invoices_Delete = "Invoices.Delete";

        public const string Payments_View = "Payments.View";
        public const string Payments_Create = "Payments.Create";
        public const string Payments_Update = "Payments.Update";
        public const string Payments_Delete = "Payments.Delete";

        public static readonly string[] All =
        {
            Patients_View, Patients_Create, Patients_Update, Patients_Delete,
            Doctors_View, Doctors_Create, Doctors_Update, Doctors_Delete,
            Appointments_View, Appointments_Create, Appointments_Update, Appointments_Delete,
            Specialties_View, Specialties_Create, Specialties_Update, Specialties_Delete,
            MedicalRecords_View, MedicalRecords_Create, MedicalRecords_Update, MedicalRecords_Delete,
            Prescriptions_View, Prescriptions_Create, Prescriptions_Update, Prescriptions_Delete,
            Medicines_View, Medicines_Create, Medicines_Update, Medicines_Delete,
            Invoices_View, Invoices_Create, Invoices_Update, Invoices_Delete,
            Payments_View, Payments_Create, Payments_Update, Payments_Delete
        };
    }
}