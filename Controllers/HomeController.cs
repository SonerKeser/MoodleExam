using Microsoft.AspNetCore.Mvc;
using MoodleExam.Models;
using MoodleExam.Utils;

namespace MoodleExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly sinavContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(sinavContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index(int? courseId, int? userId)
        {
            if (courseId != null && userId != null)
            {
                try
                {
                    List<UserCourse> userCourses = MoodleAPIClient.UserCourses(_httpClientFactory, userId.Value);
                    var course = userCourses.FirstOrDefault(x => x.id.Equals(courseId));
                    if (course != null)
                    {
                        ViewData["UserId"] = userId.Value;
                        List<Exam> exams = _context.Exams!.Where(x => x.Courseid.Equals(course.id)).ToList();
                        foreach (Exam exam in exams)
                        {
                            var result = _context.Userexams!.FirstOrDefault(x => x.Userid.Equals(userId.Value) && x.Examid.Equals(exam.Id));
                            if (result != null)
                            {
                                ViewData[exam.Id.ToString()] = result.Id;
                            }
                        }
                        return View(exams);
                    }
                }
                catch(Exception ex)
                {
                    TempData["error"] = true;
                    TempData["message"] = ex.Message;
                }
            }
            return RedirectToAction("NotFound");
        }

        public IActionResult Login()
        {
            if (AuthControl())
            {
                return RedirectToAction("Index", "Exam");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username != null && password != null)
            {
                try
                {
                    if (MoodleAPIClient.MoodleAdmin!.username!.Equals(username) && MoodleAPIClient.MoodleAdmin!.password!.Equals(password))
                    {
                        Response.Cookies.Append("auth", CryptoClient.Encrypt("Admin_User"), new CookieOptions() { Expires = DateTime.Now.AddDays(7) });
                        return RedirectToAction("Index", "Exam");
                    }
                    else
                    {
                        ViewData["notUser"] = true;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["error"] = true;
                    ViewData["message"] = ex.Message;
                }
            }
            else
            {
                ViewData["nulled"] = true;
            }
            return View();
        }

#pragma warning disable CS0114 // Üye devralınmış üyeyi gizler; geçersiz kılma anahtar sözcüğü eksik
        public IActionResult NotFound()
        {
            return View();
        }
#pragma warning restore CS0114 // Üye devralınmış üyeyi gizler; geçersiz kılma anahtar sözcüğü eksik

        private bool AuthControl()
        {
            var cookie = Request.Cookies.FirstOrDefault(x => x.Key.Equals("auth"));
            if(cookie.Value != null)
            {
                if(CryptoClient.Decrypt(cookie.Value).Equals("Admin_User"))
                {
                    return true;
                }
                else
                {
                    Response.Cookies.Delete("auth");
                }
            }
            return false;
        }
    }
}