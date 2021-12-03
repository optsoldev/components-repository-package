﻿namespace Optsol.Components.Repository.Infra.Mock.Core
{
    public class Context
    {
        public CustomerCollection Customers { get; set; }

        public Context()
        {
            Customers = new CustomerCollection();
            Customers.Init();
        }

        public int SaveChanges()
        {            
            return Customers.Total;
        }
    }
}
