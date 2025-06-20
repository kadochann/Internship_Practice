using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoresWebAPI.Models;

[Table("Role")]
public class Role : IdentityRole<int>
{
    [Column("role_desc")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleDesc { get; set; } = null!;
    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}