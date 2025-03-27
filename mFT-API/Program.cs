using mFT_API.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

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
    app.MapGet("/User", () => {
        var rows = new List<string>();

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
                rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}, {reader.GetInt32(3)}");

            }
        }

        return rows;
    })
    .WithName("GetUsers")
    .WithOpenApi();

    app.MapPost("/User", (User user) =>
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var command = new SqlCommand(
            "INSERT INTO Users (userName, password, groupID) VALUES (@userName, @password, @groupID)",
            conn);

        command.Parameters.Clear();
        command.Parameters.AddWithValue("@userName", user.UserName);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@groupID", user.GroupID);

        var newId = Convert.ToInt32(command.ExecuteScalar());

        return newId;
    })
    .WithName("CreateUser")
    .WithOpenApi();

//CATEGORIES


//TRANSACTIONS
    app.MapGet("/UserTransaction", () => {
        var rows = new List<string>();

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
                rows.Add($"{reader.GetInt32(0)}, " +
                    $"{reader.GetString(1)}, " + //Name
                    $"{reader.GetDecimal(2)}, " + //Amount
                    $"{reader.GetInt32(3)}, " + //Type
                    $"{reader.GetInt32(4)}, " + //Category
                    $"{reader.GetInt32(5)}, " + //Frequency
                    $"{reader.GetDateTime(6).ToShortDateString}, " + //DueDate
                    $"{reader.GetDateTime(7).ToShortDateString}, " + //PaidDate
                    $"{reader.GetInt32(8)}, " + //Occurences
                    $"{reader.GetInt32(9)}, " + //DayOfMonth
                    $"{reader.GetInt32(10)}, " + //SecondDay
                    $"{reader.GetString(11)}, " + //Notes
                    $"{reader.GetInt32(12)}" //UserID
                    );
            }
        }

        return rows;
    })
    .WithName("GetUserTransactions")
    .WithOpenApi();

    app.MapPost("/UserTransaction", (UserTransaction userTransaction) =>
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var command = new SqlCommand(
            "INSERT INTO UserTransactions " +
            "(transactionName, transactionAmount, transactionType, transactionCategory, recurrenceFrequency, dueDate, paidDate, numberOfOccurrences, dayOfMonth, semiMonthlySecondDay, notes, userID) " +
            "VALUES " +
            "(@transactionName, @transactionAmount, @transactionType, @transactionCategory, @recurrenceFrequency, @dueDate, @paidDate, @numberOfOccurrences, @dayOfMonth, @semiMonthlySecondDay, @notes, @userID)",
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

        var newId = Convert.ToInt32(command.ExecuteScalar());

        return newId;
    })
    .WithName("CreateUserTransaction")
    .WithOpenApi();

app.Run();
