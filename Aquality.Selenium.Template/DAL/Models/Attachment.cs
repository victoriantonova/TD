using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("attachment")]
    public class Attachment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("content")]
        public byte[] Content { get; set; }
        [Column("content_type")]
        public string ContentType { get; set; }
        [Column("test_id")]
        public int TestId { get; set; }

        public Test Test { get; set; }
    }
}
