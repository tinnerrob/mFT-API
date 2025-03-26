using System;
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

//try
//{
//    using var conn = new SqlConnection(connectionString);
//    conn.Open();

//    var createCmd = new SqlCommand(@"
//        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persons')
//        BEGIN
//            CREATE TABLE Persons (
//                ID int NOT NULL PRIMARY KEY IDENTITY,
//                FirstName varchar(255),
//                LastName varchar(255)
//            );
//        END
//    ", conn);
//    createCmd.ExecuteNonQuery();
//}
//catch (Exception e)
//{
//    // Table may already exist
//    Console.WriteLine(e.Message);
//}

app.MapGet("/Person", () => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var selectCmd = new SqlCommand(@"
        SELECT * FROM Persons
    ", conn);

    using SqlDataReader reader = selectCmd.ExecuteReader();
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}");
        }
    }

    return rows;
})
.WithName("GetPersons")
.WithOpenApi();

app.MapGet("/User", () => {
    var rows = new List<string>();

    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var selectCmd = new SqlCommand(@"
        SELECT * FROM users
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
        "INSERT INTO users (user_name, password, group) VALUES (@username, @password, @group)",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@username", user.Name);
    command.Parameters.AddWithValue("@password", user.Password);
    command.Parameters.AddWithValue("@group", user.Group);

    var newId = Convert.ToInt32(command.ExecuteScalar());

    return newId;
})
.WithName("CreateUser")
.WithOpenApi();

//app.MapPost("/Person", (Person person) => {
//    using var conn = new SqlConnection(connectionString);
//    conn.Open();

//    var command = new SqlCommand(
//        "INSERT INTO Persons (firstName, lastName) VALUES (@firstName, @lastName)",
//        conn);

//    command.Parameters.Clear();
//    command.Parameters.AddWithValue("@firstName", person.FirstName);
//    command.Parameters.AddWithValue("@lastName", person.LastName);

//    var newId = Convert.ToInt32(command.ExecuteScalar());

//    return newId;
//})
//.WithName("CreatePerson")
//.WithOpenApi();

//app.MapGet("/Category", () => {
//    var rows = new List<string>();

//    using var conn = new SqlConnection(connectionString);
//    conn.Open();

//    var selectCmd = new SqlCommand(@"
//        SELECT * FROM categories
//    ", conn);

//    using SqlDataReader reader = selectCmd.ExecuteReader();
//    if (reader.HasRows)
//    {
//        while (reader.Read())
//        {
//            rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}");

//        }
//    }

//    return rows;
//})
//.WithName("GetCategories")
//.WithOpenApi();

//app.MapPost("/Category", (Category category) => {
//    using var conn = new SqlConnection(connectionString);
//    conn.Open();

//    var command = new SqlCommand(
//        "INSERT INTO categories (name, color, icon, type, user_id) VALUES (@Name, @Color, @Icon, @Type, @User_ID)",
//        conn);

//    command.Parameters.Clear();
//    command.Parameters.AddWithValue("@name", category.Name);
//    command.Parameters.AddWithValue("@color", category.Color);
//    command.Parameters.AddWithValue("@icon", category.Icon);
//    command.Parameters.AddWithValue("@type", category.Type);
//    command.Parameters.AddWithValue("@user_id", category.User_ID);

//    var newId = Convert.ToInt32(command.ExecuteScalar());

//    return newId;
//})
//.WithName("CreateCategory")
//.WithOpenApi();

app.Run();

public class Person
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

public class User
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public int Group { get; set; }

}