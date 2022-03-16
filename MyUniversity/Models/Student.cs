

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyUniversity.Models
{
    [Table("Students")]
    public class Student
    {
        /// <summary>
        ///     Mapped to the ID column of the Identity User
        /// </summary>
        [Display(Name = "User ID")]
        [Key]
        [ForeignKey(nameof(Student.User))]
        public Guid UserId { get; set; }


        [Display(Name = "Student Enrollment ID")]
        [Required(ErrorMessage = "{0} cannot be empty.")]
        [StringLength(10, ErrorMessage = "{0} should contain {1} characters.")]
        [MinLength(10, ErrorMessage = "{0} should contain {1} characters.")]
        public string EnrollmentID { get; set; }


        [Display(Name = "Name of the Parent / Guardian")]
        [MinLength(2, ErrorMessage = "{0} should have at least {1} characters.")]
        [StringLength(60, ErrorMessage = "{0} should not contain more than {1} characters.")]
        public string ParentName { get; set; }


        #region Navigational Properties to the MyIdentityUser model (1:1 mapping)

        public MyIdentityUser User { get; set; }

        #endregion

    }
}