using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("test")]
    public class Test
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("status_id")]
        public int? StatusId { get; set; }
        [Column("method_name")]
        public string MethodName { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("session_id")]
        public int? SessionId { get; set; }
        [Column("start_time")]
        public DateTime? StartTime { get; set; }
        [Column("end_time")]
        public DateTime? EndTime { get; set; }
        [Column("env")]
        public string Env { get; set; }
        [Column("browser")]
        public string Browser { get; set; }
        [Column("author_id")]
        public int? AuthorId { get; set; }

        public Project Project { get; set; }
    }
}
