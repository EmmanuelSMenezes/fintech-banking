using Xunit;
using Moq;
using FluentAssertions;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Tests;

public class UserRepositoryTests
{
    [Fact]
    public void User_Creation_ShouldHaveValidProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@owaypay.com";
        var fullName = "Test User";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password@123");

        // Act
        var user = new User
        {
            Id = userId,
            Email = email,
            FullName = fullName,
            PasswordHash = passwordHash,
            IsActive = true,
            Role = "user",
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        user.Id.Should().Be(userId);
        user.Email.Should().Be(email);
        user.FullName.Should().Be(fullName);
        user.IsActive.Should().BeTrue();
        user.Role.Should().Be("user");
    }

    [Fact]
    public void User_PasswordHash_ShouldBeVerifiable()
    {
        // Arrange
        var password = "Password@123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Act
        var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void User_PasswordHash_ShouldNotMatchWrongPassword()
    {
        // Arrange
        var password = "Password@123";
        var wrongPassword = "WrongPassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Act
        var isValid = BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword);

        // Assert
        isValid.Should().BeFalse();
    }
}

public class AccountTests
{
    [Fact]
    public void Account_Creation_ShouldHaveValidProperties()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var accountNumber = "OWP12345678";
        var bankCode = "001";
        var balance = 1000m;

        // Act
        var account = new Account
        {
            Id = accountId,
            UserId = userId,
            AccountNumber = accountNumber,
            BankCode = bankCode,
            Balance = balance,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        account.Id.Should().Be(accountId);
        account.UserId.Should().Be(userId);
        account.AccountNumber.Should().Be(accountNumber);
        account.BankCode.Should().Be(bankCode);
        account.Balance.Should().Be(balance);
        account.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Account_Balance_ShouldBeDecimal()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            AccountNumber = "OWP12345678",
            BankCode = "001",
            Balance = 1000.50m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        account.Balance.Should().Be(1000.50m);
        account.Balance.Should().BeOfType(typeof(decimal));
    }
}

public class TransactionTests
{
    [Fact]
    public void Transaction_Creation_ShouldHaveValidProperties()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var amount = 100m;
        var transactionType = "PIX";
        var status = "PENDING";

        // Act
        var transaction = new Transaction
        {
            Id = transactionId,
            AccountId = accountId,
            UserId = userId,
            TransactionType = transactionType,
            Amount = amount,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Id.Should().Be(transactionId);
        transaction.AccountId.Should().Be(accountId);
        transaction.UserId.Should().Be(userId);
        transaction.TransactionType.Should().Be(transactionType);
        transaction.Amount.Should().Be(amount);
        transaction.Status.Should().Be(status);
    }

    [Fact]
    public void Transaction_Status_ShouldBeValid()
    {
        // Arrange
        var validStatuses = new[] { "PENDING", "COMPLETED", "FAILED", "CANCELLED" };

        // Act & Assert
        foreach (var status in validStatuses)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TransactionType = "PIX",
                Amount = 100,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            transaction.Status.Should().Be(status);
        }
    }

    [Fact]
    public void Transaction_Type_ShouldBeValid()
    {
        // Arrange
        var validTypes = new[] { "PIX_QR_CODE", "WITHDRAWAL", "DEPOSIT" };

        // Act & Assert
        foreach (var type in validTypes)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TransactionType = type,
                Amount = 100,
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow
            };

            transaction.TransactionType.Should().Be(type);
        }
    }
}

public class RepositoryMockTests
{
    [Fact]
    public async Task UserRepository_Mock_GetByEmailAsync_ReturnsUser()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var email = "test@owaypay.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = "Test User",
            PasswordHash = "hash",
            IsActive = true,
            Role = "user",
            CreatedAt = DateTime.UtcNow
        };

        mockRepository.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);

        // Act
        var result = await mockRepository.Object.GetByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
        mockRepository.Verify(x => x.GetByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task AccountRepository_Mock_GetByUserIdAsync_ReturnsAccount()
    {
        // Arrange
        var mockRepository = new Mock<IAccountRepository>();
        var userId = Guid.NewGuid();
        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountNumber = "OWP12345678",
            BankCode = "001",
            Balance = 1000,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        mockRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(account);

        // Act
        var result = await mockRepository.Object.GetByUserIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
        mockRepository.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task TransactionRepository_Mock_CreateAsync_ReturnsTransaction()
    {
        // Arrange
        var mockRepository = new Mock<ITransactionRepository>();
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TransactionType = "PIX",
            Amount = 100,
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };

        mockRepository.Setup(x => x.CreateAsync(It.IsAny<Transaction>()))
            .ReturnsAsync(transaction);

        // Act
        var result = await mockRepository.Object.CreateAsync(transaction);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(transaction.Id);
        mockRepository.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
    }
}

