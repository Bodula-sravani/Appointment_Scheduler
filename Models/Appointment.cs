namespace Appointment_Scheduler.Models
{
	public class Appointment
	{

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set;}
		public TimeOnly StartTime { get; set; }
		public TimeOnly EndTime { get; set; }

		public string userId { get; set; }

	}
}
