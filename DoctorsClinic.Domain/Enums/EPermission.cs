using System.ComponentModel.DataAnnotations;

namespace DoctorsClinic.Domain.Enums
{
    public enum EPermission
    {
        [Display(Name = "عرض المستخدمين")] Users_View = 1,
        [Display(Name = "إضافة مستخدم")] Users_Create,
        [Display(Name = "تعديل مستخدم")] Users_Update,
        [Display(Name = "حذف مستخدم")] Users_Delete,

        [Display(Name = "الادوار")] Role,
        [Display(Name = "اضافة دور")] AddRole,
        [Display(Name = "تعديل دور")] EditRole,
        [Display(Name = "حذف دور")] DeleteRole,
        [Display(Name = "تعيين الدور للمستخدم")] SetRole,

        [Display(Name = "الصلاحيات")] Permission,
        [Display(Name = "تعيين صلاحيات المستخدم")] SetPermission,

        [Display(Name = "عرض المرضى")] Patients_View,
        [Display(Name = "إضافة مريض")] Patients_Create,
        [Display(Name = "تعديل مريض")] Patients_Update,
        [Display(Name = "حذف مريض")] Patients_Delete,

        [Display(Name = "عرض الأطباء")] Doctors_View,
        [Display(Name = "إضافة طبيب")] Doctors_Create,
        [Display(Name = "تعديل طبيب")] Doctors_Update,
        [Display(Name = "حذف طبيب")] Doctors_Delete,

        [Display(Name = "عرض المواعيد")] Appointments_View,
        [Display(Name = "إضافة موعد")] Appointments_Create,
        [Display(Name = "تعديل موعد")] Appointments_Update,
        [Display(Name = "حذف موعد")] Appointments_Delete,

        [Display(Name = "عرض التخصصات")] Specialties_View,
        [Display(Name = "إضافة تخصص")] Specialties_Create,
        [Display(Name = "تعديل تخصص")] Specialties_Update,
        [Display(Name = "حذف تخصص")] Specialties_Delete,

        [Display(Name = "عرض السجلات الطبية")] MedicalRecords_View,
        [Display(Name = "إضافة سجل طبي")] MedicalRecords_Create,
        [Display(Name = "تعديل سجل طبي")] MedicalRecords_Update,
        [Display(Name = "حذف سجل طبي")] MedicalRecords_Delete,

        [Display(Name = "عرض الوصفات الطبية")] Prescriptions_View,
        [Display(Name = "إضافة وصفة طبية")] Prescriptions_Create,
        [Display(Name = "تعديل وصفة طبية")] Prescriptions_Update,
        [Display(Name = "حذف وصفة طبية")] Prescriptions_Delete,

        [Display(Name = "عرض الأدوية")] Medicines_View,
        [Display(Name = "إضافة دواء")] Medicines_Create,
        [Display(Name = "تعديل دواء")] Medicines_Update,
        [Display(Name = "حذف دواء")] Medicines_Delete,

        [Display(Name = "عرض الفواتير")] Invoices_View,
        [Display(Name = "إضافة فاتورة")] Invoices_Create,
        [Display(Name = "تعديل فاتورة")] Invoices_Update,
        [Display(Name = "حذف فاتورة")] Invoices_Delete,

        [Display(Name = "عرض المدفوعات")] Payments_View,
        [Display(Name = "إضافة مدفوعات")] Payments_Create,
        [Display(Name = "تعديل مدفوعات")] Payments_Update,
        [Display(Name = "حذف مدفوعات")] Payments_Delete,
    }
}