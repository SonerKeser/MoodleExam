using MoodleExam.Models;
using Newtonsoft.Json;

namespace MoodleExam.Utils
{
    public class MoodleAPIClient
    {
        public static MoodleAdmin? MoodleAdmin { get; set; }
        public static TokenData? TokenData { get; set; }

        public static TokenData Token(IHttpClientFactory httpClientFactory)
        {
            try
            {
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    HttpResponseMessage responseData = client.GetAsync(new Uri($"https://ue.yenitrendakademi.com/login/token.php?service=moodle_mobile_app&moodlewsrestformat=json&username={MoodleAdmin!.username}&password={MoodleAdmin!.password}")).Result;
                    TokenData data = JsonConvert.DeserializeObject<TokenData>(responseData.Content.ReadAsStringAsync().Result) ?? new TokenData();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA: " + ex.Message);
                return new TokenData();
            }
        }

        public static List<UserCourse> UserCourses(IHttpClientFactory httpClientFactory, int userid)
        {
            try
            {
                if(TokenData == null)
                {
                    TokenData = Token(httpClientFactory);
                }
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    HttpResponseMessage responseData = client.GetAsync(new Uri($"https://ue.yenitrendakademi.com/webservice/rest/server.php?moodlewsrestformat=json&wsfunction=core_enrol_get_users_courses&wstoken={TokenData!.token}&userid={userid}")).Result;
                    List<UserCourse> courses = JsonConvert.DeserializeObject<List<UserCourse>>(responseData.Content.ReadAsStringAsync().Result) ?? new List<UserCourse>();
                    return courses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA: " + ex.Message);
                return new List<UserCourse>();
            }
        }

        public static List<Course> Courses(IHttpClientFactory httpClientFactory)
        {
            try
            {
                if (TokenData == null)
                {
                    TokenData = Token(httpClientFactory);
                }
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    HttpResponseMessage responseData = client.GetAsync(new Uri($"https://ue.yenitrendakademi.com/webservice/rest/server.php?moodlewsrestformat=json&wsfunction=core_course_get_courses&wstoken={TokenData!.token}")).Result;
                    List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(responseData.Content.ReadAsStringAsync().Result) ?? new List<Course>();
                    return courses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA: " + ex.Message);
                return new List<Course>();
            }
        }

        public static List<User> User(IHttpClientFactory httpClientFactory, int userId)
        {
            try
            {
                if (TokenData == null)
                {
                    TokenData = Token(httpClientFactory);
                }
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    HttpResponseMessage responseData = client.GetAsync(new Uri($"https://ue.yenitrendakademi.com/webservice/rest/server.php?moodlewsrestformat=json&wsfunction=core_user_get_users_by_field&wstoken={TokenData!.token}&field=id&values[0]={userId}")).Result;
                    List<User> data = JsonConvert.DeserializeObject<List<User>>(responseData.Content.ReadAsStringAsync().Result) ?? new List<User>();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA: " + ex.Message);
                return new List<User>();
            }
        }

        public static CourseList Course(IHttpClientFactory httpClientFactory, int courseId)
        {
            try
            {
                if (TokenData == null)
                {
                    TokenData = Token(httpClientFactory);
                }
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    HttpResponseMessage responseData = client.GetAsync(new Uri($"https://ue.yenitrendakademi.com/webservice/rest/server.php?moodlewsrestformat=json&wsfunction=core_course_get_courses_by_field&wstoken={TokenData!.token}&field=id&value={courseId}")).Result;
                    CourseList data = JsonConvert.DeserializeObject<CourseList>(responseData.Content.ReadAsStringAsync().Result) ?? new CourseList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA: " + ex.Message);
                return new CourseList();
            }
        }
    }
}
