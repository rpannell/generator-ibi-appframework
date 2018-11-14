namespace IBI.<%= Name %>.Service.Services.Interfaces
{
    /// <summary>
    /// Definition of the UserInformationService
    /// </summary>
    public interface IUserInformationService
    {
        #region Methods

        /// <summary>
        /// Returns the ActiveDirectoryId of the currently logged in user
        /// </summary>
        /// <returns>int</returns>
        int GetCurrentUserActiveDirectoryId();

        /// <summary>
        /// Returns the username of the currently logged in user
        /// </summary>
        /// <returns>string</returns>
        string GetCurrentUserName();

        /// <summary>
        /// returns true or false if the user has a role or not
        /// </summary>
        /// <param name="role">The name of the role</param>
        /// <param name="plugin">The plugin name (defaults to <%= Name %>)</param>
        /// <returns>boolean</returns>
        bool IsInRole(string role, string plugin = "<%= Name %>");

        #endregion Methods
    }
}