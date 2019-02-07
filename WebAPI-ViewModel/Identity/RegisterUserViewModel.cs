namespace WebAPI_ViewModel.Identity
{
    public class RegisterUserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }
    }

    public class AddNewUserViewModel : DefaultViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

}
