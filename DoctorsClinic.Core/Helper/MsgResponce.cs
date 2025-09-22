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
            public static string banned = "Your account is blocked";
            public static string NotActive = "Your account is not active";
        }

        public class AccountStatus
        {
            public static string Locked = "Account temporarily locked, try again later";
            public static string LockedTenMinutes = "Account temporarily locked, try again after 10 minutes";
        }

        public class Role
        {
            public static string NotFound(int? id) => $"Role Id :{id} not found";
            public static string NameExists = "Role name already exists!";
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

        public class Prescription
        {
            public static string NotFound = "Prescription not found";
        }

        public class PrescriptionMedicine
        {
            public static string NotFound = "Prescription Medicine not found";
        }

        public class Appointment
        {
            public static string NotFound = "Appointment not found";
            public static string AlreadyBooked = "The appointment slot is already booked";
            public static string CurrentlyAvailable = "The appointment is currently available";
        }

        public class Invoice
        {
            public static string NotFound = "Invoice not found";

            public static string Paid = "Invoice has already been paid";
            public static string Pending = "Invoice payment is pending";
            public static string Cancelled = "Invoice has been cancelled";
        }

        public class AlarmAmounts
        {
            public static string WrongEntry = "Total amount must be greater than zero!";
        }

        public class Payment
        {
            public static string NotFound = "Payment not found";

            public static string Failed = "Payment failed";
            public static string Success = "Payment completed successfully";
            public static string MethodNotSupported = "Payment method not supported";
            public static string AmountInvalid = "Invalid payment amount";
        }
    }
}