using Clinic.Domain.Enums;

namespace OfficeService.BLL.Models.Requests;
public class CreateOfficeRequest
{
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
    public required string OfficeNumber { get; set; }
    public required string RegistryPhoneNumber { get; set; }
    public OfficeStatus Status { get; set; }
}
