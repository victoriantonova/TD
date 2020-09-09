using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("log")]
    public class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("is_exception")]
        public bool IsExceprion { get; set; }
        [Column("test_id")]
        public int TestId { get; set; }

        public Test Test { get; set; }
    }
}
