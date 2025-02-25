using System.Text;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Data;
using ClinicService.DAL.Utilities.Pagination;
using ClinicService.Test.TestEntities;
using ClinicServiceApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            }));
        Server = Factory.Server;
        Client = Server.CreateClient();
        Context = Factory.Services.CreateScope().ServiceProvider.GetService<ClinicDbContext>()!;
    }

    public static HttpRequestMessage AddContent<T>(T entity, HttpRequestMessage requestMessage)
    {
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, JsonContentType);
        return requestMessage;
    }
    public async Task<HttpResponseMessage> SendPostRequest(DoctorViewModel viewModel)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, UrlPost);
        var actualRequest = AddContent(viewModel, request);
        return await Client.SendAsync(request);
    }

    public static T? GetResponseResult<T>(HttpResponseMessage responseMessage)
    {
        var content = responseMessage.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<T>(content) ?? default;
    }

    public List<DoctorViewModel> CreateDoctorList(string searchPrefix)
    {
        var doctorViewModels = new List<DoctorViewModel>();
        for (int i = 0; i < 5; i++)
        {
            var viewModel = TestDoctorViewModel.NewDoctorViewModel;
            viewModel.Id = Guid.NewGuid();
            viewModel.FirstName = searchPrefix + viewModel.FirstName;
            doctorViewModels.Add(viewModel);
        }
        return doctorViewModels;

    }
    public GetAllDoctorsParams CreateGetAllDoctorsParams(string searchPrefix)
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

    public PagedResult<DoctorViewModel> CreatePagedResult(GetAllDoctorsParams getAllDoctorsParams, List<DoctorViewModel> doctorViewModels)
    {
        return new PagedResult<DoctorViewModel>()
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
}
