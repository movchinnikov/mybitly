namespace MyBitly.DAL.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Url")]
    public class UrlEntity : EntityBase
    {
        [StringLength(8)]
        [Required]
        [Index(IsUnique = true)]
        public string Hash { get; set; }

        [Required]
        public string LongUrl { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [Required]
        public int Clicks { get; set; }
    }
}