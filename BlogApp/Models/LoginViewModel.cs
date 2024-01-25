using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models{

  public class LoginViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name ="Eposta Adres")]
    public string? Email {get; set;}
    
    [Required]
    [StringLength(10)]
    [DataType(DataType.Password)]
    [Display(Name="Şifre")]
    public string? Password { get; set; }

  }


}