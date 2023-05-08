﻿using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        public virtual Student Student { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
