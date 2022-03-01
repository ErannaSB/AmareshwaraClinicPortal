//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Patient
    {
        [Display(Name = "PID")]
        public int PatientId { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        [Display(Name = "Gender")]

        public string Sex { get; set; }
        public string Date { get; set; }
        [Display(Name = "Mobile No")]

        public string MobileNumber { get; set; }
        public string Diagnosis { get; set; }
        public string Medicines { get; set; }
        public string Investigation { get; set; }
        public string Note { get; set; }
        public Nullable<int> MedicineId { get; set; }
        public string PID { get; set; }

        public virtual Medicine Medicine { get; set; }
        public string[] SelectedIDMedicines { get; set; }

    }
}
