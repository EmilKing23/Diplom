namespace DiplomKarakuyumjyan
{
    public class UserConfiguration
    {
        public static bool Authorized = false;

        public static UserTypes Usertype;

       public  enum UserTypes
        {
            Admin,
            Client,
            Employer,
            Manager
        }
    }
}