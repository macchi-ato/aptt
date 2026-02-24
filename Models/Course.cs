using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace mobile_app.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Ignore]
        public string DateRange => $"{StartDate:MM-dd-yyyy} - {EndDate:MM-dd-yyyy}";
        public int? StartNotificationId { get; set; }
        public int? EndNotificationId { get; set; }
        public bool StartNotificationScheduled { get; set; }
        public bool EndNotificationScheduled { get; set; }

    }
}
