using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HealthLayby.Admin
{
    public static class RouteConfig
    {
        public static void ConfigRoute(this WebApplication webApplication)
        {
            webApplication.MapControllerRoute
            (
                name: "resetPassword",
                pattern: "reset-password",
                defaults: new { controller = "Login", action = "ResetPassword" }
            );

            webApplication.MapControllerRoute
            (
                name: "forgotPassword",
                pattern: "forgot-password",
                defaults: new { controller = "Login", action = "ForgotPassword" }
            );

            webApplication.MapControllerRoute
            (
                name: "updateProfile",
                pattern: "update-profile",
                defaults: new { controller = "Admin", action = "UpdateProfile" }
            );

            webApplication.MapControllerRoute
            (
                name: "changePassword",
                pattern: "change-password",
                defaults: new { controller = "Admin", action = "ChangePassword" }
            );

            webApplication.MapControllerRoute
            (
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}"
            );
        }
    }
}
