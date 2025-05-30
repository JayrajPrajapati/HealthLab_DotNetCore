﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Models.Models
{
    public partial class CustomerCreditCard
    {
        [Key]
        public long CreditCardId { get; set; }
        public long CustomerId { get; set; }
        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string NameOnCard { get; set; }
        public long Month { get; set; }
        public long Year { get; set; }
        [Required]
        [StringLength(5)]
        public string CCV { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerCreditCard")]
        public virtual Customer Customer { get; set; }
    }
}