using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MaxLength(280, ErrorMessage = "Title can not be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(280, ErrorMessage = "Content can not be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
