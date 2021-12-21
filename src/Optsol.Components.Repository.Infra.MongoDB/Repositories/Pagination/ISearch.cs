using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using System;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories.Pagination
{
    public interface ISearch<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
    {
        Func<FilterDefinitionBuilder<TAggregateRoot>, FilterDefinition<TAggregateRoot>> Searcher();
    }
}
