using HealthLayby.Helpers.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.ApiViewModels.Customer.Response
{
    /// <summary>
    /// EmergencyContactResponse
    /// </summary>
    public class EmergencyContactResponse
    {
        /// <summary>
        /// Gets or sets the emergency contact identifier.
        /// </summary>
        /// <value>
        /// The emergency contact identifier.
        /// </value>
        public long EmergencyContactId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the emergency.
        /// </summary>
        /// <value>
        /// The first name of the emergency.
        /// </value>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the emergency.
        /// </summary>
        /// <value>
        /// The last name of the emergency.
        /// </value>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the emergency mobile number.
        /// </summary>
        /// <value>
        /// The emergency mobile number.
        /// </value>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact email.
        /// </summary>
        /// <value>
        /// The emergency contact email.
        /// </value>
        public string Email { get; set; } = string.Empty;
    }
}
