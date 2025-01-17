using System.ComponentModel;

namespace RazorAllinRent.Dto
{
    public class UpdateItemDto: CreateItemDto
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        [DisplayName("Image")]
        public new IFormFile? ImageFile { get; set; } = null!;
        [DisplayName("Activé")]
        public bool IsActive { get; set; }
    }
}
