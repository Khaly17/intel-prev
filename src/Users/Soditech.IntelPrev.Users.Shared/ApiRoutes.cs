namespace Soditech.IntelPrev.Users.Shared;

public static class UserRoutes
{
    
    public static class Account
    {
        public const string Register = "/api/account/register";
        public const string Login = "/api/account/login";
        public const string Logout = "/api/account/logout";
        public const string RefreshToken = "/api/account/refreshToken";
        public const string ConfirmEmail = "/api/account/confirmEmail";
        public const string ForgotPassword = "/api/account/forgotPassword";
        
    }
    
    public static class Users
    {
        public const string GetUsers = "/api/users";
        public const string GetUserInfo = "/api/infos/user";
        public const string GetById = "/api/users/{id:guid}";
        public const string Create = "/api/users";
        public const string Update = "/api/users/{id:guid}";
        public const string Delete = "/api/users/{id:guid}";
        public const string Count = "/api/users/count";
        public const string UserNotificationTags = "/api/users/user-notification-tags";
        public const string ResetPassword = "/api/users/resetPassword";
        public const string UpdatePassword = "/api/users/updatePassword";
        public const string ForgotPassword = "/api/users/forgotPassword";
    }
    
    public static class Roles
    {
        public const string GetAll = "/api/roles";
        public const string GetById = "/api/roles/{id:guid}";
        public const string GetUsers = "/api/roles/{id:guid}/users";
        public const string Create = "/api/roles";
        public const string Update = "/api/roles/{id:guid}";
        public const string Delete = "/api/roles/{id:guid}";
        public const string Count = "/api/roles/count";

        public const string AffectToUser = "/api/roles/users";
        public const string UnAffectToUser = "/api/roles/users/remove";

    }
    
    public static class Tenants
    {
        public const string GetAll = "/api/tenants";
        public const string GetById = "/api/tenants/{id:guid}";
        public const string Create = "/api/tenants";
        public const string Update = "/api/tenants/{id:guid}";
        public const string Delete = "/api/tenants/{id:guid}";
        public const string Count = "/api/tenants/count";
        public const string Disable = "/api/tenants/{id:guid}/disable";
        public const string Enable = "/api/tenants/{id:guid}/enable";
        public const string GetUsers = "/api/tenants/{id:guid}/users";
        public const string GetRoles = "/api/tenants/{id:guid}/roles";
        public const string GetSites = "/api/tenants/{id:guid}/sites";
    }

}