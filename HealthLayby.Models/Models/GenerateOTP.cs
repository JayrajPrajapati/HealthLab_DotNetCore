﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Models.Models
{
    public partial class GenerateOTP
    {
        [Key]
        public long OtpId { get; set; }
        public Guid GUID { get; set; }
        public long Otp { get; set; }
        public bool IsUsed { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
    }
}