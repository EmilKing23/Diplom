namespace DiplomKarakuyumjyan
{
    public class UserConfiguration
    {
        public static bool Authorized = false;

        public static UserTypes Usertype;

        public static User UserInfo;  

       public  enum UserTypes
        {
            Admin,
            Employer,
            Manager
        }

        public class User
        {
            public string Name { get; set; }
            public string SurName { get; set; }
            public UserTypes Role { get; set; } 
        }

    }
}