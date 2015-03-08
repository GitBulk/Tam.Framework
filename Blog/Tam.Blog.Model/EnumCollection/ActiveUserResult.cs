namespace Tam.Blog.Model.EnumCollection
{
    public enum ActiveUserResult
    {
        /// <summary>
        /// User is not found
        /// </summary>
        NotFound = 0,

        /// <summary>
        /// Usertoken not match
        /// </summary>
        NotMatch,

        /// <summary>
        /// Success active user
        /// </summary>
        Success
    }
}