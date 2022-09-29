using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination;

    public interface ISearch<TAggregateRoot> : Optsol.Repository.Infra.EFCore.Base.Pagination.ISearch<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
    }
