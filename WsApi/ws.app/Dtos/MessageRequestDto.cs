using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class MessageRequestDto
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string TargetId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Body { get; set; }
    }
}