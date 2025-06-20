using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Models;

[Table("User")]
public class User : IdentityUser<int> // <--- T burada int oldu
{
    [Column("source")]
    [StringLength(100)]
    [Unicode(false)]
    public string Source { get; set; } = null!;

    [Column("first_name")]
    [StringLength(20)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [Column("middle_name")]
    [StringLength(1)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [Column("last_name")]
    [StringLength(30)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [Column("RoleId")]
    public int RoleId { get; set; }

    [Column("pub_id")]
    public int PubId { get; set; }

    [Column("hire_date", TypeName = "datetime")]
    public DateTime? HireDate { get; set; }

    [ForeignKey("PubId")]
    [InverseProperty("Users")]
    public virtual Publisher Pub { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
