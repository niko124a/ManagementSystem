using System.ComponentModel.DataAnnotations;

namespace Common.Entities;

public class ApiAuth
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    
}