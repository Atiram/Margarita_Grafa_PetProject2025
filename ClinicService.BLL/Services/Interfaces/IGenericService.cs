namespace ClinicService.BLL.Services.Interfaces;
public interface IGenericService<TModel> where TModel : class
{
    Task<TModel> GetById(Guid id);
    Task<TModel> CreateAsync(TModel model);
    Task<TModel> UpdateAsync(TModel model);
    Task<bool> DeleteAsync(Guid id);
}
