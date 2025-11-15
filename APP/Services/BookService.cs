using APP.Domain;
using APP.Models;
using CORE.APP.Domain;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace APP.Services
{
    public class BookService : Service<Book>, IService<BookRequest, BookResponse>
    {
        public BookService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Book> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking) // will return Books DbSet
                .Include(book => book.BookAuthors).ThenInclude(bookAuthor => bookAuthor.Author)
                .Include(book => book.BookGenres).ThenInclude(bookGenre => bookGenre.Genre)
                .OrderByDescending(book => book.BookName)
                .ThenByDescending(book => book.PublishedOn);
        }

        public List<BookResponse> List()
        {

            return Query(false).Select(book => new BookResponse 
            {
                Id = book.Id,
                Guid = book.Guid,
                BookName = book.BookName,
                Description = book.Description,
                NumberOfPages = book.NumberOfPages,
                StockAmount = book.StockAmount,
                PublishedOn = book.PublishedOn,
                IsTopSeller = book.IsTopSeller,
                PriceF = book.Price.ToString("C2"),
                StockAmountF = (book.StockAmount ?? 0).ToString(),
                BookGenres = book.BookGenres.Select(bg => bg.Genre.GenreName).ToList(),
                Authors = book.BookAuthors.Select(ba => ba.Author.LastName).ToList()
            }).ToList();
        }

        public BookResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(book => book.Id == id);
            if (entity is null)
                return null;

            return new BookResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                BookName = entity.BookName,
                Description = entity.Description,
                NumberOfPages = entity.NumberOfPages,
                StockAmount = entity.StockAmount,
                PublishedOn = entity.PublishedOn,
                IsTopSeller = entity.IsTopSeller,
                Price = entity.Price,
                PriceF = entity.Price.ToString("C2"),
                StockAmountF = (entity.StockAmount ?? 0).ToString(),
                Authors = entity.BookAuthors.Select(ba => ba.Author.LastName).ToList(),
                BookGenres = entity.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
            };
        }

        public BookRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(book => book.Id == id);
            if (entity is null)
                return null;

            return new BookRequest
            {
                Id = entity.Id,
                BookName = entity.BookName,
                Description = entity.Description,
                NumberOfPages = entity.NumberOfPages,
                StockAmount = entity.StockAmount,
                PublishedOn = entity.PublishedOn,
                IsTopSeller = entity.IsTopSeller,
                BookPrice = entity.Price,
                AuthorIds = entity.BookAuthors.Select(a => a.AuthorId).ToList(),
                GenreIds = entity.BookGenres.Select(bg => bg.GenreId).ToList()
            };
        }


        public CommandResponse Create(BookRequest request)
        {
            if (Query().Any(book => book.BookName == request.BookName.Trim()))
                return Error("Book with the same name exists!");

            var entity = new Book
            {
                BookName = request.BookName.Trim(),
                Description = request.Description,
                NumberOfPages = request.NumberOfPages,
                StockAmount = request.StockAmount,
                PublishedOn = request.PublishedOn,
                IsTopSeller = request.IsTopSeller,
                Price = request.BookPrice
            };

            if (request.AuthorIds != null && request.AuthorIds.Any())
            {
                entity.BookAuthors = request.AuthorIds
                    .Select(id => new BookAuthor { AuthorId = id })
                    .ToList();
            }

            if (request.GenreIds != null && request.GenreIds.Any())
            {
                entity.BookGenres = request.GenreIds
                    .Select(id => new BookGenre { GenreId = id })
                    .ToList();
            }

            Create(entity);

            return Success("Book created successfully.", entity.Id);
        }



        public CommandResponse Update(BookRequest request)
        {
            if (Query().Any(book => book.Id != request.Id && book.BookName == request.BookName.Trim()))
                return Error("Book with the same name exists!");

            var entity = Query(false).SingleOrDefault(book => book.Id == request.Id); 
            if (entity is null)
                return Error("Book is not found!");

            // delete the relations 
            Delete(entity.BookGenres); 
            entity.BookGenres = request.GenreIds
                .Select(id => new BookGenre { GenreId = id })
                .ToList();

            Delete(entity.BookAuthors);
            entity.BookAuthors = request.AuthorIds
                .Select(id => new BookAuthor { AuthorId = id })
                .ToList();

            // update 
            entity.BookName = request.BookName.Trim();
            entity.Description = request.Description;
            entity.NumberOfPages = request.NumberOfPages;
            entity.StockAmount = request.StockAmount;
            entity.PublishedOn = request.PublishedOn;
            entity.IsTopSeller = request.IsTopSeller;
            entity.Price = request.BookPrice;

            Update(entity); 

            return Success("Book updated successfully.", entity.Id);
        }

        
        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(book => book.Id == id);
            if (entity is null)
                return Error("Book is not found!");

            Delete(entity.BookAuthors);
            Delete(entity.BookGenres);
            Delete(entity); 

            return Success("Book is deleted successfully.", entity.Id);
        }



        public List<BookResponse> List(BookQueryRequest request)
        {
            var query = Query();

            if (!string.IsNullOrWhiteSpace(request.BookName))
                query = query.Where(b => b.BookName.Contains(request.BookName));

            if (request.AuthorIds != null && request.AuthorIds.Any())
                query = query.Where(b => b.BookAuthors.Any(ba => request.AuthorIds.Contains(ba.Id)));

            if (request.GenreIds != null && request.GenreIds.Any())
                query = query.Where(b => b.BookGenres.Any(bg => request.GenreIds.Contains(bg.GenreId)));

            if (request.MinPrice.HasValue)
                query = query.Where(b => b.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= request.MaxPrice.Value);

            if (request.PublishedTo.HasValue)
                query = query.Where(b => b.PublishedOn <= request.PublishedTo.Value);

            if (request.IsTopSeller.HasValue)
                query = query.Where(b => b.IsTopSeller == request.IsTopSeller.Value);

            if (request.HasStock.HasValue && request.HasStock.Value)
                query = query.Where(b => b.StockAmount > 0);

            return query
                .ToList()
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Guid = b.Guid,
                    BookName = b.BookName,
                    Description = b.Description,
                    NumberOfPages = b.NumberOfPages,
                    StockAmount = b.StockAmount,
                    PublishedOn = b.PublishedOn,
                    IsTopSeller = b.IsTopSeller,
                    PriceF = b.Price.ToString("C2"),
                    StockAmountF = (b.StockAmount ?? 0).ToString(),
                    Authors = b.BookAuthors.Select(a => a.Author.LastName).ToList(),
                    BookGenres = b.BookGenres.Select(bg => bg.Genre.GenreName).ToList()
                })
                .ToList();

        }

    }
}