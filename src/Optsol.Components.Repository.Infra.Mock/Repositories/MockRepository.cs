using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Core;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.Mock.Repositories
{
    public class MockRepository : IRepository<Customer>
    {
        protected Context Context { get; set; }

        public MockRepository()
        {
            Context = new Context();
        }

        public void Delete(Customer entity)
        {
            Context.Customers.Delete(entity);
        }

        public IEnumerable<Customer> GetAll()
        {
            return Context.Customers.GetCustomers();
        }

        public IEnumerable<Customer> GetAllByIds(params Guid[] ids)
        {
            var customers = Context.Customers.FindByKeys(ids);
            return customers;
        }

        public Customer GetById(Guid id)
        {
            var customer = Context.Customers.FindById(id);
            return customer;
        }

        public IEnumerable<Customer> GetWithExpression(Expression<Func<Customer, bool>> filterExpression)
        {
            return Context.Customers.FindWithExpression(filterExpression);
        }

        public void Insert(Customer entity)
        {
            Context.Customers.Insert(entity);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Update(Customer entity)
        {
            var customerRemoved = Context.Customers.FindById(entity.Id);
            Context.Customers.Delete(customerRemoved);
            Context.Customers.Insert(entity);
        }
    }
}
