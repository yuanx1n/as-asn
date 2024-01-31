namespace WebApplication1.Model
{
	public class Auditlog
	{
		public int Id { get; set; }
		public string? Userid { get; set; }
		public string? Action { get; set; }
		public DateTime Time { get; set; }
	}
}
