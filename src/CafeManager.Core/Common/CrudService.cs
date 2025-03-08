
using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Exceptions;

using MapsterMapper;

namespace CafeManager.Core.Common;

public abstract class CrudService<TId, TEntity, TDto, TAddDto, TUpdateDto> : ICrudService<TId, TDto, TAddDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TId : struct
    where TAddDto : notnull
    where TUpdateDto : notnull
    where TDto : notnull
{
    protected readonly IBaseRepository<TEntity, TId> _repository;
    protected readonly IMapper _mapper;

    protected CrudService(IBaseRepository<TEntity, TId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TDto> CreateAsync(TAddDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.CreateAsync(entity, cancellationToken);
        return _mapper.Map<TDto>(result);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public virtual async Task<TDto> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(id, cancellationToken);

        return entity is null
            ? throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} with id {id} not found.")
            : _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> UpdateAsync(TId id, TUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(id, cancellationToken) ??
            throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} with id {id} not found.");

        _mapper.Map(dto, entity);
        var result = await _repository.UpdateAsync(entity, cancellationToken);

        return _mapper.Map<TDto>(result);
    }
}