using System.Collections.Generic;

namespace WebAPI_ViewModel.Identity
{
    public class ApplicationRoleViewModel : DefaultViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string Description { get; set; }
    }

    public class AssignRoleViewModel
    {
        public string UserId { get; set; }

        public List<string> RoleId { get; set; }
    }

    public class UserHasRoleViewModel
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }
    }

}
