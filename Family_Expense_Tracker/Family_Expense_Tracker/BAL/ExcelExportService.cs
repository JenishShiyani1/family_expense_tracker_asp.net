using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;

public interface IExcelExportService
{
    Task<FileResult> ExportToExcelAsync(string tableName, string UserOrGroup, int? ID, string excelFileName);
}

public class ExcelExportService : IExcelExportService
{
    private readonly HttpClient _httpClient;
    Uri baseAddress = new Uri("https://localhost:7082/api");

    public ExcelExportService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = baseAddress;
    }

    public async Task<FileResult> ExportToExcelAsync(string tableName, string UserOrGroup ,int? ID, string excelFileName)
    {
        // Call the API to retrieve data
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{tableName}/{UserOrGroup}/{ID}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch data from API. Status code: {response.StatusCode}");
        }

        // Deserialize the API response into a DataTable
        var jsonData = await response.Content.ReadAsStringAsync();
        var dataTable = JsonConvert.DeserializeObject<DataTable>(jsonData);

        // Create an Excel workbook and populate it with data
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(excelFileName);
        worksheet.Cell(1, 1).InsertTable(dataTable);

        // Save the workbook to a memory stream
        using var stream = new System.IO.MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        // Return the Excel file as a FileResult
        return new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = excelFileName + ".xlsx"
        };
    }
}
