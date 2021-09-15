using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Infra.Mock.Core
{
    public class Context
    {
        public CustomerCollection Customers { get; set; }

        public int SaveChanges()
        {
            return Customers.Total;
        }
    }
}
