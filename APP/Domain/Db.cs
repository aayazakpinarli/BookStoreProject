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
                .HasOne(userEntity => userEntity.Country) // each User entity has one related Country entity
                .WithMany(countryEntity => countryEntity.Users) // each Country entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CountryId) // FK in the User entity references PK in Country entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related User entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.City) // each User entity has one related City entity
                .WithMany(cityEntity => cityEntity.Users) // each City entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CityId) // FK in the User entity references PK in City entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a City entity if there are related User 

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.User) // each UserRole entity has one related User entity
                .WithMany(userEntity => userEntity.UserRoles) // each User entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.UserId) // FK in the UserRole entity references PK in the User entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a User entity if there are related UserRole entitie

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.Role) // each UserRole entity has one related Role entity
                .WithMany(roleEntity => roleEntity.UserRoles) // each Role entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.RoleId) // FK in the UserRole entity references PK in Role entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Role entity if there are related UserRole entities

            modelBuilder.Entity<BookGenre>()
                .HasOne(bookGenreEntity => bookGenreEntity.Book) // each BookGenre entity has one related Book entity
                .WithMany(bookEntity => bookEntity.BookGenres) // each Book entity has many related BookGenre entities
                .HasForeignKey(bookGenreEntity => bookGenreEntity.BookId) // FK in the BookGenre entity references PK in the Book entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Book entity if there are related BookGenre entities

            modelBuilder.Entity<BookGenre>()
                .HasOne(bookGenreEntity => bookGenreEntity.Genre) // each BookGenre entity has one related Book entity
                .WithMany(genreEntity => genreEntity.BookGenres) // each Book entity has many related BookGenre entities
                .HasForeignKey(bookGenreEntity => bookGenreEntity.GenreId) // FK in the BookGenre entity references PK in the Book entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Genre entity if there are related BookGenre 

            modelBuilder.Entity<Book>()
                .HasMany(bookEntity => bookEntity.Authors) // each Book entity has many related Author entities
                .WithMany(authorEntity => authorEntity.Books) // each Author entity has many related Book entities
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor", // name of the join table 
                    j => j.HasOne<Author>()
                          .WithMany()
                          .HasForeignKey("AuthorId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Book>()
                          .WithMany()
                          .HasForeignKey("BookId")
                          .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}