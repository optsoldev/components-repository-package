using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.Mock.Core
{
    public sealed class CustomerCollection
    {
        private readonly IList<Customer> customers;

        public CustomerCollection()
        {
            customers = new List<Customer>();
        }

        public int Total => customers.Count;

        public IReadOnlyCollection<Customer> GetCustomers()
        {
            IReadOnlyCollection<Customer> customers = new ReadOnlyCollection<Customer>(this.customers);
            return customers;
        }

        public void Insert(Customer customer)
        {
            customers.Add(customer);
        }

        public void Delete(Customer customer)
        {
            customers.Remove(customer);
        }

        public IEnumerable<Customer> Init()
        {
            Insert(Customer.Create(
                Person.Create("Weslley Bruno", "Carneiro"),
                Email.Create("weslley.carneiro@optsol.com.br"),
                DateValue.Create().SetDateValueWithDate(DateTime.Parse("1985-11-21"))
            ));

            return customers;
        }

        public Customer FindByKey(Key id)
        {
            return FindByKeys(id).FirstOrDefault();
        }

        public IEnumerable<Customer> FindWithExpression(Expression<Func<Customer, bool>> filterExpression)
        {
            return customers.Where(filterExpression.Compile());
        }

        public IEnumerable<Customer> FindByKeys(params Key[] ids)
        {
            return customers.Where(customer => ids.Contains(customer.Id));
        }
    }
}
