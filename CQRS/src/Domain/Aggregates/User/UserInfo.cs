namespace Domain.Aggregates.User;

public sealed class UserInfo
{
    private UserInfo()
    {
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string EmailAddress { get; private set; }

    public string PhoneNumber { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public string CurrentCity { get; private set; }

    public static UserInfo CreateUserInfo(string firstName, string lastName, string emailAddress, string phoneNumber,
        DateTime dateOfBirth, string currentCity)
    {
        UserInfo userInfo = new()
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            PhoneNumber = phoneNumber,
            DateOfBirth = dateOfBirth,
            CurrentCity = currentCity
        };

        return userInfo;
    }
}