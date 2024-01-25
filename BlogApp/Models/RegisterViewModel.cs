using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models{

  public class RegisterViewModel
  {
    [Required]
    [Display(Name ="UserName")]
    public string? UserName { get; set; }

    [Required]
    [Display(Name ="Ad Soyad")]
    public string? Name { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name ="Eposta Adres")]
    public string? Email {get; set;}

    [Required]
    [StringLength(10)]
    [DataType(DataType.Password)]
    [Display(Name="Parola")]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage ="Parolanız eşleşmiyor.")]
    [Display(Name="Parola Tekrar")]
    public string? ConfirmPassword { get; set; }


  }


}