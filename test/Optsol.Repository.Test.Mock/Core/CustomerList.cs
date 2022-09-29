using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination;
using Optsol.Domain.ValueObjects;
using Optsol.Repository.Base.Pagination;
using Optsol.Repository.Test.Mock.Entities.Core;

namespace Optsol.Repository.Test.Mock.Core
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
            IReadOnlyCollection<Customer> getCustomers = new ReadOnlyCollection<Customer>(this.customers);
            return getCustomers;
        }

        public void Insert(Customer customer)
        {
            customers.Add(customer);
        }

        public void InsertRange(IList<Customer> customerCollection)
        {
            foreach (var customer in customerCollection)
            {
                Insert(customer);
            }
        }

        public void Delete(Customer customer)
        {
            customers.Remove(customer);
        }

        public void DeleteRange(IList<Customer> customersCollection)
        {
            foreach (var customer in customersCollection)
            {
                customers.Remove(customer);
            }
        }

        public IEnumerable<Customer> Init()
        {
            InsertCustomers();
            return customers;
        }

        private void InsertCustomers()
        {
            var nomes = new Dictionary<string, string>
            {
                { "Weslley", "Carneiro" },
                { "Romulo", "Louzada" },
                { "Felipe", "Carvalho" },
                { "Luiz", "Marcon" },
                { "Guilherme", "Conrado" },
                { "Vera", "Carneiro" },
                { "Bruna", "Victória" },
                { "Elaine", "Rocha" },
                { "Paulo", "Gilles" },
                { "Vitor", "Marcelo" },
                { "Antônio", "Freitas" },
                { "Marcos", "Andre" },
                { "Paula", "Fernandes" },
                { "Joao", "Gomes" },
                { "Pedro", "Carvalho" },
                { "Angelo", "Ticiano" },
                { "Sara", "Cristina" },
                { "Luciano", "Rocha" }
            };

            foreach (var nome in nomes)
            {
                Insert(Customer.Create(
                    Person.Create($"{nome.Key}", $"{nome.Value}"),
                    Email.Create($"{nome.Key.ToLower()}.{nome.Value.ToLower()}@optsol.com.br"),
                    DateValue.Create().SetDateValueWithDate(DateTime.Parse($"1985-{(new Random()).Next(1, 12)}-{(new Random()).Next(1, 28)}"))
                    )
                );
            }
        }

        public Customer FindById(Guid id)
        {
            return FindByKeys(id).FirstOrDefault();
        }

        public IEnumerable<Customer> FindWithExpression(Expression<Func<Customer, bool>> filterExpression)
        {
            return customers.Where(filterExpression.Compile());
        }

        public IEnumerable<Customer> FindByKeys(params Guid[] ids)
        {
            return customers.Where(customer => ids.Contains(customer.Id));
        }

        public ISearchResult<Customer> GetPaginated<TSearch>(ISearchRequest<TSearch> searchRequest) where TSearch : class
        {
            var search = searchRequest.Search as ISearch<Customer>;
            var include = searchRequest.Search as IInclude<Customer>;
            var orderBy = searchRequest.Search as IOrderBy<Customer>;

            var page = searchRequest.Page is not null && searchRequest.Page.Value > 0 ? searchRequest.Page.Value : 1;
            var pageSize = searchRequest.PageSize is not null && searchRequest.PageSize.Value > 0
                ? searchRequest.PageSize.Value
                : 10;

            var query = customers.AsQueryable();

            query = ApplySearch(query, search?.Searcher());

            query = ApplyInclude(query, include?.Include());

            query = ApplyOrderBy(query, orderBy?.OrderBy());

            return new SearchResult<Customer>()
                .SetPage(page)
                .SetPageSize(searchRequest.PageSize)
                .SetTotalCount(query.Count())
                .SetPaginatedItems(ApplyPagination(query, page, pageSize));
        }

        private static IEnumerable<Customer> ApplyPagination(IQueryable<Customer> query, int page, int? size)
        {
            var skip = --page * (size ?? 0);

            query = query.Skip(skip);

            if (size.HasValue)
            {
                query = query.Take(size.Value);
            }

            return query.AsEnumerable();
        }

        private static IQueryable<Customer> ApplySearch(IQueryable<Customer> query, Expression<Func<Customer, bool>> search = null)
        {
            var searchIsNotNull = search != null;
            if (searchIsNotNull)
            {
                query = query.Where(search);
            }

            return query;
        }

        private static IQueryable<Customer> ApplyInclude(IQueryable<Customer> query, Func<IQueryable<Customer>, IQueryable<Customer>> includes = null)
        {
            var includesIsNotNull = includes != null;
            if (includesIsNotNull)
            {
                query = includes(query);
            }

            return query;
        }

        private static IQueryable<Customer> ApplyOrderBy(IQueryable<Customer> query, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null)
        {
            var orderByIsNotNull = orderBy != null;
            if (orderByIsNotNull)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}