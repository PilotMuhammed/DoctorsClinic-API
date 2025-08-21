using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Helper
{
    public static class MsgResponce
    {
        public static string Failed = "An error occurred";
        public static string Success = "Operation completed successfully";

        public class User
        {
            public static string NotFound = "User not found";
            public static string UserNameExists = "Username already exists!";
            public static string Created = "User created successfully";
            public static string Updated = "User updated successfully";
            public static string Deleted = "User deleted successfully";
        }

        public class Patient
        {
            public static string NotFound = "Patient not found";
            public static string Created = "Patient added successfully";
            public static string Updated = "Patient information updated";
            public static string Deleted = "Patient deleted successfully";
        }

        public class Doctor
        {
            public static string NotFound = "Doctor not found";
            public static string Created = "Doctor added successfully";
            public static string Updated = "Doctor information updated";
            public static string Deleted = "Doctor deleted successfully";
        }

        public class Specialty
        {
            public static string NotFound = "Specialty not found";
            public static string Created = "Specialty added successfully";
            public static string Updated = "Specialty information updated";
            public static string Deleted = "Specialty deleted successfully";
        }
        public class MedicalRecord
        {
            public static string NotFound = "Medical Record not found";
            public static string Created = "Medical Record added successfully";
            public static string Updated = "Medical Record information updated";
            public static string Deleted = "Medical Record deleted successfully";
        }
        public class Medicine
        {
            public static string NotFound = "Medicine not found";
            public static string Created = "Medicine added successfully";
            public static string Updated = "Medicine information updated";
            public static string Deleted = "Medicine deleted successfully";
        }

        public class Appointment
        {
            public static string NotFound = "Appointment not found";
            public static string AlreadyBooked = "The appointment slot is already booked";
            public static string Created = "Appointment booked successfully";
            public static string Updated = "Appointment updated";
            public static string Cancelled = "Appointment cancelled";
            public static string Completed = "Appointment completed";
        }

        public class Invoice
        {
            public static string NotFound = "Invoice not found";
            public static string Paid = "Invoice has already been paid";
            public static string Pending = "Invoice payment is pending";
            public static string Cancelled = "Invoice has been cancelled";
        }

        public class Payment
        {
            public static string Failed = "Payment failed";
            public static string Success = "Payment completed successfully";
            public static string MethodNotSupported = "Payment method not supported";
            public static string AmountInvalid = "Invalid payment amount";
        }

        public class Role
        {
            public static string NotFound(int? id) => $"Role Id :{id} not found";
            public static string NameExists = "Role name already exists!";
            public static string Created = "Role created successfully";
            public static string Deleted = "Role deleted successfully";
        }

        public class Permission
        {
            public static string Invalid(List<int> permissions) =>
               $"The permissions: {string.Join(", ", permissions)} are invalid";
        }

        public class Password
        {
            public static string Wrong = "Password is wrong";
            public static string Incorrect = "Current password is incorrect";
            public static string Same = "Cannot use the same password as the previous one";
            public static string Changed = "Password changed successfully";
            public static string TooWeak = "Password does not meet the required strength criteria";
        }
    }
}
