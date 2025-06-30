using Clinic.Domain;
using Microsoft.AspNetCore.Mvc;
using OfficeService.BLL.Models.Requests;
using OfficeService.BLL.Services.Interfaces;

namespace OfficeService.API.Controllers;
[Route("api/offices")]
[ApiController]
public class OfficeController(IOfficeService officeService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var officeModel = await officeService.GetByIdAsync(id, cancellationToken);
        return Ok(officeModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var offices = await officeService.GetAllAsync(cancellationToken);
        return Ok(offices);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfficeRequest request, CancellationToken cancellationToken)
    {
        var createdOffice = await officeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdOffice.Id }, createdOffice);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOfficeRequest request, CancellationToken cancellationToken)
    {
        var updatedOffice = await officeService.UpdateAsync(request, cancellationToken);
        return Ok(updatedOffice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
    {
        var isDeleted = await officeService.DeleteAsync(id, cancellationToken);
        return isDeleted ? Ok() : NotFound(string.Format(NotificationMessages.NotFoundErrorMessage, id));
    }
}
