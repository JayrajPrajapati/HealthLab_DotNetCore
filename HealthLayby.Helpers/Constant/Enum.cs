using System.ComponentModel;

namespace HealthLayby.Helpers.Constant
{
    /// <summary>
    /// Enum
    /// </summary>
    public class Enum
    {
        /// <summary>
        /// merchantRequestenum
        /// </summary>
        public enum MerchantRequestEnum
        {
            /// <summary>
            /// The pending
            /// </summary>
            Pending = 1,
            /// <summary>
            /// The rejected
            /// </summary>
            Rejected = 2,
            /// <summary>
            /// The accepted
            /// </summary>
            Accepted = 3
        }

        /// <summary>
        /// Content Management Enum
        /// </summary>
        public enum ContentManagementEnum
        {
            /// <summary>
            /// The privacy policy
            /// </summary>
            PrivacyPolicy = 1,
            /// <summary>
            /// The termsconditions
            /// </summary>
            Termsconditions = 2,
            /// <summary>
            /// The why including admin fee
            /// </summary>
            WhyIncludingAdminFee = 3
        }

        /// <summary>
        /// Month Enum
        /// </summary>
        public enum MonthEnum
        {
            /// <summary>
            /// The jan
            /// </summary>
            [Description("Jan")]
            Jan = 0,
            /// <summary>
            /// The feb
            /// </summary>
            [Description("Feb")]
            Feb = 1,
            /// <summary>
            /// The march
            /// </summary>
            [Description("March")]
            March = 2,
            /// <summary>
            /// The apr
            /// </summary>
            [Description("Apr")]
            Apr = 3,
            /// <summary>
            /// The may
            /// </summary>
            [Description("May")]
            May = 4,
            /// <summary>
            /// The jun
            /// </summary>
            [Description("Jun")]
            Jun = 5,
            /// <summary>
            /// The jul
            /// </summary>
            [Description("Jul")]
            Jul = 6,
            /// <summary>
            /// The aug
            /// </summary>
            [Description("Aug")]
            Aug = 7,
            /// <summary>
            /// The sep
            /// </summary>
            [Description("Sep")]
            Sep = 8,
            /// <summary>
            /// The oct
            /// </summary>
            [Description("Oct")]
            Oct = 9,
            /// <summary>
            /// The nov
            /// </summary>
            [Description("Nov")]
            Nov = 10,
            /// <summary>
            /// The decimal
            /// </summary>
            [Description("Dec")]
            Dec = 11
        }

        /// <summary>
        /// DiscountType Enum
        /// </summary>
        public enum DiscountTypeEnum
        {
            /// <summary>
            /// The percentage
            /// </summary>
            Percentage = 0,
            /// <summary>
            /// The amount
            /// </summary>
            Amount = 1
        }

        /// <summary>
        /// PlanStatus
        /// </summary>
        public enum PlanStatus
        {
            /// <summary>
            /// The active
            /// </summary>
            Active = 0,
            /// <summary>
            /// The completed
            /// </summary>
            Completed = 1,
            /// <summary>
            /// The pause
            /// </summary>
            Pause = 2 ,
            /// <summary>
            /// The cancelled
            /// </summary>
            Cancelled = 3
        }
    }
}
