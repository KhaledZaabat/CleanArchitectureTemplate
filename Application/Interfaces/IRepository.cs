using System;
using System.Collections.Generic;
using System.Text;
using CWM.CleanArchitecture.Domain.Common;

namespace CWM.CleanArchitecture.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query();         // AsNoTracking — for reads/projections
    IQueryable<T> QueryTracked();  // with tracking — for updates/deletes

    // Writes
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}
