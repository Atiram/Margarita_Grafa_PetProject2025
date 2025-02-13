using AutoMapper;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    private readonly IDoctorRepository _repository;
    private readonly IMapper _mapper;

    public DoctorIntegrationTests()
    {
        _mapper = Factory.Services.GetService(typeof(IMapper)) as IMapper;
        _repository = new DoctorRepository(Context);
    }

    [Fact]
    public async Task Create_Entity_ReturnsEntity()
    {
        Guid id = Guid.NewGuid();
        var entity = new DoctorEntity
        {
            Id = id,
            FirstName = "Test DoctorName",
        }; ;

        var actualResult = await _repository.CreateAsync(entity);

        Assert.Equivalent(actualResult, entity);
        //Context.Products.Last().ShouldBeEquivalentTo(entity);
    }


    //[Fact]
    //public async void Create_ValidViewModel_ReturnsViewModel()
    //{
    //    //Arrange
    //    Guid id = Guid.NewGuid();
    //    var viewModel = new DoctorViewModel
    //    {
    //        Id = id,
    //        FirstName = "Test DoctorName",
    //    };
    //    var entity = new DoctorEntity
    //    {
    //        Id = id,
    //        FirstName = "Test DoctorName",
    //    };

    //    using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
    //    request.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
    //    //request.Content.AddContent(TestShipmentViewModels.ValidChangeShipmentViewModel);

    //    //Act
    //    var actualResult = await Client.SendAsync(request);

    //    var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

    //    entity.Id = responseResult.Id;
    //    entity.FirstName = responseResult.FirstName;
    //    viewModel.Id = responseResult.Id;
    //    viewModel.FirstName = responseResult.FirstName;

    //    //Assert
    //    //actualResult.StatusCode.ShouldBe(HttpStatusCode.OK);
    //    //responseResult.FirstName.ShouldNotBe(default);
    //    Assert.Equivalent(responseResult, viewModel);
    //    //ShipmentCollection.Find(x => x.Id == responseResult.Id).Single().ShouldNotBeNull();
    //}
}
