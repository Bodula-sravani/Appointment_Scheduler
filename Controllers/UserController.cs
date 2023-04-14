using Appointment_Scheduler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;

namespace Appointment_Scheduler.Controllers
{
	public class UserController : Controller
	{

		IConfiguration configuration;
		public SqlConnection connection;
		public UserController(IConfiguration configuration)
		{
			this.configuration = configuration;
			this.connection = new SqlConnection(configuration.GetConnectionString("DB"));

		}
		// GET: UserController
		public ActionResult Index()
		{
			return View();
		}

		// GET: UserController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: UserController/Create
		public ActionResult Register()
		{
			return View();
		}
		public void insertUser(Users user)
		{
			Console.WriteLine("inside insert user");
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("insertUser", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@userid", user.userId);
				command.Parameters.AddWithValue("@username", user.Name);
				command.Parameters.AddWithValue("@useremail", user.Email);
				command.Parameters.AddWithValue("@userpassword", user.Password);
				command.Parameters.AddWithValue("@userphonenumber", user.phoneNumber);

				command.ExecuteNonQuery();
				connection.Close();
			}
			catch(SqlException e)
			{
				Console.WriteLine("error: " + e.Message);
			}
			Console.WriteLine(user.phoneNumber);
			Console.WriteLine("exiting insert user");
		}
		// POST: UserController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(Users user)
		{
			try
			{
				Console.WriteLine("phone nmber: "+user.phoneNumber);
				insertUser(user);
				Console.WriteLine("compeleted insert");
				//return RedirectToAction("Home/Index");
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return View();
			}
		}

		public Users getUser(string userId)
		{
            Console.WriteLine("entered getUser method");
            Users user = new Users();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userId);
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("reader excecuted");
                while (reader.Read())
                {
                    user.Password = (string)reader["userPassword"];
					user.phoneNumber = (string)reader["userPhonenumber"];
					user.userId = (string)reader["userId"];
					user.Email = (string)reader["userEmail"];	
                }
                reader.Close();
                connection.Close();
                
            }
            catch (SqlException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            return user;
        }
		public ActionResult userPage(string userId)
		{
			
			Users user = getUser(userId);
			ViewData["name"] = user.Name;
            Console.WriteLine("user Name: " + user.Name);
            return View(user);
		}

		public ActionResult Create(int id)
		{
			return View();
		}

		// POST: UserController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}


		// GET: UserController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: UserController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: UserController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: UserController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
