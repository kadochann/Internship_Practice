using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Models;

[Table("Sale")]
public partial class Sale
{
    [Key]
    [Column("sale_id")]
    public int SaleId { get; set; }

    [Column("store_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string StoreId { get; set; } = null!;

    [Column("order_num")]
    [StringLength(20)]
    [Unicode(false)]
    public string OrderNum { get; set; } = null!;

    [Column("order_date", TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column("quantity")]
    public short Quantity { get; set; }

    [Column("pay_terms")]
    [StringLength(12)]
    [Unicode(false)]
    public string PayTerms { get; set; } = null!;

    [Column("book_id")]
    public int BookId { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Sales")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("StoreId")]
    [InverseProperty("Sales")]
    public virtual Store Store { get; set; } = null!;
}
