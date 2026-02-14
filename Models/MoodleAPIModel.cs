namespace MoodleExam.Models
{
    public class MoodleAdmin
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }

    public class TokenData
    {
        public string? token { get; set; }
        public string? privatetoken { get; set; }
    }

    public class Overviewfile
    {
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public int filesize { get; set; }
        public string? fileurl { get; set; }
        public int timemodified { get; set; }
        public string? mimetype { get; set; }
    }

    public class UserCourse
    {
        public int id { get; set; }
        public string? shortname { get; set; }
        public string? fullname { get; set; }
        public string? displayname { get; set; }
        public int enrolledusercount { get; set; }
        public string? idnumber { get; set; }
        public int visible { get; set; }
        public string? summary { get; set; }
        public int summaryformat { get; set; }
        public string? format { get; set; }
        public bool showgrades { get; set; }
        public string? lang { get; set; }
        public bool enablecompletion { get; set; }
        public bool completionhascriteria { get; set; }
        public bool completionusertracked { get; set; }
        public int category { get; set; }
        public double? progress { get; set; }
        public bool completed { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public int marker { get; set; }
        public int lastaccess { get; set; }
        public bool isfavourite { get; set; }
        public bool hidden { get; set; }
        public List<Overviewfile>? overviewfiles { get; set; }
        public bool showactivitydates { get; set; }
        public bool showcompletionconditions { get; set; }
    }

    public class Courseformatoption
    {
        public string? name { get; set; }
        public int value { get; set; }
    }

    public class Customfield
    {
        public string? name { get; set; }
        public string? shortname { get; set; }
        public string? type { get; set; }
        public object? valueraw { get; set; }
        public string? value { get; set; }
    }

    public class Course
    {
        public int id { get; set; }
        public string? shortname { get; set; }
        public int categoryid { get; set; }
        public int categorysortorder { get; set; }
        public string? fullname { get; set; }
        public string? displayname { get; set; }
        public string? idnumber { get; set; }
        public string? summary { get; set; }
        public int summaryformat { get; set; }
        public string? format { get; set; }
        public int showgrades { get; set; }
        public int newsitems { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public int numsections { get; set; }
        public int maxbytes { get; set; }
        public int showreports { get; set; }
        public int visible { get; set; }
        public int groupmode { get; set; }
        public int groupmodeforce { get; set; }
        public int defaultgroupingid { get; set; }
        public int timecreated { get; set; }
        public int timemodified { get; set; }
        public int enablecompletion { get; set; }
        public int completionnotify { get; set; }
        public string? lang { get; set; }
        public string? forcetheme { get; set; }
        public List<Courseformatoption>? courseformatoptions { get; set; }
        public bool showactivitydates { get; set; }
        public bool? showcompletionconditions { get; set; }
        public List<Customfield>? customfields { get; set; }
        public int? hiddensections { get; set; }
    }

    public class Contact
    {
        public int id { get; set; }
        public string? fullname { get; set; }
    }

    public class CourseItem
    {
        public int id { get; set; }
        public string? fullname { get; set; }
        public string? displayname { get; set; }
        public string? shortname { get; set; }
        public int categoryid { get; set; }
        public string? categoryname { get; set; }
        public int sortorder { get; set; }
        public string? summary { get; set; }
        public int summaryformat { get; set; }
        public List<object>? summaryfiles { get; set; }
        public List<Overviewfile>? overviewfiles { get; set; }
        public bool showactivitydates { get; set; }
        public bool showcompletionconditions { get; set; }
        public List<Contact>? contacts { get; set; }
        public List<string>? enrollmentmethods { get; set; }
        public List<Customfield>? customfields { get; set; }
        public string? idnumber { get; set; }
        public string? format { get; set; }
        public int showgrades { get; set; }
        public int newsitems { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public int maxbytes { get; set; }
        public int showreports { get; set; }
        public int visible { get; set; }
        public int groupmode { get; set; }
        public int groupmodeforce { get; set; }
        public int defaultgroupingid { get; set; }
        public int enablecompletion { get; set; }
        public int completionnotify { get; set; }
        public string? lang { get; set; }
        public string? theme { get; set; }
        public int marker { get; set; }
        public int legacyfiles { get; set; }
        public string? calendartype { get; set; }
        public int timecreated { get; set; }
        public int timemodified { get; set; }
        public int requested { get; set; }
        public int cacherev { get; set; }
        public List<Filter>? filters { get; set; }
        public List<Courseformatoption>? courseformatoptions { get; set; }
    }

    public class Filter
    {
        public string? filter { get; set; }
        public int localstate { get; set; }
        public int inheritedstate { get; set; }
    }

    public class CourseList
    {
        public List<CourseItem>? courses { get; set; }
        public List<object>? warnings { get; set; }
    }

    public class Preference
    {
        public string? name { get; set; }
        public object? value { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? fullname { get; set; }
        public string? email { get; set; }
        public string? department { get; set; }
        public int firstaccess { get; set; }
        public int lastaccess { get; set; }
        public string? auth { get; set; }
        public bool suspended { get; set; }
        public bool confirmed { get; set; }
        public string? lang { get; set; }
        public string? theme { get; set; }
        public string? timezone { get; set; }
        public int mailformat { get; set; }
        public string? description { get; set; }
        public int descriptionformat { get; set; }
        public string? profileimageurlsmall { get; set; }
        public string? profileimageurl { get; set; }
        public List<Preference>? preferences { get; set; }
    }
}
