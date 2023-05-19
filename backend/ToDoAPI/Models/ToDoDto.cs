using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.Models
{
    public class ToDoDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}
