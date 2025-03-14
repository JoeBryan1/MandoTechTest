using System.Diagnostics;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Excersice2.Models;
using Microsoft.Data.SqlClient;

namespace Excersice2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
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
            teachers += $"<tr><td>{reader.GetInt32(0)}</td> <td>{reader.GetString(1)}</td></tr>\n";
        }

        reader.NextResult();

        while (reader.Read())
        {
            classes += $"<tr><td>{reader.GetInt32(0)}</td> <td>{reader.GetString(1)}</td> <td>{reader.GetInt32(2)}</td></tr>\n";
        }
        
        reader.NextResult();

        while (reader.Read())
        {
            paidFees += $"<tr><td>{reader.GetInt32(0)}</td> <td>{reader.GetInt32(1)}</td> <td>{reader.GetBoolean(2)}</td></tr>\n";
        }
        
        return new ContentResult
        {
            ContentType="text/html", 
            Content=
                "<table><tr><th>Teacher ID</th><th>Teacher Name</th></tr>" + teachers + "</table>"
            + "<table><tr><th><Class ID></th><th>Class Name</th><th>Teacher Id</th><tr>" + classes + "</tr></table>" 
            + "<table><tr><th><Student ID></th><th>Class ID</th><th>Has Paid Fees</th><tr>" + paidFees + "</tr></table>"
        };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
