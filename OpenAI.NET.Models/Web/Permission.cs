namespace OpenAI.NET.Models.Web
{
    /// <summary>
    /// Access rights to OpenAI.NET.Web.
    /// </summary>
    public static class Permission
    {
        /// <summary>
        /// No rights.
        /// </summary>
        public const string None = "None";
        /// <summary>
        /// A user who can be deleted by a user with same role.
        /// </summary>
        public const string Untouchable = "Untouchable";
        /// <summary>
        /// Ability to call API actions.
        /// </summary>
        public const string CanCallApi = "CanCallApi";
        /// <summary>
        /// Ability to manage users.
        /// </summary>
        public const string CanManageUsers = "CanManageUsers";
        /// <summary>
        /// All rights.
        /// </summary>
        public const string All = CanCallApi + "," + CanManageUsers + "," + Untouchable;
    }
}