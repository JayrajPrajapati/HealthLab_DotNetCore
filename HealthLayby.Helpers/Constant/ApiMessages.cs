namespace HealthLayby.Helpers.Constant
{
    /// <summary>
    /// ApiMessages
    /// </summary>
    public static class ApiMessages
    {

        #region Common Message

        /// <summary>
        /// The invalid request payload
        /// </summary>
        public const string InvalidRequestPayload = "Invalid Request Payload.";

        /// <summary>
        /// The internal server error
        /// </summary>
        public const string InternalServerError = "Internal server error.";

        /// <summary>
        /// The no account exists using email
        /// </summary>
        public const string NoAccountExistsUsingEmail = "No account exists with provided email";

        /// <summary>
        /// The data retrieved successfully
        /// </summary>
        public const string DataRetrievedSuccessfully = "Data Retrieved Successfully.";
        /// <summary>
        /// The error uploading profile pic
        /// </summary>
        public const string ErrorUploadingProfilePic = "Error in Uploading Profile Pic.";

        #endregion

        #region Login Messages

        /// <summary>
        /// The incorrect email password
        /// </summary>
        public const string IncorrectEmailPassword = "Incorrect Email Address and Password.";

        /// <summary>
        /// The login successfully
        /// </summary>
        public const string LoginSuccessfully = "Successfully login.";

        #endregion

        #region Forgot Password

        /// <summary>
        /// The check inbox for otp
        /// </summary>
        public const string CheckInboxForOtp = "Please check your mail inbox for OTP.";

        #endregion

        #region Verify OTP

        /// <summary>
        /// The provided otp does not match
        /// </summary>
        public const string ProvidedOTPDoesNotMatch = "OTP does not match.";

        /// <summary>
        /// The otp verified successfully
        /// </summary>
        public const string OTPVerifiedSuccessfully = "OTP Verified successfully.";

        #endregion

        #region Change/Reset Password

        /// <summary>
        /// The existing password does not match
        /// </summary>
        public const string ExistingPasswordDoesNotMatch = "Existing password does not match.";

        /// <summary>
        /// Creates new passwordandverifypassworddoesnotmatch.
        /// </summary>
        public const string NewPasswordAndVerifyPasswordDoesNotMatch = "New password and verify new password does not match.";

        /// <summary>
        /// Creates new passwordandexistingpassmustbedifferent.
        /// </summary>
        public const string NewPasswordAndExistingPassMustbeDifferent = "New password and existing password must be different.";

        /// <summary>
        /// The password updated successfully
        /// </summary>
        public const string PasswordUpdatedSuccessfully = "Your password has been updated changed successfully.";

        /// <summary>
        /// The password does not updated
        /// </summary>
        public const string PasswordDoesNotUpdated = "Password does not updated.";

        #endregion

        #region Update Customer Profile

        /// <summary>
        /// The profile updated successfully
        /// </summary>
        public const string ProfileUpdatedSuccessfully = "Profile updated successfully.";

        /// <summary>
        /// The profile not updated
        /// </summary>
        public const string ProfileNotUpdated = "Profile not updated.";

        #endregion

        #region Customer Emergency Contact

        /// <summary>
        /// The emergency contact saved successfully
        /// </summary>
        public const string EmergencyContactSavedSuccessfully = "Emergency Contact details saved successfully.";

        /// <summary>
        /// The emergency contact not saved
        /// </summary>
        public const string EmergencyContactNotSaved = "Emergency Contact details not saved.";

        /// <summary>
        /// The emergency contact deleted successfully
        /// </summary>
        public const string EmergencyContactDeletedSuccessfully = "Emergency Contact deleted successfully.";

        /// <summary>
        /// The emergency contact not deleted
        /// </summary>
        public const string EmergencyContactNotDeleted = "Emergency Contact not deleted.";

        #endregion

        #region Customer Family Member

        /// <summary>
        /// The family member saved successfully
        /// </summary>
        public const string FamilyMemberSavedSuccessfully = "Family member details saved successfully.";

        /// <summary>
        /// The family member not saved
        /// </summary>
        public const string FamilyMemberNotSaved = "Family member details not saved.";

        /// <summary>
        /// The family member deleted successfully
        /// </summary>
        public const string FamilyMemberDeletedSuccessfully = "Family member deleted successfully.";

        /// <summary>
        /// The family member not deleted
        /// </summary>
        public const string FamilyMemberNotDeleted = "Family member not deleted.";

        #endregion

        #region SignUp

        /// <summary>
        /// The success registration
        /// </summary>
        public const string SuccessRegistration = "Registration Successfull.";
        /// <summary>
        /// The invalid registration
        /// </summary>
        public const string InvalidRegistration = "Registration not completed.";
        /// <summary>
        /// The already added email
        /// </summary>
        public const string AlreadyAddedEmail = "Email Address already exists.";

        #endregion

        #region File Upload

        /// <summary>
        /// The unsupported media types
        /// </summary>
        public const string UnsupportedMediaTypes = "Unsupported media type";

        #endregion

        #region Delete Customer Account

        /// <summary>
        /// The profile deleted successfully
        /// </summary>
        public const string ProfileDeletedSuccessfully = "Profile delete successfully.";
        #region Bank

        /// <summary>
        /// The validated BSB number
        /// </summary>
        public const string ValidBSBNumber = "BSBNumber Successfully Validated.";

        /// <summary>
        /// The profile not deleted
        /// </summary>
        public const string ProfileNotDeleted = "Profile not deleted.";

        #endregion
        /// <summary>
        /// The in valid BSB number
        /// </summary>
        public const string InValidBSBNumber = "In-correct BSBNumber.";

        /// <summary>
        /// The successfull bank
        /// </summary>
        public const string SuccessfullBank = "Your bank account has been added successfully.";

        /// <summary>
        /// The invalid bank
        /// </summary>
        public const string InvalidBank = "Failed to add your bank details.";

        /// <summary>
        /// The update bank
        /// </summary>
        public const string UpdateBank = "Your bank account has been updated successfully.";

        #endregion

        #region Customer Credit Card

        /// <summary>
        /// The successfull credit card
        /// </summary>
        public const string SuccessfullCreditCard = "Credit Card Successfully Added.";
        /// <summary>
        /// The updated credit card
        /// </summary>
        public const string UpdatedCreditCard = "Credit Card Details Updated.";
        /// <summary>
        /// The invalid credit card
        /// </summary>
        public const string InvalidNewCreditCard = "Error in adding credit card.";
        /// <summary>
        /// The invalid old credit card
        /// </summary>
        public const string InvalidOldCreditCard = "Error in updating credit card.";
        #endregion

        #region Customer Lab By Plans List
        /// <summary>
        /// The successfull plan list
        /// </summary>
        public const string SuccessfullPlanList = "Layby Plans Fetch Successfully.";
        /// <summary>
        /// The invalid plan list
        /// </summary>
        public const string InvalidPlanList = "Error in Fetching Plans.";
        /// <summary>
        /// The invalid plan list
        /// </summary>
        public const string NoPlanList = "No LaybyPlans Fetch for these Customer.";
        #endregion

        #region Merchant Listing

        /// <summary>
        /// The successfull merchant list
        /// </summary>
        public const string SuccessfullMerchantList = "Merchant Listing Fetch Successfully.";
        /// <summary>
        /// The invalid merchant list
        /// </summary>
        public const string InvalidMerchantList = "Error in Fetching Merchant.";
        #endregion

        #region Service Listing

        /// <summary>
        /// The successfull merchant list
        /// </summary>
        public const string SuccessfullServiceList = "Service Listing Fetch Successfully.";
        /// <summary>
        /// The invalid merchant list
        /// </summary>
        public const string InvalidServiceList = "Error in Fetching Service.";
        #endregion

        #region Service Details 

        /// <summary>
        /// The success service detail
        /// </summary>
        public const string SuccessServiceDetail = "Service Details Added Successfull.";
        /// <summary>
        /// The success service schedule details
        /// </summary>
        public const string SuccessServiceScheduleDetails = "Service Details & Plan Schedule Added Successfull.";
        /// <summary>
        /// The update service detail
        /// </summary>
        public const string UpdateServiceDetail = "Service Details Updated Successfully.";
        /// <summary>
        /// The update service schedule details
        /// </summary>
        public const string UpdateServiceScheduleDetails = "Service Details & Plan Schedule Updated Successfully.";
        /// <summary>
        /// The invalid service detail
        /// </summary>
        public const string InvalidServiceDetail = "Service Details not completed.";

        #endregion

        #region Customer Lab By Plans for Dashboard List
        /// <summary>
        /// The successfull plan list
        /// </summary>
        public const string SuccessfullDashboardPlanList = "Dashboard Layby Plans Fetch Successfully.";
        /// <summary>
        /// The invalid plan list
        /// </summary>
        public const string InvalidDashboardPlanList = "Error in Fetching Dashboard  Plans.";
        /// <summary>
        /// The invalid plan list
        /// </summary>
        public const string NoDashboardPlanList = "No Dashboard  LaybyPlans Fetch for these Customer.";
        #endregion
    }
}
