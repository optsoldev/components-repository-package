using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Infra.Mock.Entities.Core
{
    public sealed class Customer : AggregateRoot
    {
        public Person Person { get; private set; }

        public Email Email { get; private set; }

        public DateValue BirthDate { get; private set; }

        public static Customer Create(Person person, Email email, DateValue birthDate) =>
            new()
            {
                Person = person,
                Email = email,
                BirthDate = birthDate
            };
    }
}
