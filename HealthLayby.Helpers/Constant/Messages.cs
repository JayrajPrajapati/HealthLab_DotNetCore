namespace HealthLayby.Helpers.Constant
{
    /// <summary>
    /// Message Constant
    /// </summary>
    public static class MessageConstant
    {
        #region Common Messages

        /// <summary>
        /// The project name
        /// </summary>
        public const string ProjectName = "Health Layby";

        /// <summary>
        /// The admin profile update success
        /// </summary>
        public const string UpdateAdminProfileSuccess = "Profile updated successfully.";

        /// <summary>
        /// The admin old password error
        /// </summary>
        public const string AdminOldPasswordError = "Old password not correct!";

        /// <summary>
        /// The admin new password success
        /// </summary>
        public const string AdminNewPasswordSuccess = "Changed password updated successfully.";

        /// <summary>
        /// The success message key
        /// </summary>
        public const string SuccessMessageKey = "SuccessMessage";

        /// <summary>
        /// The error message key
        /// </summary>
        public const string ErrorMessageKey = "ErrorMessage";

        /// <summary>
        /// The information message key
        /// </summary>
        public const string InfoMessageKey = "InfoMessage";

        /// <summary>
        /// The required
        /// </summary>
        public const string Required = "{0} cannot be blank!";

        /// <summary>
        /// The not valid
        /// </summary>
        public const string NotValid = "{0} is not valid!";

        /// <summary>
        /// The password not valid
        /// </summary>
        public const string PasswordNotValid = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)!";

        /// <summary>
        /// The common error message
        /// </summary>
        public const string CommonErrorMessage = "Oops! Something went wrong!";

        /// <summary>
        /// The invalid modal state
        /// </summary>
        public const string InvalidModalState = "Please fill all required fields!";

        /// <summary>
        /// The al ready exist
        /// </summary>
        public const string AlreadyExist = "{0} already exist!";

        /// <summary>
        /// The in valid file type
        /// </summary>
        public const string InValidFileType = "Please select a valid (.jpg, .jpeg, .png) file!";

        /// <summary>
        /// The profile pic
        /// </summary>
        public const string RequiredProfilePic = "Please upload profile picture!";

        /// <summary>
        /// The required logo
        /// </summary>
        public const string RequiredLogo = "Please upload logo!";
        /// <summary>
        /// The in valid file size
        /// </summary>
        public const string InValidFileSize = "Please select a file less than 2mb of size!";

        /// <summary>
        /// The compare not valid
        /// </summary>
        public const string CompareNotValid = "{0} and {1} must match!";

        /// <summary>
        /// The password minimum maximum length
        /// </summary>
        public const string PasswordMinMaxLength = "Please enter minimum 6 characters and maximum 16 characters!";

        /// <summary>
        /// The email address maximum length
        /// </summary>
        public const string EmailAddressMaxLength = "Please enter characters maximum 30 characters!";

        /// <summary>
        /// The name maximum length
        /// </summary>
        public const string NameMaxLength = "Please enter characters maximum 50 characters!";

        /// <summary>
        /// The profile pic
        /// </summary>
        public const string RequiredCategoryPic = "Please upload category picture!";
        /// <summary>
        /// The email cannot be blank
        /// </summary>
        public const string EmailCannotBeBlank = "Email cannot be blank!";

        /// <summary>
        /// The email is not valid
        /// </summary>
        public const string EmailIsNotValid = "Please enter a valid email address.";
        /// <summary>
        /// The service not available
        /// </summary>
        public const string ServiceNotAvailable = "There are no any service for this category. Please select another category";
        /// <summary>
        /// The select service
        /// </summary>
        public const string SelectService = "Please select Service!";
        /// <summary>
        /// The select service
        /// </summary>
        public const string SelectCategory = "Please select Category!";

        #endregion

        #region Login Messages

        /// <summary>
        /// The in valid email and password
        /// </summary>
        public const string InValidEmailAndPassword = "Please enter valid email and password!";

        /// <summary>
        /// The block login
        /// </summary>
        public const string BlockLogin = "Your account request is not approved by admin, please wait.";

        /// <summary>
        /// The in valid password
        /// </summary>
        public const string InValidPassword = "Please enter valid password!";

        /// <summary>
        /// The admin not found
        /// </summary>
        public const string AdminNotFound = "Admin account not found!";

        /// <summary>
        /// The reser password success
        /// </summary>
        public const string ResetPasswordSuccess = "Your reset password link has been successfully sent to registered email. Check your email for further instructions.";

        /// <summary>
        /// The reset password failed
        /// </summary>
        public const string ResetPasswordFailed = "We are unable to reset password for provided email!";

        /// <summary>
        /// The in valid reset token
        /// </summary>
        public const string InValidResetToken = "Please provide valid reset token!";

        /// <summary>
        /// The expired reset token
        /// </summary>
        public const string ExpiredResetToken = "Reset password link is expired or not valid!";

        /// <summary>
        /// The reset password success
        /// </summary>
        public const string ResetUpdatePasswordSuccess = "Your password is reset successfully!";

        #endregion

        #region Customer Messages

        /// <summary>
        /// The create customer success
        /// </summary>
        public const string CreateCustomerSuccess = "Customer has been created successfully.";

        /// <summary>
        /// The update customer success
        /// </summary>
        public const string UpdateCustomerSuccess = "Customer has been updated successfully.";

        /// <summary>
        /// The customer not found
        /// </summary>
        public const string CustomerNotFound = "Customer not found!";

        /// <summary>
        /// The delete customer success
        /// </summary>
        public const string DeleteCustomerSuccess = "Customer deleted successfully";

        /// <summary>
        /// The email alredy exist
        /// </summary>
        public const string EmailAlredyExist = "Provided email is already exist!";

        /// <summary>
        /// The phone number already exist
        /// </summary>
        public const string PhoneNumberAlreadyExist = "Provided phone number is already exist!";

        #endregion

        #region Category Message

        /// <summary>
        /// The category name alredy exist
        /// </summary>
        public const string CategoryNameAlredyExist = "Provided category name is already exist!";

        /// <summary>
        /// The create category success
        /// </summary>
        public const string CreateCategorySuccess = "Category has been created successfully.";

        /// <summary>
        /// The update category success
        /// </summary>
        public const string UpdateCategorySuccess = "Category has been updated successfully.";

        /// <summary>
        /// The category used in service fail
        /// </summary>
        public const string CategoryUsedInServiceFail = "Category used in service so you can not delete!";

        /// <summary>
        /// The category not found
        /// </summary>
        public const string CategoryNotFound = "Category not found!";

        /// <summary>
        /// The delete category success
        /// </summary>
        public const string DeleteCategorySuccess = "Category deleted successfully.";

        #endregion

        #region Content Management

        /// <summary>
        /// The update privacy policy success
        /// </summary>
        public const string UpdatePrivacyPolicySuccess = "Privacy Policy content has been updated successfully.";

        /// <summary>
        /// The update term condition success
        /// </summary>
        public const string UpdateTermConditionSuccess = "Terms & Conditions content has been updated successfully.";

        /// <summary>
        /// The update why including admin fees success
        /// </summary>
        public const string UpdateWhyIncludingAdminFeesSuccess = "Why Including Admin Fee content has been updated successfully.";

        /// <summary>
        /// The description in valid
        /// </summary>
        public const string DescriptionInValid = "Please add description!";
        /// <summary>
        /// The reject reason FAQ
        /// </summary>
        public const string RejectReasonFAQ = "Please add reject reason!";

        /// <summary>
        /// The editor image upload not allowed
        /// </summary>
        public const string EditorImageUploadNotAllowed = "Image not allowed to upload";

        #endregion

        #region FAQ

        /// <summary>
        /// The create FAQ success
        /// </summary>
        public const string CreateFAQSuccess = "FAQ has been created successfully.";

        /// <summary>
        /// The update FAQ success
        /// </summary>
        public const string UpdateFAQSuccess = "FAQ has been updated successfully.";

        /// <summary>
        /// The FAQ not found
        /// </summary>
        public const string FAQNotFound = "FAQ not found!";

        /// <summary>
        /// The delete FAQ success
        /// </summary>
        public const string DeleteFAQSuccess = "FAQ deleted successfully";

        #endregion

        #region Merchant

        /// <summary>
        /// The merchant not found
        /// </summary>
        public const string MerchantNotFound = " merchant account not found!";

        /// <summary>
        /// The reqired bank
        /// </summary>
        public const string ReqiredBank = "Please select {0}!";

        /// <summary>
        /// The merchant bank not found
        /// </summary>
        public const string MerchantBankNotFound = "Merchant bank not found!";

        /// <summary>
        /// The update merchant success
        /// </summary>
        public const string UpdateMerchantSuccess = "Merchant has been updated successfully.";

        /// <summary>
        /// The create merchant success
        /// </summary>
        public const string CreateMerchantSuccess = "Merchant has been created successfully.";

        /// <summary>
        /// The delete merchant success
        /// </summary>
        public const string DeleteMerchantSuccess = "Merchant deleted successfully.";

        /// <summary>
        /// The merchant used in service fail
        /// </summary>
        public const string MerchantUsedInServiceFail = "Merchant used in service so you can not delete!";

        /// <summary>
        /// The merchant already created
        /// </summary>
        public const string MerchantAlreadyCreated = "Merchant already exists, Please create with different credentials!";

        /// <summary>
        /// The register email success
        /// </summary>
        public const string RegisterEmailSuccess = "Your otp for registration has been successfully sent to registered email. Check your email and enter otp.";

        /// <summary>
        /// The register email failed
        /// </summary>
        public const string RegisterEmailFailed = "We are unable to send otp for registration on provided email!";

        /// <summary>
        /// The otp not match
        /// </summary>
        public const string OTPNotMatch = "Please check email again and enter correct otp.";

        /// <summary>
        /// The otp not found
        /// </summary>
        public const string OTPNotFound = "OTP not found!";

        /// <summary>
        /// The error update merchant
        /// </summary>
        public const string ErrorUpdateMerchant = "Error in updating merchant.";

        /// <summary>
        /// The otp succesfully validated
        /// </summary>
        public const string OTPSuccesfullyValidated = "Congratulations your OTP has successfully validated.";

        /// <summary>
        /// The merchant bank success
        /// </summary>
        public const string MerchantBankSuccess = "Congratulations your registration is successfully done.";

        /// <summary>
        /// The merchant bank failure
        /// </summary>
        public const string MerchantBankFailure = "Some error occur while adding bank detail in registration ,please try again with proper data.";

        /// <summary>
        /// The terms in valid
        /// </summary>
        public const string TermsInValid = "Please add terms and condition!";
        
        #endregion

        #region MerchantRequest

        /// <summary>
        /// The accept merchant request
        /// </summary>
        public const string AcceptMerchantRequest = "Merchant's request has been accepted and username & password has been successfully sent to registered email.";

        /// <summary>
        /// The reject merchant request
        /// </summary>
        public const string RejectMerchantRequestSuccess = "Merchant's request has been rejected and rejection reason has been successfully sent to registered email.";


        #endregion

        #region Service

        /// <summary>
        /// The service not found
        /// </summary>
        public const string ServiceNotFound = "Service bank not found!";

        /// <summary>
        /// The update service success
        /// </summary>
        public const string UpdateServiceSuccess = "Service has been updated successfully.";

        /// <summary>
        /// The create service success
        /// </summary>
        public const string CreateServiceSuccess = "Service has been created successfully.";

        /// <summary>
        /// The servicename already exist
        /// </summary>
        public const string ServiceNameAlreadyExist = "Service name is already exist!";

        /// <summary>
        /// The delete service success
        /// </summary>
        public const string DeleteServiceSuccess = "Service deleted successfully.";

        /// <summary>
        /// The price validation
        /// </summary>
        public const string PriceValidation = "Price must be greater than 0!";

        #endregion

        #region Rewards

        /// <summary>
        /// The reward not found
        /// </summary>
        public const string RewardNotFound = "Rewards not found!";

        /// <summary>
        /// The reward delete success
        /// </summary>
        public const string RewardDeleteSuccess = "Reward deleted successfully";

        /// <summary>
        /// The reward update success
        /// </summary>
        public const string RewardUpdateSuccess = "Reward has been updated successfully.";

        /// <summary>
        /// The reward create success
        /// </summary>
        public const string RewardCreateSuccess = "Reward has been created successfully.";

        #endregion

        #region File Upload

        /// <summary>
        /// The unsupported media types
        /// </summary>
        public const string UnsupportedMediaTypes = "Unsupported media type.";

        /// <summary>
        /// The invalid authorization
        /// </summary>
        public const string InvalidAuthorization = "Invalid Authorization.";

        /// <summary>
        /// The internal server error
        /// </summary>
        public const string InternalServerError = "Internal server error.";

        /// <summary>
        /// The file deleted successfully
        /// </summary>
        public const string FileDeletedSuccessfully = "File deleted successfully.";

        #endregion

        #region Bank
        /// <summary>
        /// The bank BSB maximum length
        /// </summary>
        public const string BankBSBMaxLength = "Please enter BSB number of six characters !";

        /// <summary>
        /// The in valid BSB
        /// </summary>
        public const string InValidBSB = "In-correct BSBNumber.";

        #endregion

        #region Customer Credit Card
        /// <summary>
        /// The card number maximum length
        /// </summary>
        public const string CardNumberMaxLength = "Please enter card number of sixteen characters !";

        /// <summary>
        /// The month year maximum length
        /// </summary>
        public const string MonthYearMaxLength = "Please enter month / year of two characters !";

        /// <summary>
        /// The CVV maximum length
        /// </summary>
        public const string CVVMaxLength = "Please enter cvv of three characters !";
        #endregion

        #region Customer Plan
        /// <summary>
        /// The customer data not found
        /// </summary>
        public const string CustomerDataNotFound = "Customer Plan not found.";
        /// <summary>
        /// The customer plan detail successes
        /// </summary>
        public const string CustomerPlanDetailSuccesses = "Customer plan detail fetched successfully.";
        /// <summary>
        /// The agreement success message
        /// </summary>
        public const string AgreementSuccessMessage = "Agreement details fetch successfully.";
        /// <summary>
        /// The agreement failed message
        /// </summary>
        public const string AgreementFailedMessage = "Failed to agreement details";

        /// <summary>
        /// The successfull send otp
        /// </summary>
        public const string SuccessfullSendOTP = "We have succesfully otp on customer email.";

        /// <summary>
        /// The un successfull send otp
        /// </summary>
        public const string UnSuccessfullSendOTP = "Error in sending otp on customer email.";

        /// <summary>
        /// The valid plan otp
        /// </summary>
        public const string ValidPlanOTP = "OTP is validated successfully and layby plan is completed.";

        /// <summary>
        /// The in valid plan otp
        /// </summary>
        public const string InValidPlanOTP = "Error in validating otp.";
        #endregion

        #region Transaction
        /// <summary>
        /// The transaction list success
        /// </summary>
        public const string TransactionListSuccess = "transactions details fetch successfully.";
        /// <summary>
        /// The transaction list not found
        /// </summary>
        public const string TransactionListNotFound = "failed to transactions details.";
        #endregion

        #region Clinic
        /// <summary>
        /// The clinic not found
        /// </summary>
        public const string ClinicNotFound = "Clinic details Not found.";
        #endregion

        #region Merchant Profile
        /// <summary>
        /// The merchant profile success
        /// </summary>
        public const string MerchantProfileSuccess = "Merchant profile updated successfully!";

        /// <summary>
        /// The merchant profile error
        /// </summary>
        public const string MerchantProfileError = "Error in updating merchant profile!";
        #endregion

        #region Merchant Change Password
        /// <summary>
        /// The merchant change password success
        /// </summary>
        public const string MerchantChangePasswordSuccess = "Merchant password change successfully!";

        /// <summary>
        /// The merchant change password error
        /// </summary>
        public const string MerchantChangePasswordError = "Error in updating merchant password!";
        /// <summary>
        /// The merchant old new password error
        /// </summary>
        public const string MerchantOldNewPasswordError = "Error old and new password is same!";
        /// <summary>
        /// The merchant new confirm password error
        /// </summary>
        public const string MerchantNewConfirmPasswordError = "Error new and confirm password not same!";
        #endregion

    }
}
