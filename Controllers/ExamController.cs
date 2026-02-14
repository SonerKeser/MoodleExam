using Microsoft.AspNetCore.Mvc;
using MoodleExam.Models;
using MoodleExam.Utils;

namespace MoodleExam.Controllers
{
    public class ExamController : Controller
    {
        private readonly sinavContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExamController(sinavContext context, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _environment = environment;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            if (AuthControl())
            {
                return View(GetExams());
            }
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Create()
        {
            if (AuthControl())
            {
                return View(MoodleAPIClient.Courses(_httpClientFactory));
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Create(string ExamName, int? CourseId, int? TimeLimit, DateTime? FirstAccessTime, DateTime? LastAccessTime, string Answers, IFormFile? ExamPdf, IFormFile? AnswerPdf)
        {
            if (ExamName != null && CourseId != null && TimeLimit != null && FirstAccessTime != null && LastAccessTime != null && Answers != null && ExamPdf != null && AnswerPdf != null)
            {
                try
                {
                    string id = Guid.NewGuid().ToString();
                    string examPdfName = id + Path.GetExtension(ExamPdf.FileName);
                    string answerPdfName = id + Path.GetExtension(AnswerPdf.FileName);
                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", examPdfName), FileMode.Create))
                    {
                        ExamPdf.CopyTo(stream);
                    }
                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", answerPdfName), FileMode.Create))
                    {
                        AnswerPdf.CopyTo(stream);
                    }
                    var exam = new Exam() { Name = ExamName, Courseid = CourseId.Value, Timelimit = TimeLimit.Value, Firstaccesstime = FirstAccessTime.Value, Lastaccesstime = LastAccessTime.Value, Answers = Answers, ExamPdf = examPdfName, AnswerPdf = answerPdfName };
                    _context.Exams!.Add(exam);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
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
            return View(MoodleAPIClient.Courses(_httpClientFactory));
        }

        public IActionResult Edit(int? id)
        {
            if (AuthControl())
            {
                if (id != null)
                {
                    var result = _context.Exams.FirstOrDefault(x => x.Id.Equals(id));
                    if (result != null)
                    {
                        ViewData["Courses"] = MoodleAPIClient.Courses(_httpClientFactory);
                        return View(result);
                    }
                }
                return RedirectToAction("NotFound", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Edit(int? ExamId, string ExamName, int? CourseId, int? TimeLimit, DateTime? FirstAccessTime, DateTime? LastAccessTime, string Answers, IFormFile? ExamPdf, IFormFile? AnswerPdf)
        {
            if (ExamId != null && ExamName != null && CourseId != null && TimeLimit != null && FirstAccessTime != null && LastAccessTime != null && Answers != null)
            {
                try
                {
                    var exam = _context.Exams.FirstOrDefault(x => x.Id.Equals(ExamId));
                    if (exam != null)
                    {
                        exam.Name = ExamName;
                        exam.Courseid = CourseId.Value;
                        exam.Timelimit = TimeLimit.Value;
                        exam.Firstaccesstime = FirstAccessTime.Value;
                        exam.Lastaccesstime = LastAccessTime.Value;
                        exam.Answers = Answers;
                        if(ExamPdf != null)
                        {
                            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", exam.ExamPdf)))
                            {
                                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", exam.ExamPdf));
                            }
                            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", exam.ExamPdf), FileMode.Create))
                            {
                                ExamPdf.CopyTo(stream);
                            }
                        }
                        if(AnswerPdf != null)
                        {
                            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", exam.AnswerPdf)))
                            {
                                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", exam.AnswerPdf));
                            }
                            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", exam.AnswerPdf), FileMode.Create))
                            {
                                AnswerPdf.CopyTo(stream);
                            }
                        }
                        _context.SaveChanges();
                        return RedirectToAction("Index");
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
            return View(MoodleAPIClient.Courses(_httpClientFactory));
        }

        public IActionResult Start(int? id, int? UserId)
        {
            if (id != null && UserId != null)
            {
                try
                {
                    var exam = _context.Exams!.FirstOrDefault(x => x.Id.Equals(id.Value));
                    if (exam != null)
                    {
                        var result = _context.Userexams!.FirstOrDefault(x => x.Examid.Equals(id.Value) && x.Userid.Equals(UserId.Value));
                        if (exam.Firstaccesstime < DateTime.Now && exam.Lastaccesstime > DateTime.Now && result == null)
                        {
                            ViewData["UserId"] = UserId.Value;
                            Response.Cookies.Append("StartTime", DateTime.Now.ToString());
                            if (_environment.IsDevelopment())
                            {
                                ViewData["IsDevelopment"] = true;
                            }
                            return View(exam);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = true;
                    TempData["message"] = ex.Message;
                }
            }
            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult End(Dictionary<string, string>? body)
        {
            if (body != null)
            {
                try
                {
                    string myAnswersStr = "";
                    Dictionary<int, string> myAnswers = new();
                    foreach (var item in body)
                    {
                        if (int.TryParse(item.Key, out int i))
                        {
                            myAnswers.Add(i, item.Value);
                            if (myAnswersStr.Equals(""))
                            {
                                myAnswersStr += i + "." + item.Value;
                            }
                            else
                            {
                                myAnswersStr += "-" + i + "." + item.Value;
                            }
                        }
                    }
                    var exam = _context.Exams!.FirstOrDefault(x => x.Id.Equals(int.Parse(body["ExamId"])));
                    if (exam != null)
                    {
                        Dictionary<int, string> answers = new();
                        var result = exam.Answers.Split('-');
                        foreach (var item in result)
                        {
                            var answer = item.Split('.');
                            answers.Add(int.Parse(answer[0]), answer[1]);
                        }
                        double trueAnswerCount = 0;
                        double falseAnswerCount = 0;
                        int nullAnswerCount = answers.Count - myAnswers.Count;
                        foreach (var myAnswer in myAnswers)
                        {
                            if (answers[myAnswer.Key].Equals(myAnswer.Value))
                            {
                                trueAnswerCount++;
                            }
                            else
                            {
                                falseAnswerCount++;
                            }
                        }
                        double calculatedAnswerCount = trueAnswerCount - (falseAnswerCount / 4);
                        var userExam = new Userexam() { Userid = int.Parse(body["UserId"]), Examid = int.Parse(body["ExamId"]), Answers = myAnswersStr, Score = $"D={trueAnswerCount}|Y={falseAnswerCount}|B={nullAnswerCount}|N={calculatedAnswerCount}", Endtime = DateTime.Now, Starttime = Convert.ToDateTime(Request.Cookies["StartTime"]) };
                        _context.Userexams!.Add(userExam);
                        _context.SaveChanges();
                        return RedirectToAction("Result", new { id = userExam.Id });
                    }
                }
                catch(Exception ex)
                {
                    TempData["error"] = true;
                    TempData["message"] = ex.Message;
                }
            }
            return RedirectToAction("NotFound", "Home");
        }

        public IActionResult Report(int? id)
        {
            if(id != null)
            {
                var result = _context.Userexams.Where(x => x.Examid.Equals(id)).ToList();
                if (result != null)
                {
                    if (MoodleAPIClient.TokenData == null)
                    {
                        MoodleAPIClient.TokenData = MoodleAPIClient.Token(_httpClientFactory);
                    }
                    ViewData["token"] = MoodleAPIClient.TokenData.token;
                    return View(result);
                }
            }
            return RedirectToAction("NotFound", "Home");
        }

        public IActionResult Result(int? id)
        {
            if (id != null)
            {
                var result = _context.Userexams!.FirstOrDefault(x => x.Id.Equals(id.Value));
                if (result != null)
                {
                    var exam = _context.Exams!.FirstOrDefault(x => x.Id.Equals(result.Examid));
                    if(exam != null)
                    {
                        ViewData["Answers"] = exam.Answers;
                        ViewData["ExamPdf"] = exam.ExamPdf;
                        ViewData["AnswerPdf"] = exam.AnswerPdf;
                        if (_environment.IsDevelopment())
                        {
                            ViewData["IsDevelopment"] = true;
                        }
                        return View(result);
                    }
                }
            }
            return RedirectToAction("NotFound", "Home");
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                try
                {
                    var exam = _context.Exams!.FirstOrDefault(x => x.Id.Equals(id.Value));
                    if (exam != null)
                    {
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", exam.ExamPdf)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/exams", exam.ExamPdf));
                        }
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", exam.AnswerPdf)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/answers", exam.AnswerPdf));
                        }
                        _context.Exams!.Remove(exam);
                        _context.SaveChanges();
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
            return RedirectToAction("Index");
        }

        private bool AuthControl()
        {
            var cookie = Request.Cookies.FirstOrDefault(x => x.Key.Equals("auth"));
            if (cookie.Value != null)
            {
                if (CryptoClient.Decrypt(cookie.Value).Equals("Admin_User"))
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

        private List<Exam> GetExams()
        {
            var exams = new List<Exam>();
            if (_context.Exams != null)
            {
                foreach (var exam in _context.Exams)
                {
                    var courses = MoodleAPIClient.Course(_httpClientFactory, exam.Courseid).courses;
                    if(courses != null && courses.Count > 0)
                    {
                        ViewData[exam.Courseid.ToString()] = courses[0].displayname;
                        exams.Add(exam);
                    }
                }
            }
            return exams;
        }
    }
}
