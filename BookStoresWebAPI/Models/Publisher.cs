using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Models;

[Table("Publisher")]
public partial class Publisher
{
    [Key]
    [Column("pub_id")]
    public int PubId { get; set; }

    [Column("publisher_name")]
    [StringLength(40)]
    [Unicode(false)]
    public string? PublisherName { get; set; }

    [Column("city")]
    [StringLength(20)]
    [Unicode(false)]
    public string? City { get; set; }

    [Column("state")]
    [StringLength(2)]
    [Unicode(false)]
    public string? State { get; set; }

    [Column("country")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Country { get; set; }

    [InverseProperty("Pub")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    [InverseProperty("Pub")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
