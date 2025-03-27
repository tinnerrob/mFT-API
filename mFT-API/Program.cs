using System;
using System.Transactions;
using mFT_API.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// For production scenarios, consider keeping Swagger configurations behind the environment check
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
        SELECT * FROM Transactions
    ", conn);

    using SqlDataReader reader = selectCmd.ExecuteReader();
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}");

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
        "INSERT INTO Transactions (transactionName, amount, transactionType) VALUES (@transactionName, @amount, @transactionType)",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@transactionName", userTransaction.TransactionName);
    command.Parameters.AddWithValue("@amount", userTransaction.Amount);
    command.Parameters.AddWithValue("@type", userTransaction.TransactionType);
    //command.Parameters.AddWithValue("@category", userTransaction.Category);
    //command.Parameters.AddWithValue("@recurrenceFrequency", transaction.RecurrenceFrequency);
    //command.Parameters.AddWithValue("@dueDate", transaction.DueDate);
    //command.Parameters.AddWithValue("@paidDate", transaction.PaidDate);
    //command.Parameters.AddWithValue("@numberOfOccurrences", transaction.NumberOfOccurrences);
    //command.Parameters.AddWithValue("@dayOfMonth", transaction.DayOfMonth);
    //command.Parameters.AddWithValue("@semiMonthlySecondDay", transaction.SemiMonthlySecondDay);
    //command.Parameters.AddWithValue("@notes", transaction.Notes);
    //command.Parameters.AddWithValue("@userID", transaction.UserID);

    var newId = Convert.ToInt32(command.ExecuteScalar());

    return newId;
})
.WithName("CreateUserTransaction")
.WithOpenApi();

app.Run();
