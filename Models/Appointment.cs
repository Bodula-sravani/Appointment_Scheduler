﻿namespace Appointment_Scheduler.Models
{
	public class Appointment
	{

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set;}
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }

		public string userId { get; set; }

	}
}
