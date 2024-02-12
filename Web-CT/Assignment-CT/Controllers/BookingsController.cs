using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using Assignment_CT.Models;
using EllipticCurve;
using Microsoft.AspNet.Identity;
using Rotativa;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Assignment_CT.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const String API_KEY = "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";



        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return View(db.Booking.ToList());
            }
            // If the user is in the Patient role, retrieve only their bookings
            else if (User.IsInRole("Patient"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                var userBookings = db.Booking.Where(b => b.UserId == currentUserId).ToList();
                return View(userBookings);
            }
            else if (User.IsInRole("Doctor"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                var userBookings = db.Booking.Where(b => b.DoctorId == currentUserId).ToList();
                return View(userBookings);
            }
            return HttpNotFound();
        }

        // Source: https://www.c-sharpcorner.com/article/export-pdf-from-html-in-mvc-net4/
        // Author: Mukesh Kumar
        public ActionResult ExportPDF()
        {
            List<Booking> Data = null;
            if (User.IsInRole("admin"))
            {
                Data = db.Booking.ToList();
            }
            // If the user is in the Patient role, retrieve only their bookings
            else if (User.IsInRole("Patient"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                Data = db.Booking.Where(b => b.UserId == currentUserId).ToList();
            }
            else if (User.IsInRole("Doctor"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                Data = db.Booking.Where(b => b.DoctorId == currentUserId).ToList();
            }

            return new PartialViewAsPdf("ExportPDF", Data)
            {
                FileName = "BookingLists.pdf"
            };
        }

        // Source: https://www.c-sharpcorner.com/article/export-data-in-excel-file-with-asp-net-mvc/
        // Author: Mukesh Kumar
        public ActionResult ExportToExcel()
        {
            List<Booking> Data = null;
            if (User.IsInRole("admin"))
            {
                Data = db.Booking.ToList();
            }
            // If the user is in the Patient role, retrieve only their bookings
            else if (User.IsInRole("Patient"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                Data = db.Booking.Where(b => b.UserId == currentUserId).ToList();
            }
            else if (User.IsInRole("Doctor"))
            {
                string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
                Data = db.Booking.Where(b => b.DoctorId == currentUserId).ToList();
            }
            var gv = new GridView();
            gv.DataSource = Data;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BookingLists.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }

        public ActionResult DoctorBookingsChartData()
        {
            var doctorRoleId = db.Roles.Where(r => r.Name == "Doctor").Select(r => r.Id).FirstOrDefault();

            var allDoctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == doctorRoleId))
                                  .Select(u => new { u.Id, u.FirstName, u.LastName }).ToList();

            // Now, group Bookings by DoctorId and count
            var doctorBookingData = db.Booking
                                        .GroupBy(b => b.DoctorId)
                                        .Select(group => new
                                        {
                                            DoctorId = group.Key,
                                            Count = group.Count(),
                                            AverageRating = group.Where(b => b.Rated).Average(b => (decimal?)b.Rating) ?? 0
                                        }).ToList();

            var result = allDoctors.Select(doctor => new
            {
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                Count = doctorBookingData.FirstOrDefault(b => b.DoctorId == doctor.Id)?.Count ?? 0,
                AverageRating = doctorBookingData.FirstOrDefault(b => b.DoctorId == doctor.Id)?.AverageRating ?? 0
            }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET: Bookings/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var booking = db.Booking.Include(b => b.Result).SingleOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }



        // GET: Bookings/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }
            var viewModel = new BookingViewModel
            {
                Booking = new Booking(),
                DoctorEventsList = db.DoctorEvents.ToList() // Retrieve all doctor events
            };

            ViewBag.Doctors = GetDoctorsSelectList();

            return View(viewModel);
        }

        public SelectList GetDoctorsSelectList()
        {
            var doctorRoleId = db.Roles.Where(r => r.Name == "Doctor").Select(r => r.Id).FirstOrDefault();

            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == doctorRoleId))
                                  .Select(u => new { u.Id, u.FirstName }).ToList();

            return new SelectList(doctors, "Id", "FirstName");
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.S
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "BookingId,AppointmentDate,FirstName,LastName,DoctorId,Description,ImageUpload,EventId")] Booking booking)
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (db.Booking.Any(b => b.UserId == userId && DbFunctions.TruncateTime(b.AppointmentDate) == DbFunctions.TruncateTime(booking.AppointmentDate)))
                {
                    ModelState.AddModelError("AppointmentDate", "You already have a booking on this date.");
                    ViewBag.Doctors = GetDoctorsSelectList();
                    return View(booking);
                }

                if (booking.ImageUpload != null && booking.ImageUpload.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(booking.ImageUpload.InputStream))
                    {
                        booking.Image = binaryReader.ReadBytes(booking.ImageUpload.ContentLength);
                    }
                }
                else
                {
                    Console.WriteLine("No valid image provided.");
                }

                booking.UserId = userId;
                ApplicationUser user = db.Users.Find(userId);
                if (user != null)
                {
                    booking.Email = user.Email;
                }
                db.Booking.Add(booking);

                var oneEvent = db.DoctorEvents.Find(booking.EventId);
                oneEvent.Booked = true;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Doctors = GetDoctorsSelectList();

            return View(booking);
        }

        [HttpGet]
        public JsonResult GetDoctorEvents(string doctorId)
        {
            var events = db.DoctorEvents.Where(d => d.DoctorId == doctorId)
                            .Select(d => new
                            {
                                id = d.EventId,
                                start = d.StartTime,
                                end = d.EndTime,
                                isBooked = d.Booked,
                                title = "Available" // You can customize the title
                            }).ToList();

            return Json(events, JsonRequestBehavior.AllowGet);
        }


        // GET: Bookings/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Booking model)
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }

            var booking = db.Booking.Find(model.BookingId);
            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.AppointmentDate = model.AppointmentDate;
            booking.FirstName = model.FirstName;
            booking.LastName = model.LastName;
            booking.Email = model.Email;
            booking.Description = model.Description;
            booking.ImageUpload = model.ImageUpload;
            if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
            {
                using (var binaryReader = new BinaryReader(model.ImageUpload.InputStream))
                {
                    booking.Image = binaryReader.ReadBytes(model.ImageUpload.ContentLength);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Bookings/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }
            Booking booking = db.Booking.Find(id);
            DoctorEvents OneEvent = db.DoctorEvents.Find(booking.EventId);
            OneEvent.Booked = false;
            db.Booking.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Bookings/EditResult
        [HttpGet]
        [Authorize]
        public ActionResult EditResult(int? id)
        {
            if (!User.IsInRole("Doctor"))
            {
                return HttpNotFound();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var booking = db.Booking.Find(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.Image != null)
            {
                var base64Image = Convert.ToBase64String(booking.Image);
                ViewBag.ImageData = string.Format("data:image/png;base64,{0}", base64Image);
            }

            var userId = User.Identity.GetUserId();
            ApplicationUser loggedInUser = db.Users.FirstOrDefault(u => u.Id == userId);

            // Retrieve the doctor's name
            var doctorName = loggedInUser != null ? (loggedInUser.FirstName + " " + loggedInUser.LastName) : "Unknown Doctor";

            var viewModel = new Result
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                DoctorName = doctorName,
                UserId = booking.UserId,
                Email = booking.Email,
                BookingId = booking.BookingId
            };

            return View(viewModel);
        }

        // Post: Bookings/EditResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditResult([Bind(Include = "ResultId,FirstName,LastName,DoctorName,ResultDescription,ResultDate,Email,BookingId")] Result result, HttpPostedFileBase fileAttachment)
        {
            var fromEmail = "";

            if (ModelState.IsValid)
            {
                result.DoctorId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (user != null)
                {
                    fromEmail = user.Email;
                }
                result.ResultDate = DateTime.Today;
                db.Results.Add(result);

                var booking = db.Booking.Find(result.BookingId);
                if (booking != null)
                {
                    booking.Result = result;
                }

                db.SaveChanges();
                if (fileAttachment != null && fileAttachment.ContentLength > 0)
                {
                    Send(fromEmail, result.Email, "Result", result.ResultDescription, fileAttachment);
                    ViewBag.Result = "Email has been send.";
                    return View(result);
                }

                Send(fromEmail, result.Email, "Result", result.ResultDescription, fileAttachment);
                ViewBag.Result = "Email has been send.";
                return View(result);
            }
            return View(result);
        }

        public async Task Send(String fromEmail, String toEmail, String subject, String contents, HttpPostedFileBase fileAttachment)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress(fromEmail, "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            if (fileAttachment != null && fileAttachment.ContentLength > 0)
            {
                byte[] fileData = new byte[fileAttachment.ContentLength];
                fileAttachment.InputStream.Read(fileData, 0, fileAttachment.ContentLength);
                var fileBase64 = Convert.ToBase64String(fileData);
                msg.AddAttachment(fileAttachment.FileName, fileBase64, fileAttachment.ContentType);

            }

            var response = await client.SendEmailAsync(msg);
        }

        // GET: Bookings/BulkEmail
        public ActionResult BulkEmail()
        {
            var fromEmail = "";
            ApplicationUser doctor = db.Users.Find(User.Identity.GetUserId());
            if (doctor != null)
            {
                fromEmail = doctor.Email;
            }
            var bookings = db.Booking.Select(b => new CheckboxViewModel
            {
                UserId = b.UserId,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Email = b.Email,
                DoctorId = b.DoctorId,
                IsChecked = false,
                DoctorName = doctor.FirstName
            }).ToList();
            return View(bookings);
        }

        // POST: Bookings/BulkEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> BulkEmail(List<CheckboxViewModel> bookings, string emailContent, HttpPostedFileBase fileAttachment)
        {
            
            var fromEmail = "";
            if (bookings == null || !bookings.Any())
            {
                // No email addresses were selected
                return View(); // Return to the same view with an appropriate message if needed
            }
            
            ApplicationUser doctor = db.Users.Find(User.Identity.GetUserId());
            if (doctor != null)
            {
                fromEmail = doctor.Email;
            }


            foreach (var item in bookings)
            {
                if (item.IsChecked == true)
                {
                    // This is just an example. In a real-world scenario, you would get more user details, maybe from the booking list
                    var user = db.Users.FirstOrDefault(u => u.Email == item.Email); // Replace 'Users' with your actual DbSet<User>

                    // Send email
                    await Send(fromEmail, item.Email, "Results", emailContent, fileAttachment);
                    ViewBag.Result = "Email has been send.";// Save the results to the database

                    // Log the result
                    var result = new Result
                    {
                        FirstName = user.FirstName, // Assuming your user has a first name
                        LastName = user.LastName,   // Assuming your user has a last name
                        UserId = user.Id,           // Assuming your user has an Id
                        DoctorName = doctor.FirstName,    // Example doctor name
                        DoctorId = doctor.Id,     // Example doctor id
                        ResultDescription = emailContent, // This is just a sample description
                        ResultDate = DateTime.Now,
                        Email = item.Email
                    };

                    db.Results.Add(result);
                }
            }

            await db.SaveChangesAsync();
            

            return View(bookings); ; // Redirect to some action after completing the task
        }




        [HttpGet]
        [Authorize(Roles = "Patient")]
        public ActionResult Rate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the booking using the provided ID
            var booking = db.Booking.Include(b => b.Result).SingleOrDefault(b => b.BookingId == id);

            // Check if the booking exists, has already been rated, or does not belong to the logged-in patient
            if (booking == null || booking.Rated || booking.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            // Otherwise, provide an interface to rate the booking
            ViewBag.BookingId = id;
            return View(booking);
        }


        [HttpPost]
        [Authorize(Roles = "Patient")]
        public ActionResult Rate(int BookingId, int rate)
        {
            var booking = db.Booking.Find(BookingId);
            if (booking == null || booking.Rated || booking.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            booking.Rating = rate;
            booking.Rated = true;
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = BookingId });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
