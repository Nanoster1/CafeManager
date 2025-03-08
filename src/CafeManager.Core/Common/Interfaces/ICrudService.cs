namespace CafeManager.Core.Common.Interfaces;

public interface ICrudService<TId, TDto, TAddDto, TUpdateDto>
{
    Task<TDto> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<TDto> CreateAsync(TAddDto dto, CancellationToken cancellationToken = default);
    Task<TDto> UpdateAsync(TId id, TUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}