using System;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; } // CREATE, UPDATE, DELETE

        [Required]
        [StringLength(100)]
        public string EntityName { get; set; } // Student, Teacher, Subject

        public int EntityId { get; set; }

        [StringLength(500)]
        public string OldValues { get; set; }

        [StringLength(500)]
        public string NewValues { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}