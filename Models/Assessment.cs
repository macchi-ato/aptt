using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace mobile_app.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
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
