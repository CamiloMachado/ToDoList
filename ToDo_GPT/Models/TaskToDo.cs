using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDo_GPT.Models;

[Table("Tasks")]
public class TaskToDo
{
    [Key]
    public int TaskId { get; set; }

    [Required(ErrorMessage = "Informe o título.")]
    [StringLength(30, ErrorMessage = "O título precisar ter no mínimo 3 caracteres.", MinimumLength = (3))]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Informe a descrição.")]
    [StringLength(50, ErrorMessage = "A descrição precisar ter no mínimo 4 caracteres.", MinimumLength = (4))]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    [JsonIgnore]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}