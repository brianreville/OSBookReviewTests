using Moq;
using Newtonsoft.Json;
using OSBookReviewWepApi.Controllers;
using OSBookReviewWepApi.Models;
using OSBookReviewWepApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OSBookReviewTests
{
    public class BookControllerandServicesTest
    {
        [Fact]
        public async void LoadAuthors_ValidCall()
        {
            var mockRepo = new Mock<IDataClass>();
            mockRepo.Setup(repo => repo.GetListAsync()).Returns(GetSampleAuthors);
            var controller = new BookController(mockRepo.Object);

            var resulttask = await controller.GetAuthorsAsync();
            var model = resulttask.ToList();
            var expectedtask = await GetSampleAuthors();

            Assert.Equal(expectedtask.Count, model.Count);

        }

        [Fact]
        public async void LoadAuthorsByName_SecondTest()
        {
            // Arrange
            var mockRepo = new Mock<IDataClass>();
            var a = await GetSampleAuthors();
            Author author = a.FirstOrDefault();

            mockRepo.Setup(repo => repo.GetListAsync(author.AuthorName)).Returns(GetSampleAuthors);
            var controller = new BookController(mockRepo.Object);

            // Act
            var resulttask = await controller.GetAuthorsByName(author.AuthorName);
            var result = resulttask.ToList();
            var model = result.Where(x => x.AuthorName.Equals(author.AuthorName)).ToList();
            var expectedtask = await GetSampleAuthors();

            List<Author> expected = new(expectedtask.ToList());
            var expectedresult = expected.Where(x => x.AuthorName.Equals(author.AuthorName)).ToList();

            // Assert
            Assert.Equal(expectedresult.Count, model.Count);

        }
        // Gets a list of the Authors books
        [Fact]
        public async void LoadBooksByAuthor_TestValidGet()
        {
            // Arrange
            // Arrange
            var mockRepo = new Mock<IDataClass>();
            var a = await GetSampleAuthors();
            Author author = a.FirstOrDefault();

            mockRepo.Setup(repo => repo.GetListAsync(author.AID)).Returns(GetSampleBooks(author.AID));
            var controller = new BookController(mockRepo.Object);

            // Act
            var resulttask = await controller.GetBooksAsync(author.AID);
            var result = resulttask.ToList();
            var model = result.Where(x => x.AID.Equals(author.AID)).ToList();
            var expectedtask = await GetSampleAuthors();

            List<Author> expected = new(expectedtask.ToList());
            var expectedresult = expected.Where(x => x.AuthorName.Equals(author.AuthorName)).ToList();

            // Assert
            Assert.Equal(expectedresult.Count, model.Count);



        }


        // Creates a sample list of authors
        private async Task<List<Author>> GetSampleAuthors()
        {
            List<Author> output = new()
            {
                new Author
                {
                    AID = 5000,
                    AuthorName = "Test",
                    PublisherName = "Test Publisher"
                },
                new Author
                {
                    AID = 5001,
                    AuthorName = "Test 2",
                    PublisherName = "Test Publisher 2"

                }
            };

            return output;
        }

        // Test for Getting a Single Book
        [Fact]
        public async void GetBook_ValidGetRequest()
        {
            // Arrange
            var mockRepo = new Mock<IDataClass>();
            BookReview p = GetSampleBooks().First();
            mockRepo.Setup(repo => repo.GetIndv(p.BDID)).Returns(GetBookReview(p.BDID));
            var controller = new BookController(mockRepo.Object);

            // Act
            var result = await controller.GetBookByID(p.BDID);
            var expected = GetSampleBooks().First();

            var resultstring = JsonConvert.SerializeObject(result);
            var expectedstring = JsonConvert.SerializeObject(expected);
            // Assert
            Assert.Equal(expectedstring, resultstring);
        }

        // method for return a single test book
        private async Task<BookReview> GetBookReview(int bdid)
        {
            var books = GetSampleBooks();
            BookReview book = new();
            foreach (BookReview b in books)
            {
                if (b.BDID.Equals(bdid))
                {
                    book = b;
                }
            }
            return book;
        }
        // Test for Adding a New Book Review
        [Fact]
        public async void AddBookReview_ValidPost()
        {
            // Arrange
            var mockRepo = new Mock<IDataClass>();
            BookReview p = GetSampleBooks().First();

            var controller = new BookController(mockRepo.Object);

            // Act
            var result = await controller.AddBookReview(p);

            //Assert
            mockRepo.Verify(x => x.AddAsync(p), Times.Once());
        }

        // Test for Updating Book Review
        [Fact]
        public async void UpdateBookReview_ValidPost()
        {
            // Arrange
            var mockRepo = new Mock<IDataClass>();
            BookReview p = GetSampleBooks().First();
            var controller = new BookController(mockRepo.Object);

            // Act
            var result = await controller.UpdateBookReview(p);

            //Assert
            mockRepo.Verify(x => x.UpdateAsync(p), Times.Once());
        }

        // Test for Updating Multiple Books
        [Fact]
        public async void UpdateMultipleBookReview_ValidPost()
        {
            //Arrange
            var mockRepo = new Mock<IDataClass>();
            var books = GetSampleBooks();

            var controller = new BookController(mockRepo.Object);

            // Act
            var result = await controller.UpdateBookReviewList(books);

            // Assert
            mockRepo.Verify(x => x.UpdateAsync(books), Times.Once());

        }


        private List<BookReview> GetSampleBooks()
        {
            List<BookReview> books = new()
            {
                new BookReview{

                    BDID = 1,
                    Userid = 1,
                    Rating = 4,
                    OverallRating = 10,
                    PublisherName = "Test Publisher",
                    BookName ="Test",
                    AuthorName="Test Author",
                    Username="Brian",
                    ImageUrlS="test1",
                    ImageUrlM="test1",
                    ImageUrlL="test1",
                    ReviewRemarks = "Testing Remarks 1"
                },
                new BookReview
                {
                    BDID = 2,
                    Userid = 1,
                    Rating = 4,
                    OverallRating = 10,
                    PublisherName = "Test Publisher 2",
                    BookName ="Test 2",
                    AuthorName="Test Author2",
                    Username="Daniel",
                    ImageUrlS="test2",
                    ImageUrlM="test2",
                    ImageUrlL="test2",
                    ReviewRemarks = "Testing Remarks 2"
                }
            };
            return books;
        }

        private async Task<List<BookReview>> GetSampleBooks(int AID)
        {

            List<BookReview> books = new()
            {
                new BookReview{

                    BDID = 1,
                    Userid = 1,
                    Rating = 4,
                    OverallRating = 10,
                    PublisherName = "Test Publisher",
                    BookName = "Test",
                    AuthorName = "Test Author",
                    Username = "Brian",
                    ImageUrlS = "test1",
                    ImageUrlM = "test1",
                    ImageUrlL = "test1",
                    ReviewRemarks = "Testing Remarks 1",
                    AID = 5000

                },
                new BookReview
                {
                    BDID = 2,
                    Userid = 1,
                    Rating = 4,
                    OverallRating = 10,
                    PublisherName = "Test Publisher 2",
                    BookName = "Test 2",
                    AuthorName = "Test Author2",
                    Username = "Daniel",
                    ImageUrlS = "test2",
                    ImageUrlM = "test2",
                    ImageUrlL = "test2",
                    ReviewRemarks = "Testing Remarks 2",
                    AID= 5001
                }
            };
            return books;
        }
    }
}