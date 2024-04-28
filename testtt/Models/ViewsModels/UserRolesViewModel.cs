namespace testtt.Models.ViewsModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserFName { get; set; }
        public string UserLName { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
}
