using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyUniversity.Models
{
    [Table("SubmissionDetails")]
    public class SubmissionDetail
    {


        
        [Display(Name = "Submission ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmissionId { get; set; }


        [Display(Name = "Enter Thesis Id")]
        [ForeignKey(nameof(SubmissionDetail.Thesis))]

        public short ThesisId { get; set; }


        [Display(Name = "Enter Description")]
        [Required]
        [MinLength(2)]
        [StringLength(60)]
        [Column("varchar")]
        public string SubmissionDesc { get; set; }


        [Display(Name = "Sumission Due Date:")]
        [Required(ErrorMessage = "Required")]
        public DateTime SubmissionDueOn { get; set; }


        [Display(Name = "Submission Date:")]
        [Required(ErrorMessage = "Required")]
        
        public DateTime SubmissionOn { get; set; }


        [Required(ErrorMessage = "Upload Submission File")]
        [StringLength(150)]
        public string SubmissionFile { get; set; }



        [StringLength(60)]
        public string FileContentType { get; set; }


        [Required]
        public string ReviewedBy { get; set; }


        [Required]
        public DateTime ReviewOn { get; set; }


        [Display(Name = "Remarks of submission")]
        [Required]
        [MaxLength(3)]
        [Column("remark")]
        public int Remarks { get; set; }

      //public bool SubmissionStatus { get; set; }

        #region Navigational Properties to the Thesis mod Model (1:0 mapping)

        public Thesis Thesis { get; set; }

        #endregion

    }
}




