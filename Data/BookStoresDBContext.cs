using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookStoresWebAPI.Models;
using BookStoresWebAPI.Models.Identity;

namespace BookStoresWebAPI.Data;

public partial class BookStoresDbContext : IdentityDbContext<AppUser, AppRole, int>
{

    public BookStoresDbContext(DbContextOptions<BookStoresDbContext> options)
    :base(options)
    { 
    }//constructor for dependency injection

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KADO-PC; Database=BookStoresDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Ensures Identity configurations are applied

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Author__86516BCFCC610736");

            entity.Property(e => e.Phone)
                .HasDefaultValue("UNKNOWN")
                .IsFixedLength();
            entity.Property(e => e.State).IsFixedLength();
            entity.Property(e => e.Zip).IsFixedLength();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__490D1AE10D2C2049");

            entity.Property(e => e.PublishedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Type)
                .HasDefaultValue("UNDECIDED")
                .IsFixedLength();

            entity.HasOne(d => d.Pub).WithMany(p => p.Books).HasConstraintName("FK__Book__pub_id__5165187F");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors).HasConstraintName("FK__BookAutho__autho__52593CB8");

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors).HasConstraintName("FK__BookAutho__book___534D60F1");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Job__6E32B6A5C786464E");

            entity.Property(e => e.JobDesc).HasDefaultValue("New Position - title not formalized yet");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PubId).HasName("PK__Publishe__2515F2224DDAEE4C");

            entity.Property(e => e.Country).HasDefaultValue("USA");
            entity.Property(e => e.State).IsFixedLength();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens).HasConstraintName("FK__RefreshTo__user___5441852A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__Role__760965CC21849C50");
            entity.Property(e => e.Id).HasColumnName("role_id"); // RoleId yok, Id ile eþliyoruz
            entity.Property(e => e.RoleDesc)
                  .HasDefaultValue("New Position - title not formalized yet");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK_Sale2");

            entity.Property(e => e.StoreId).IsFixedLength();

            entity.HasOne(d => d.Book).WithMany(p => p.Sales).HasConstraintName("FK__Sale__book_id__5535A963");

            entity.HasOne(d => d.Store).WithMany(p => p.Sales).HasConstraintName("FK__Sale__store_id__5629CD9C");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("UPK_storeid");

            entity.Property(e => e.StoreId).IsFixedLength();
            entity.Property(e => e.State).IsFixedLength();
            entity.Property(e => e.Zip).IsFixedLength();
        });

        /*modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_user_id_2")
                .IsClustered(false);

            entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MiddleName).IsFixedLength();
            entity.Property(e => e.PubId).HasDefaultValue(1);
            entity.Property(e => e.RoleId).HasDefaultValue((short)1);

            entity.HasOne(d => d.Pub).WithMany(p => p.Users).HasConstraintName("FK__User__pub_id__5812160E");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK__User__role_id__571DF1D5");
        }); */
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("user_id");
            entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MiddleName).IsFixedLength();
            entity.Property(e => e.PubId).HasDefaultValue(1);
            entity.Property(e => e.RoleId).HasDefaultValue(1);
            entity.HasOne(d => d.Pub).WithMany(p => p.Users)
            .HasForeignKey(d => d.PubId)
            .HasConstraintName("FK__User__pub_id__5812160E");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__role_id__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
