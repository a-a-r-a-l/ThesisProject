using System.ComponentModel.DataAnnotations;

namespace MyUniversity.Models.Enum
{
    public enum MyIdentityGenders
    {
        [Display(Name = "Male")]
        Male,

        [Display(Name = "Female")]
        Female,

        [Display(Name = "Other")]
        Other
    }
}
