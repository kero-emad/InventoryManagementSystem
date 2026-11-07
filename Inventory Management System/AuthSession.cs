using Inventory_Management_System.Models;

namespace Inventory_Management_System
{
    public static class AuthSession
    {
        public static User CurrentUser { get; set; }

        public static bool IsAdmin
        {
            get
            {
                return CurrentUser != null && CurrentUser.Role == "Admin";
            }
        }
    }
}
