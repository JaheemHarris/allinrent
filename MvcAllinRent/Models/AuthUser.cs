namespace MvcAllinRent.Models
{
    public class AuthUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string IdNumber { get; set; } = null!;

        public string Password { get; set; } = null!;

        /// <summary>
        /// 0: Deactivated, 1: Activated, 2: Pending
        /// </summary>
        public byte Status { get; set; }
    }

}
