using Microsoft.AspNetCore.Builder;

namespace HealthLayby.Merchant
{
    public static class RouteConfig
    {
        public static void ConfigRoute(this WebApplication webApplication)
        {
            webApplication.MapControllerRoute
            (
                name: "register",
                pattern: "register",
                defaults: new { controller = "Login", action = "Register" }
            );

            webApplication.MapControllerRoute
            (
                name: "forgotPassword",
                pattern: "forgot-password",
                defaults: new { controller = "Login", action = "ForgotPassword" }
            );

            webApplication.MapControllerRoute
            (
                name: "resetPassword",
                pattern: "reset-password",
                defaults: new { controller = "Login", action = "ResetPassword" }
            );

            webApplication.MapControllerRoute
            (
                name: "submitOTP",
                pattern: "submit-otp",
                defaults: new { controller = "Login", action = "SubmitOTP" }
            );

            webApplication.MapControllerRoute
            (
                name: "resendOTP",
                pattern: "resend-otp/{token}/{merchantId}",
                defaults: new { controller = "Login", action = "ResendOTP" }
            );

            webApplication.MapControllerRoute
            (
                name: "index",
                pattern: "{controller=Login}/{action=Index}/{id?}"
            );
        }
    }
}
