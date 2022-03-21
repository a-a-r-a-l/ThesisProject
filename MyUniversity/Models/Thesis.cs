using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyUniversity.Models
{
    [Table("Theses")]
    public class Thesis
    {
        [Display(Name = "Thesis ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ThesisId { get; set; }

        [Display(Name = "Thesis Title")]
        [Required(ErrorMessage = "Required")]
        public string ThesisName { get; set; }

        [Display(Name = "Thesis Descriptions")]
        [Required(ErrorMessage = "Required")]
        public string ThesisDescription { get; set; }



        [Display(Name = "Subject ID")]
        [Required]
        [ForeignKey(nameof(Thesis.Subject))]      // foreign key .
        public short SubjectId { get; set; }
        public Subject Subject { get; set; }


        [Display(Name = "Student ID")]
        [Required]
        [ForeignKey(nameof(Thesis.Student))]      // foreign key 
        public  string EnrollmentId { get; set; }


        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }


        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }


        [Display(Name = "Completion Percentage")]
        [Required]
        public int CompletionPercent { get; set; }


        public ICollection<SubmissionDetail> SubmissionDetails { get; set; }


        #region navigational properties to the studet Model 
        public Student Student { get; set; }
        #endregion
    }
}
