using mFT_API.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Reflection.PortableExecutable;

const int keySize = 16;
const int iterations = 10000;
HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;

//USERS
app.MapGet("/User", () =>
{
    var users = new List<User>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var selectCmd = new SqlCommand(@"
                SELECT * FROM Users
            ", conn);

    using SqlDataReader reader = selectCmd.ExecuteReader();
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            var user = new User
            {
                UserID = reader.GetInt32(0),
                UserName = reader.GetString(1),
                Email = reader.GetString(2),
                PasswordSalt = reader.GetString(3),
                PasswordHash = reader.GetString(4),
                GroupID = reader.GetInt32(5),
                Active = reader.GetBoolean(6),
                SubscriptionDate = reader.GetDateTime(7)
            };
            users.Add(user);
        }
    }
            
return users;
})
.WithName("GetUsers")
.WithOpenApi();

app.MapPost("/User", (User user) =>
    {

        byte[] salt = RandomNumberGenerator.GetBytes(keySize);
        //string? password = "test";
        var saltString = Convert.ToBase64String(salt);
;
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: "test",
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: iterations,
            numBytesRequested: 256 / 8));

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var command = new SqlCommand(
            "INSERT INTO Users (userName, email, passwordSalt, passwordHash, groupID, active, subscriptionDate) VALUES (@userName, @email, @passwordSalt, @passwordHash, @groupID, @active, @subscriptionDate)",
            conn);

        command.Parameters.Clear();
        command.Parameters.AddWithValue("@userName", user.UserName);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@passwordSalt", saltString);
        command.Parameters.AddWithValue("@passwordHash", hashed);
        command.Parameters.AddWithValue("@groupID", user.GroupID);
        command.Parameters.AddWithValue("@active", user.Active);
        command.Parameters.AddWithValue("@subscriptionDate", user.SubscriptionDate);

        var newId = Convert.ToInt32(command.ExecuteScalar());

        return newId;
    })
    .WithName("CreateUser")
    .WithOpenApi();

//CATEGORIES


//TRANSACTIONS
    app.MapGet("/UserTransaction", () =>
    {
        var transactions = new List<UserTransaction>();

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var selectCmd = new SqlCommand(@"
                SELECT * FROM UserTransactions
            ", conn);

        using SqlDataReader reader = selectCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var transaction = new UserTransaction
                {
                    TransactionID = reader.GetInt32(0),
                    TransactionName = reader.GetString(1),
                    TransactionAmount = reader.GetDecimal(2),
                    TransactionType = reader.GetInt32(3),
                    TransactionCategory = reader.GetInt32(4),
                    RecurrenceFrequency = reader.GetInt32(5),
                    DueDate = reader.GetDateTime(6),
                    PaidDate = reader.GetDateTime(7),
                    NumberOfOccurrences = reader.GetInt32(8),
                    DayOfMonth = reader.GetInt32(9),
                    SemiMonthlySecondDay = reader.GetInt32(10),
                    Notes = reader.GetString(11),
                    UserID = reader.GetInt32(12),
                    CreditCardVendor = reader.GetString(13),
                    CreditCardLastFourDigits = reader.GetInt32(14),
                    CreditCardType = reader.GetInt32(15),
                    ParentRecurringTransactionId = reader.GetInt32(16)
                };
                transactions.Add(transaction);
            }
        }

        return transactions;
    })
    .WithName("GetUserTransactions")
    .WithOpenApi();


app.MapPost("/UserTransaction", (UserTransaction userTransaction) =>
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var command = new SqlCommand(
            "INSERT INTO UserTransactions " +
            "(transactionName, transactionAmount, transactionType, transactionCategory, recurrenceFrequency, dueDate, paidDate, numberOfOccurrences, dayOfMonth, semiMonthlySecondDay, notes, userID, creditCardVendor, creditCardLastFourDigits, creditCardType, parentRecurringTransactionId) " +
            "VALUES " +
            "(@transactionName, @transactionAmount, @transactionType, @transactionCategory, @recurrenceFrequency, @dueDate, @paidDate, @numberOfOccurrences, @dayOfMonth, @semiMonthlySecondDay, @notes, @userID, @creditCardVendor, @creditCardLastFourDigits, @creditCardType, @parentRecurringTransactionId)",
            conn);

        command.Parameters.Clear();
        command.Parameters.AddWithValue("@transactionName", userTransaction.TransactionName);
        command.Parameters.AddWithValue("@transactionAmount", userTransaction.TransactionAmount);
        command.Parameters.AddWithValue("@transactionType", userTransaction.TransactionType);
        command.Parameters.AddWithValue("@transactionCategory", userTransaction.TransactionCategory);
        command.Parameters.AddWithValue("@recurrenceFrequency", userTransaction.RecurrenceFrequency);
        command.Parameters.AddWithValue("@dueDate", userTransaction.DueDate);
        command.Parameters.AddWithValue("@paidDate", userTransaction.PaidDate);
        command.Parameters.AddWithValue("@numberOfOccurrences", userTransaction.NumberOfOccurrences);
        command.Parameters.AddWithValue("@dayOfMonth", userTransaction.DayOfMonth);
        command.Parameters.AddWithValue("@semiMonthlySecondDay", userTransaction.SemiMonthlySecondDay);
        command.Parameters.AddWithValue("@notes", userTransaction.Notes);
        command.Parameters.AddWithValue("@userID", userTransaction.UserID);
        command.Parameters.AddWithValue("@creditCardVendor", userTransaction.CreditCardVendor);
        command.Parameters.AddWithValue("@creditCardLastFourDigits", userTransaction.CreditCardLastFourDigits);
        command.Parameters.AddWithValue("@creditCardType", userTransaction.CreditCardType);
        command.Parameters.AddWithValue("@parentRecurringTransactionId", userTransaction.ParentRecurringTransactionId);

        var newId = Convert.ToInt32(command.ExecuteScalar());

        return newId;
    })
    .WithName("CreateUserTransaction")
    .WithOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
