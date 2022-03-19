using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyUniversity.Models
{
    [Table("Subjects")]
    public class Subject
    {
        [Display(Name = "Subject ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SubjectId { get; set; }

        [Display(Name = "Name of the Subject")]
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string SubjectName { get; set; }

    }
}
