﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Models.Models
{
    public partial class CustomerEmergencyContact
    {
        [Key]
        public long EmergencyContactId { get; set; }
        public long CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Number { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerEmergencyContact")]
        public virtual Customer Customer { get; set; }
    }
}