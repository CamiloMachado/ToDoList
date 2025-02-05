using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDo_GPT.Models;

[Table("Users")]
public class User
{
    public User()
    {
        TasksToDo = new Collection<TaskToDo>();
    }

    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(55)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [StringLength(60)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Informe um email válido.")]
    public string? UserEmail { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(50, ErrorMessage = "A senha precisar ter no mínimo 4 caracteres.", MinimumLength = (4))]
    [DataType(DataType.Password)]
    public string? UserPassword { get; set; }

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Compare("UserPassword", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
    public string? ConfirmPassword { get; set; }

    [JsonIgnore]
    public ICollection<TaskToDo>? TasksToDo { get; set; }
}