using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using System;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories.Pagination;

public interface IOrderBy<TAggregateRoot> 
    where TAggregateRoot : IAggregateRoot
{
    Func<SortDefinitionBuilder<TAggregateRoot>, SortDefinition<TAggregateRoot>> OrderBy();
}
