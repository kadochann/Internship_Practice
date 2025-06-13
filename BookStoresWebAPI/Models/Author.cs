using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Models;

[Table("Author")]
public partial class Author
{
    [Key]
    [Column("author_id")]
    public int AuthorId { get; set; }

    [Column("last_name")]
    [StringLength(40)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("first_name")]
    [StringLength(20)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("phone")]
    [StringLength(12)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("address")]
    [StringLength(40)]
    [Unicode(false)]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(20)]
    [Unicode(false)]
    public string? City { get; set; }

    [Column("state")]
    [StringLength(2)]
    [Unicode(false)]
    public string? State { get; set; }

    [Column("zip")]
    [StringLength(5)]
    [Unicode(false)]
    public string? Zip { get; set; }

    [Column("email_address")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EmailAddress { get; set; }

    [InverseProperty("Author")]
    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
