namespace MyBitly.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Url")]
    public class UrlEntity : Entity
    {
        [StringLength(8)]
        [Required]
        [Index(IsUnique = true)]
        public string Hash { get; set; }

        [Required]
        public string LongUrl { get; set; }
    }
}