using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Optsol.Components.Repository.Infra.Mock.Entities.Core
{
    public class Email : ValueObject
    {
        const string regexEmailPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public string Address { get; private set; }

        public Email SetAddressValue(string address)
        {
            var checkEmailIsValid = Regex.IsMatch(Address, regexEmailPattern);
            if (checkEmailIsValid)
                throw new InvalidOperationException(nameof(SetAddressValue));

            Address = address;

            return this;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }

        public static Email Create() => new();
    }
}