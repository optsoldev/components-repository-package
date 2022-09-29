using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination
{
    public interface IInclude<TAggregateRoot> : Optsol.Repository.Infra.EFCore.Base.Pagination.IInclude<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
    }
}
