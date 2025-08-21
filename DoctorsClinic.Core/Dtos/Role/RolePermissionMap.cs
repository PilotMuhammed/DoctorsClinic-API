using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Core.Dtos.Permission;

namespace DoctorsClinic.Core.Dtos.Role
{
    public static class RolePermissionMap
    {
        private static readonly Dictionary<UserRole, HashSet<string>> _map = new()
        {
            [UserRole.Admin] = new()
            {
            Permissions.Patients_View, Permissions.Patients_Create, Permissions.Patients_Update, Permissions.Patients_Delete,
            Permissions.Doctors_View, Permissions.Doctors_Create, Permissions.Doctors_Update, Permissions.Doctors_Delete,
            Permissions.Appointments_View, Permissions.Appointments_Create, Permissions.Appointments_Update, Permissions.Appointments_Delete,
            Permissions.Specialties_View, Permissions.Specialties_Create, Permissions.Specialties_Update, Permissions.Specialties_Delete,
            Permissions.MedicalRecords_View, Permissions.MedicalRecords_Create, Permissions.MedicalRecords_Update, Permissions.MedicalRecords_Delete,
            Permissions.Prescriptions_View, Permissions.Prescriptions_Create, Permissions.Prescriptions_Update, Permissions.Prescriptions_Delete,
            Permissions.Medicines_View, Permissions.Medicines_Create, Permissions.Medicines_Update, Permissions.Medicines_Delete,
            Permissions.Invoices_View, Permissions.Invoices_Create, Permissions.Invoices_Update, Permissions.Invoices_Delete,
            Permissions.Payments_View, Permissions.Payments_Create, Permissions.Payments_Update, Permissions.Payments_Delete,
            Permissions.Users_View, Permissions.Users_Create, Permissions.Users_Update, Permissions.Users_Delete
            },

            [UserRole.Doctor] = new()
            {
                Permissions.Patients_View,
                Permissions.MedicalRecords_View,
                Permissions.MedicalRecords_Create,
                Permissions.MedicalRecords_Update,
                Permissions.Prescriptions_View,
                Permissions.Prescriptions_Create,
                Permissions.Prescriptions_Update,
                Permissions.Appointments_View,
                Permissions.Appointments_Create,
                Permissions.Appointments_Update,
                Permissions.Medicines_View,
            },

            [UserRole.Receptionist] = new()
            {
                Permissions.Patients_View,
                Permissions.Patients_Create,
                Permissions.Patients_Update,
                Permissions.Appointments_View,
                Permissions.Appointments_Create,
                Permissions.Appointments_Update,
                Permissions.Appointments_Delete, 
                Permissions.Doctors_View,
                Permissions.Specialties_View,
                Permissions.Invoices_View,
                Permissions.Invoices_Create,
                Permissions.Invoices_Update,
                Permissions.Payments_View,
                Permissions.Payments_Create,
            },

            [UserRole.Nurse] = new()
            {
                Permissions.Patients_View,
                Permissions.MedicalRecords_View,
                Permissions.MedicalRecords_Create,
                Permissions.MedicalRecords_Update,
                Permissions.Prescriptions_View,
                Permissions.Appointments_View,
                Permissions.Appointments_Update,
                Permissions.Medicines_View,
            },
        };

        public static IReadOnlyCollection<string> GetPermissions(UserRole role) =>
            _map.TryGetValue(role, out var set) ? set : new HashSet<string>();
    }
}