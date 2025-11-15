using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    /// Represents the Entity Framework Core database context for the Book Store application domain.
    public class Db : DbContext
    {

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Username should be unique.
            modelBuilder.Entity<User>().HasIndex(userEntity => userEntity.UserName).IsUnique();

            modelBuilder.Entity<Country>().HasIndex(countryEntity => countryEntity.CountryName).IsUnique();

            modelBuilder.Entity<City>().HasIndex(cityEntity => cityEntity.CityName).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(roleEntity => roleEntity.RoleName).IsUnique();

            modelBuilder.Entity<Book>().HasIndex(userEntity => userEntity.BookName).IsUnique();

            modelBuilder.Entity<Genre>().HasIndex(genreEntity => genreEntity.GenreName).IsUnique();


            // define indices for optimizing query performance on frequently searched propertes
            modelBuilder.Entity<UserRole>().HasIndex(userRoleEntity => userRoleEntity.UserId);
            modelBuilder.Entity<UserRole>().HasIndex(userRoleEntity => userRoleEntity.RoleId);
            modelBuilder.Entity<User>().HasIndex(userEntity => new { userEntity.FirstName, userEntity.LastName });



            // Relationship configurations
            // ----------------------------

            // FK relationships and delete behaviors
            modelBuilder.Entity<City>()
                .HasOne(cityEntity => cityEntity.Country) // each City entity has one related Country entity
                .WithMany(countryEntity => countryEntity.Cities) // each Country entity has many related City entities
                .HasForeignKey(cityEntity => cityEntity.CountryId) // FK in the City entity references PK in Country entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related City entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Country) 
                .WithMany(countryEntity => countryEntity.Users) 
                .HasForeignKey(userEntity => userEntity.CountryId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.City) 
                .WithMany(cityEntity => cityEntity.Users) 
                .HasForeignKey(userEntity => userEntity.CityId) 
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.User)
                .WithMany(userEntity => userEntity.UserRoles)
                .HasForeignKey(userRoleEntity => userRoleEntity.UserId) 
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.Role) 
                .WithMany(roleEntity => roleEntity.UserRoles)
                .HasForeignKey(userRoleEntity => userRoleEntity.RoleId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bookGenreEntity => bookGenreEntity.Book)
                .WithMany(bookEntity => bookEntity.BookGenres)
                .HasForeignKey(bookGenreEntity => bookGenreEntity.BookId) 
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<BookGenre>()
                .HasOne(bookGenreEntity => bookGenreEntity.Genre) 
                .WithMany(genreEntity => genreEntity.BookGenres) 
                .HasForeignKey(bookGenreEntity => bookGenreEntity.GenreId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserBook>()
                .HasOne(userBookEntity => userBookEntity.Book)
                .WithMany(userEntity => userEntity.UserBooks)
                .HasForeignKey(userBookEntity => userBookEntity.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserBook>()
                .HasOne(userBookEntity => userBookEntity.User)
                .WithMany(bookEntity => bookEntity.UserBooks)
                .HasForeignKey(userBookEntity => userBookEntity.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}