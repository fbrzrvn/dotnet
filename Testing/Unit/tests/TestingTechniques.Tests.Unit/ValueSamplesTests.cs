using System;
using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit;

public class ValueSamplesTests
{
    private readonly ValueSamples _sut = new();

    [Fact]
    public void StringAssertionExample()
    {
        var fullName = _sut.FullName;

        fullName.Should().Be("Lerelle Jones");
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Lerelle");
        fullName.Should().EndWith("Jones");
    }

    [Fact]
    public void NumberAssertionExample()
    {
        var age = _sut.Age;

        age.Should().Be(21);
        age.Should().BePositive();
        age.Should().BeGreaterThan(20);
        age.Should().BeLessOrEqualTo(21);
        age.Should().BeInRange(18, 60);
    }

    [Fact]
    public void DateAssertionExample()
    {
        var dateOfBirth = _sut.DateOfBirth;

        dateOfBirth.Should().Be(new(2002, 6, 9));
        dateOfBirth.Should().BeInRange(new(2002, 1, 1), new(2003, 1, 1));
        dateOfBirth.Should().BeGreaterThan(new(2002, 1, 1));
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        var expected = new User
        {
            FullName = "Lerelle Jones",
            Age = 21,
            DateOfBirth = new(2002, 6, 9)
        };

        var user = _sut.AppUser;

        //Assert.Equal(expected, user);
        //user.Should().Be(expected);
        user.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void EnumerableObjectsAssertionExample()
    {
        var expected = new User
        {
            FullName = "Lerelle Jones",
            Age = 21,
            DateOfBirth = new (2002, 6, 9)
        };

        var users = _sut.Users.As<User[]>();

        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Lerelle") && x.Age > 5);
    }

    [Fact]
    public void EnumerableNumbersAssertionExample()
    {
        var numbers = _sut.Numbers.As<int[]>();

        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        var calculator = new Calculator();

        Action result = () => calculator.Divide(1, 0);

        result.Should()
            .Throw<DivideByZeroException>()
            .WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        var number = _sut.InternalSecretNumber;

        number.Should().Be(42);
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        var monitorSubject = _sut.Monitor();

        _sut.RaiseExampleEvent();

        monitorSubject.Should().Raise("ExampleEvent");
    }
}
