using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    [Column("role_id")]
    public short RoleId { get; set; }

    [Column("role_desc")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleDesc { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
