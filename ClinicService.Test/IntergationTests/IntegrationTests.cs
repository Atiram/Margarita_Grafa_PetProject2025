using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Data;
using ClinicService.DAL.Utilities.Pagination;
using ClinicService.Test.TestEntities;
using ClinicServiceApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;


namespace ClinicService.Test.IntergationTests;
public class IntegrationTests
{
    protected TestServer Server { get; }
    protected HttpClient Client { get; }
    protected ClinicDbContext Context { get; }
    protected WebApplicationFactory<Program> Factory { get; }
    private const string JsonContentType = "application/json";
    private const string UrlPost = "https://localhost:7105/Doctor";

    public IntegrationTests()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ClinicDbContext>>();

                services.AddDbContext<ClinicDbContext>(options => options.UseInMemoryDatabase("TestDb"));

                var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
                mockHttpMessageHandler.Protected()
                         .Setup<Task<HttpResponseMessage>>(
                             "SendAsync",
                             ItExpr.IsAny<HttpRequestMessage>(),
                             ItExpr.IsAny<CancellationToken>()
                         )
                         .ReturnsAsync(new HttpResponseMessage
                         {
                             StatusCode = HttpStatusCode.OK,
                             Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { storageLocation = "test_upload_url" })),
                         });
                var httpClient = new HttpClient(mockHttpMessageHandler.Object);
                services.AddHttpClient("FileService")
                    .ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);
            }));


        Server = Factory.Server;
        Client = Server.CreateClient();
        Context = Factory.Services.CreateScope().ServiceProvider.GetService<ClinicDbContext>()!;
    }

    public static HttpRequestMessage AddContent<T>(T entity, HttpRequestMessage requestMessage)
    {
        if (entity is UpdateDoctorRequest updateDoctorRequest)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(updateDoctorRequest.Id.ToString()), "Id");
            content.Add(new StringContent(updateDoctorRequest.FirstName), "FirstName");
            content.Add(new StringContent(updateDoctorRequest.LastName), "LastName");
            content.Add(new StringContent(updateDoctorRequest.MiddleName ?? ""), "MiddleName");
            content.Add(new StringContent(updateDoctorRequest.DateOfBirth.ToString("yyyy-MM-dd")), "DateOfBirth");
            content.Add(new StringContent(updateDoctorRequest.Email), "Email");
            content.Add(new StringContent(updateDoctorRequest.Specialization), "Specialization");
            content.Add(new StringContent(updateDoctorRequest.Office), "Office");
            content.Add(new StringContent(updateDoctorRequest.CareerStartYear.ToString()), "CareerStartYear");
            content.Add(new StringContent(updateDoctorRequest.Status.ToString()), "Status");

            if (updateDoctorRequest.Formfile != null)
            {
                var fileContent = new StreamContent(updateDoctorRequest.Formfile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(updateDoctorRequest.Formfile.ContentType);
                content.Add(fileContent, "Formfile", updateDoctorRequest.Formfile.FileName);
            }

            requestMessage.Content = content;
        }
        else if (entity is CreateDoctorRequest createDoctorRequest && createDoctorRequest.Formfile != null)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(createDoctorRequest.FirstName), "FirstName");
            content.Add(new StringContent(createDoctorRequest.LastName), "LastName");
            content.Add(new StringContent(createDoctorRequest.MiddleName ?? ""), "MiddleName");
            content.Add(new StringContent(createDoctorRequest.DateOfBirth.ToString("yyyy-MM-dd")), "DateOfBirth");
            content.Add(new StringContent(createDoctorRequest.Email), "Email");
            content.Add(new StringContent(createDoctorRequest.Specialization), "Specialization");
            content.Add(new StringContent(createDoctorRequest.Office), "Office");
            content.Add(new StringContent(createDoctorRequest.CareerStartYear.ToString()), "CareerStartYear");
            content.Add(new StringContent(createDoctorRequest.Status.ToString()), "Status");

            var fileContent = new StreamContent(createDoctorRequest.Formfile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(createDoctorRequest.Formfile.ContentType);
            content.Add(fileContent, "Formfile", createDoctorRequest.Formfile.FileName);

            requestMessage.Content = content;
        }
        else
        {
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, JsonContentType);
        }
        return requestMessage;
    }

    public async Task<HttpResponseMessage> SendPostRequest<T>(T entity)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, UrlPost);
        var actualRequest = AddContent(entity, request);
        return await Client.SendAsync(request);
    }

    public static T? GetResponseResult<T>(HttpResponseMessage responseMessage)
    {
        var content = responseMessage.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<T>(content) ?? default;
    }

    public static List<CreateDoctorRequest> CreateDoctorList(string searchPrefix)
    {
        var createDoctorRequests = new List<CreateDoctorRequest>();
        for (int i = 0; i < 5; i++)
        {
            var request = TestDoctorRequest.NewCreateDoctorRequest;
            request.FirstName = searchPrefix + request.FirstName;
            createDoctorRequests.Add(request);
        }
        return createDoctorRequests;
    }

    public static GetAllDoctorsParams CreateGetAllDoctorsParams(string searchPrefix)
    {
        return new GetAllDoctorsParams()
        {
            IsDescending = false,
            PageNumber = 1,
            PageSize = 10,
            SortParameter = null,
            SearchValue = searchPrefix,
        };
    }

    public static PagedResult<T> CreatePagedResult<T>(GetAllDoctorsParams getAllDoctorsParams, List<T> doctorViewModels)
    {
        return new PagedResult<T>()
        {
            PageSize = getAllDoctorsParams.PageSize,
            TotalCount = doctorViewModels.Count,
            TotalPages = (int)Math.Ceiling((double)doctorViewModels.Count / getAllDoctorsParams.PageSize),
            Results = doctorViewModels
               .Skip((getAllDoctorsParams.PageNumber - 1) * getAllDoctorsParams.PageSize)
               .Take(getAllDoctorsParams.PageSize)
               .ToList()
        };
    }

    public static string CreateActualUrl(GetAllDoctorsParams getAllDoctorsParams)
    {
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        foreach (var property in typeof(GetAllDoctorsParams).GetProperties())
        {
            queryString[property.Name] = property.GetValue(getAllDoctorsParams)?.ToString();
        }
        return UrlPost + "?" + queryString.ToString();
    }
}
