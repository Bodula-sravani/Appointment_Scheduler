using Appointment_Scheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Appointment_Scheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		IConfiguration configuration;
		public SqlConnection connection;
		public HomeController(IConfiguration configuration)
		{
			this.configuration = configuration;
			this.connection = new SqlConnection(configuration.GetConnectionString("DB"));

		}
		//public HomeController(ILogger<HomeController> logger)
  //      {
  //          _logger = logger;
  //      }

        public bool validateUser(string userId,string password)
        {
			//Console.WriteLine("entered validateUser method");
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("getUser", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@userid", userId);
				SqlDataReader reader = command.ExecuteReader();
				//Console.WriteLine("reader excecuted");
				string getPassword="";
				while (reader.Read())
				{
					getPassword = (string)reader["userPassword"];
				}
				//Console.WriteLine("reader value: " + (string)reader["userPassword"]);
				//Console.WriteLine("passsowrds:  " + getPassword + " " + password);
				if(password.Equals(getPassword))
				{
					Console.WriteLine("paswoord are equal");
					return true;
				}
				reader.Close();
				connection.Close();
			}
			catch(SqlException  ex) 
			{
				Console.WriteLine("error: " + ex.Message);
			}
			
			Console.WriteLine("passwords not equal exit");
			return false;
		}
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]

        public IActionResult Index(string userid,string password,Users user)
        {
			try
			{
				//Console.WriteLine("in post index: ");
				Console.WriteLine("userid: from string " + userid);
				//Console.WriteLine("password: " + password);
				//Console.WriteLine("user.id: " + user.userId);
				if (validateUser(userid, password))
				{
					Console.WriteLine("validaed user");
					Console.WriteLine("userid in index page "+user.userId);
                    Console.WriteLine("userid: from string " + userid);
                    return RedirectToAction("userPage","User",user);
				}
				else
				{
					Console.WriteLine("paswword mismatch");
					return View(); 
				}

			}
			catch
			{
				return View();
			}
		}
		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}