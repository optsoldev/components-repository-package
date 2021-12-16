﻿using Optsol.Components.Repository.Domain.Entities;
using System;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public interface IOrderBy<TAggregateRoot>
         where TAggregateRoot : IAggregateRoot
    {
        Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>> OrderBy();
    }
}
