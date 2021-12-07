
using FluentAssertions;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Entities.Core;
using Optsol.Components.Repository.Infra.Mock.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Optsol.Components.Repository.Test.Repositories
{
    public class MockRepositorySpec
    {
        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve retornar a lista com todos os Customers disponíveis")]
        public void Deve_Retornar_Todos_Customers()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            //when
            var customers = readRepository.GetAll();

            //then
            customers.Should().NotBeNull("Não pode estar nulo");
            customers.Should().NotBeEmpty("Não pode estar vazio");
            customers.Should().HaveCount(1, "Deve conter um registro");
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve retornar somente o customer com o id informado")]
        public void Deve_Retornar_Customer_Por_Id()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();
            var customer = readRepository.GetAll().First();
            //when
            var customerById = readRepository.GetById(customer.Id);

            //then
            customerById.Should().NotBeNull("Não pode estar nulo");
            (customerById == customer).Should().BeTrue("Deve ser o mesmo objeto");
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Não deve retornar nenhum customer com o id informado inexistente")]
        public void Nao_Deve_Retornar_Customer_Por_Id()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();
            var customer = CreateCustomer();

            //when
            var customerById = readRepository.GetById(customer.Id);

            //then
            customerById.Should().BeNull("Não pode retornar um customer");
            (customerById == customer).Should().BeFalse("Não pode ser o mesmo objeto");
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve retornar somente os customers com os ids informados")]
        public void Deve_Retornar_Customer_Por_Ids()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();
            var customers = readRepository.GetAll();
            var customersIds = customers.Select(customer => customer.Id).ToArray();

            //when
            var customersById = readRepository.GetAllByIds(customersIds);

            //then
            customersById.Should().NotBeNull("Não pode estar nulo");
            customersById.Should().NotBeEmpty("Não pode estar vazio");
            customersById.Should().HaveCount(customersIds.Length, "Deveria conter a mesma quantidade de registro");
            customersById.All(x => customers.Any(y => x == y)).Should().BeTrue("Deve ser o mesmo objeto");
        }

        [Trait("Repositories", "Métodos Leitura")]
        [Fact(DisplayName = "Deve retornar somente os customers com os ids informados")]
        public void Deve_Retornar_Customers_Por_Expression()
        {
            //given
            IReadRepository<Customer> readRepository = new MockRepository();

            var filterExpression = ExpressionFilter("Weslley");

            //when
            var customers = readRepository.GetWithExpression(filterExpression);

            //then
            customers.Should().NotBeNull("Não pode estar nulo");
            customers.Should().NotBeEmpty("Não pode estar vazio");
            customers.Should().HaveCount(1, "Deveria conter um registro");
        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve inserir um novo customer no contexto")]
        public void Deve_Inserir_Novo_Customer()
        {
            //given
            var newCustomer = CreateCustomer();

            IWriteRepository<Customer> writeRepository = new MockRepository();

            //when
            Action execute = () => writeRepository.Insert(newCustomer);

            //then
            execute.Should().NotThrow();
            (writeRepository as MockRepository).GetAll().Should().HaveCount(2);

        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve atualizar um customer adicionado no contexto")]
        public void Deve_Atualizar_Customer()
        {
            //given
            var newCustomer = CreateCustomer();

            IWriteRepository<Customer> writeRepository = new MockRepository();
            writeRepository.Insert(newCustomer);

            var updateCustumer = newCustomer;
            updateCustumer.BirthDate.SetDateValueWithDate(DateTime.Parse("2009-02-15"));

            //when
            Action execute = () => writeRepository.Update(updateCustumer);

            //then
            execute.Should().NotThrow("Não deveria de acontecer erro");
            (newCustomer == updateCustumer).Should().BeTrue("Devem ser a mesma entidade");
            (writeRepository as MockRepository).GetAll().Should().HaveCount(2);

        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve remover um customer do contexto")]
        public void Deve_Deletar_Customer()
        {
            //given
            var newCustomer = CreateCustomer();

            IWriteRepository<Customer> writeRepository = new MockRepository();
            writeRepository.Insert(newCustomer);

            //when
            Action execute = () => writeRepository.Delete(newCustomer);

            //then
            execute.Should().NotThrow();
            (writeRepository as MockRepository).GetAll().Should().HaveCount(1);
        }

        [Trait("Repositories", "Métodos Escrita")]
        [Fact(DisplayName = "Deve salvar mudanças no contexto")]
        public void Deve_Salvar_Mudancas_Contexto()
        {
            //given
            var newCustomer = CreateCustomer();

            IWriteRepository<Customer> writeRepository = new MockRepository();

            //when
            writeRepository.Insert(newCustomer);
            var totalItems = writeRepository.SaveChanges();

            //then
            totalItems.Should().Be(2);
        }

        public static Customer CreateCustomer()
        {
            var novaPessoa = Person.Create("Novo Customer", "Teste");
            var novoEmail = Email.Create("email@optsol.com.br");
            var novaDataNascimento = DateValue.Create().SetDateValueWithDate(DateTime.Parse("01/01/2011"));

            //when
            var customerObject = Customer.Create(novaPessoa, novoEmail, novaDataNascimento);

            return customerObject;
        }

        private static Expression<Func<Customer, bool>> ExpressionFilter(string name)
        {
            return (Customer customer) => customer.Person.Name.Contains(name);
        }
    }
}
