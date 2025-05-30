﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Models.Models
{
    public partial class ContentManagement
    {
        [Key]
        public long ContentManagementId { get; set; }
        public int? PageCode { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string Title { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}