using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination;
public interface IOrderBy<TAggregateRoot> : Optsol.Repository.Infra.EFCore.Base.Pagination.IOrderBy<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
    }
