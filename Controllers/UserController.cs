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

		
		public List<Appointment> getdateAppointments(string userid)
		{
            // All appointments of that user
            Console.WriteLine("entered get date appointmensts method");
			List<Appointment> appointmentsList = new List<Appointment>();
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("getdateAppointments", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@userid", userid);
				command.Parameters.AddWithValue("@date", DateTime.Now);
				SqlDataReader reader = command.ExecuteReader();
				Console.WriteLine("reader excecuted");
				while (reader.Read())
				{
					Appointment a = new Appointment();

					a.Id = (int)reader["Id"];
					a.Title = (string)reader["title"];
					a.Description = (string)reader["description"];
					a.Date = (DateTime)reader["Date_of_appointment"];
					a.StartTime = (TimeSpan)reader["startTime"];
					a.EndTime = (TimeSpan)reader["EndTime"];
					a.userId = (string)reader["userId"];

					appointmentsList.Add(a);

				}
				reader.Close();
				connection.Close();

			}
			catch (SqlException ex)
			{
				Console.WriteLine("error: " + ex.Message);
			}
			
			return appointmentsList;
		}
		public ActionResult userPage(string userId)
		{
            Console.WriteLine("user id in user page method: " + userId);
            Users currentUser = getUser(userId);
			ViewBag.appointmentList = getdateAppointments(userId);
			Console.WriteLine("user Name: " + currentUser.Name);

            return View(currentUser);
		}

		public ActionResult Create(string userId)
		{
			Console.WriteLine("in create method id: " + userId);
			ViewBag.userId = userId;  //may be this can be used check once by removing userid in html page
			return View();
		}

		public void insertAppointment(Appointment a)
		{
			try
			{
				Console.WriteLine("entered insert appointment method");
				connection.Open();
				SqlCommand command = new SqlCommand("addAppointment", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@date", a.Date);
                command.Parameters.AddWithValue("@start", a.StartTime);
                command.Parameters.AddWithValue("@end", a.EndTime);
                command.Parameters.AddWithValue("@user", a.userId);
                command.Parameters.AddWithValue("@title", a.Title);
                command.Parameters.AddWithValue("@des", a.Description);
				Console.WriteLine("completed parameters");
				command.ExecuteNonQuery();
				connection.Close();
				Console.WriteLine("executed the query");
                Console.WriteLine("exit insert method");
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
		public List<Appointment> GetAppointments(string userid)
		{
			Console.WriteLine("entered get appointmensts method");
			List<Appointment> appointmentsList = new List<Appointment>();
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("GetAppointments", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@userid", userid);
				SqlDataReader reader = command.ExecuteReader();
				Console.WriteLine("reader excecuted");
				while (reader.Read())
				{
					Appointment a = new Appointment();

					a.Id = (int)reader["Id"];
					a.Title = (string)reader["title"];
					a.Description = (string)reader["description"];
					a.Date = (DateTime)reader["Date_of_appointment"];
					a.StartTime = (TimeSpan)reader["startTime"];
					a.EndTime = (TimeSpan)reader["EndTime"];
					a.userId = (string)reader["userId"];

					appointmentsList.Add(a);

				}
				reader.Close();
				connection.Close();

			}
			catch (SqlException ex)
			{
				Console.WriteLine("error: " + ex.Message);
			}
			return appointmentsList;
		}
		public ActionResult List(string userId)
		{
			if(userId==null)
			{
				userId = (string)Request.Query["userId"];
			}
			try
			{
				Console.WriteLine("in list method userid after query" + userId);
				ViewBag.userId = userId;	
				return View(GetAppointments(userId));
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return View();
			}
		}

		public Appointment GetAppointment(int id)
		{
			Console.WriteLine("entered get an appointmenst method");
			Appointment appointment = new Appointment();
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand("GetAppointment", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@id", id);
				SqlDataReader reader = command.ExecuteReader();
				Console.WriteLine("reader excecuted");
				while (reader.Read())
				{
					appointment.Id = (int)reader["Id"];
					appointment.Title = (string)reader["title"];
					appointment.Description = (string)reader["description"];
					appointment.Date = (DateTime)reader["Date_of_appointment"];
					appointment.StartTime = (TimeSpan)reader["startTime"];
					appointment.EndTime = (TimeSpan)reader["EndTime"];
					appointment.userId = (string)reader["userId"];
				}
				reader.Close();
				connection.Close();

			}
			catch (SqlException ex)
			{
				Console.WriteLine("error: " + ex.Message);
			}
			return appointment;

		}
		// GET: UserController/Details/5
		public ActionResult Details(int Id)
		{
			return View(GetAppointment(Id));
		}
		// GET: UserController/Edit/5
		public ActionResult Edit(int Id)
		{
			Console.WriteLine("Entered edit method");
			Console.WriteLine("id of app in edit method :" + Id);
			ViewBag.userId = GetAppointment(Id).userId;
			return View(GetAppointment(Id));
		}
		public void updateAppointment(Appointment appointment)
		{
            try
            {
				Console.WriteLine("entered update method");
                connection.Open();
                SqlCommand command = new SqlCommand("updateAppointment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", appointment.Id);
                command.Parameters.AddWithValue("@date", appointment.Date);
                command.Parameters.AddWithValue("@start", appointment.StartTime);
                command.Parameters.AddWithValue("@end", appointment.EndTime);
                command.Parameters.AddWithValue("@user", appointment.userId);
                command.Parameters.AddWithValue("@title", appointment.Title);
                command.Parameters.AddWithValue("@des", appointment.Description);

                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            Console.WriteLine("exit update method");
        }
		// POST: UserController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Users user,Appointment a)
		{
			try
			{
				updateAppointment(a);
				Console.WriteLine("edit post userid from user: " + user.userId);
				return RedirectToAction("List", "User",user);  //not working userid is not being passed
			}
			catch
			{
				return View();
			}
		}

		// GET: UserController/Delete/5
		public ActionResult Delete(int id)
		{
			Console.WriteLine("id in delete" + id);
            ViewBag.userId = GetAppointment(id).userId;
            return View(GetAppointment(id));
		}

		public void deleteAppointment(int id)
		{
			try
			{
				Console.WriteLine("entered delete");
				connection.Open();
				SqlCommand command = new SqlCommand("deleteAppointment", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@id", id);
				command.ExecuteNonQuery();
				connection.Close();
			}
			catch (SqlException ex)
			{
				Console.WriteLine("error: " + ex.Message);

			}
			Console.WriteLine("exit delete");
		}
		// POST: UserController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id,Users user, Appointment a)
		{
			try
			{
				Console.WriteLine("enters delete post method");
				Console.WriteLine("id in delete post: " + id);
				deleteAppointment(id);
				return RedirectToAction("userPage","User",user); //not working userid is not being passed
            }
			catch
			{
				return View();
			}
		}
	}
}
