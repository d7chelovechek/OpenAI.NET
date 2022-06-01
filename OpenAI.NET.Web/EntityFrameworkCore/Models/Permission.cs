namespace OpenAI.NET.Web.EntityFrameworkCore.Models
{
    public static class Permission
    {
        public const string None = "None";
        public const string CanCallApi = "CanCallApi";
        public const string CanManageUsers = "CanManageUsers";
        public const string All = CanCallApi + "," + CanManageUsers;
    }
}