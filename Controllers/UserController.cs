using Appointment_Scheduler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data;

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
					user.Name = (string)reader["userName"];
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
		public ActionResult userPage(Users user)
		{
            Console.WriteLine("user id in user page method: " + user.userId);
            Users currentUser = getUser(user.userId);
			
			//ViewData["name"] = user1.Name;
            Console.WriteLine("user Name: " + currentUser.Name);
            return View(currentUser);
		}

		public ActionResult Create(Users user)
		{
			Console.WriteLine("in create method id: " + user.userId);
			return View();
		}

		public void insertAppointment(Appointment a)
		{
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("addAppointment", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@date", a.Date);
                command.Parameters.AddWithValue("@start", a.StartTime);
                command.Parameters.AddWithValue("@end", a.EndTime);
                command.Parameters.AddWithValue("@user", a.userId);
                command.Parameters.AddWithValue("@title", a.Title);
                command.Parameters.AddWithValue("@des", a.Description);

				command.ExecuteNonQuery();
				connection.Close();

            }
			catch(SqlException ex) 
			{
				Console.WriteLine("error: " + ex.Message);
			}
		}
		// POST: UserController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Users user,Appointment a)
		{

			try
			{
                insertAppointment(a);
				Console.WriteLine("using user obj in create post method: user id: " + user.userId);
				return RedirectToAction("userPage", "User",user);
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
