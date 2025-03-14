using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/classregistrationreport", () =>
    {
        using SqlConnection conn = new SqlConnection(new SqlConnectionStringBuilder
        {
            DataSource = ".\\SQLExpress",
            InitialCatalog = "DevelopmentTest",
            UserID = "DevelopmentTest",
            Password = "d3vel0pm3ntT3st",
            TrustServerCertificate=true
        }.ConnectionString);

        conn.Open();

        var sql = "EXEC ClassRegistrationReport;";
        
        using var command = new SqlCommand(sql, conn);
        using var reader = command.ExecuteReader();

        var teachers = "";
        var classes = "";
        var paidFees = "";

        while (reader.Read())
        {
            teachers += $"{reader.GetInt32(0)} {reader.GetString(1)}\n";
        }

        reader.NextResult();

        while (reader.Read())
        {
            classes += $"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetInt32(2)}\n";
        }
        
        reader.NextResult();

        while (reader.Read())
        {
            paidFees += $"{reader.GetInt32(0)} {reader.GetInt32(1)} {reader.GetBoolean(2)}\n";
        }
        
        return (teachers + "\n" + classes + "\n" + paidFees);
    })
.WithName("ClassRegistrationReport");

app.Run();
