using FluentAssertions;
using FluentValidation;
using Movies.Application.Services;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Validators;
using Movies.Api.Mappers;
using Movies.Contracts.Requests;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Movies.Api.Tests.Unit;

public class MovieServiceTests
{
    private readonly MovieService _sut;
    private readonly IMovieRepository _movieRepository = Substitute.For<IMovieRepository>();
    private readonly IRatingRepository _ratingRepository = Substitute.For<IRatingRepository>();
    private readonly IValidator<Movie> _movieValidator = Substitute.For<IValidator<Movie>>();

    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsValidator =
        Substitute.For<IValidator<GetAllMoviesOptions>>();

    private Movie MovieFixture = new Movie
    {
        Id = Guid.NewGuid(),
        Title = "Nick the greek",
        YearOfRelease = 2022,
        Genres = new List<string> { "Comedy" }
    };
    
    public MovieServiceTests()
    {
        this._sut = new MovieService(_movieRepository, _movieValidator, _ratingRepository, _getAllMoviesOptionsValidator);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenCreated()
    {
        // Arrange
        _movieRepository.CreateAsync(MovieFixture).Returns(true);
        
        // Act
        var result = await _sut.CreateAsync(MovieFixture);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAnEmptyList_WhenNoMoviesExists()
    {
        // Arrange
        const GetAllMoviesOptions options = null;
        
        _movieRepository.GetAllAsync(options).Returns(Enumerable.Empty<Movie>());
        
        // Act
        var result = await _sut.GetAllAsync(options);
        
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAListOfMovies_WhenSomeMoviesExist()
    {
        // Arrange
        const GetAllMoviesOptions options = null;

        var expectedMovies = new[] { MovieFixture };

        _movieRepository.GetAllAsync(options).Returns(expectedMovies);

        // Act
        var result = await _sut.GetAllAsync(options);

        // Assert
        result.Single().Should().BeEquivalentTo(MovieFixture);
        result.Should().BeEquivalentTo(expectedMovies);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAMovie_WhenMovieExists()
    {
        // Arrange
        _movieRepository.GetByIdAsync(MovieFixture.Id).Returns(MovieFixture);

        // Act
        var result = await _sut.GetByIdAsync(MovieFixture.Id);

        // Assert
        result.Should().BeEquivalentTo(MovieFixture);
    }
    
    [Fact]
    public async Task GetBySlugAsync_ShouldReturnAMovie_WhenMovieExists()
    {
        // Arrange
        _movieRepository.GetBySlugAsync(MovieFixture.Slug).Returns(MovieFixture);

        // Act
        var result = await _sut.GetBySlugAsync(MovieFixture.Slug);

        // Assert
        result.Should().BeEquivalentTo(MovieFixture);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenMovieNoExists()
    {
        // Arrange
        _movieRepository.UpdateAsync(MovieFixture).Returns(true);
    
        // Act
        var result = await _sut.UpdateAsync(MovieFixture);
    
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenMovieExists()
    {
        // Arrange
        _movieRepository.DeleteByIdAsync(MovieFixture.Id).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(MovieFixture.Id);

        // Assert
        result.Should().BeTrue();
    }
}