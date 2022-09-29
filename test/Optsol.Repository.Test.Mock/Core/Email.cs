using Optsol.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Optsol.Repository.Test.Mock.Entities.Core
{
    public class Email : ValueObject
    {
        const string regexEmailPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        private Email(string address)
        {
            SetAddressValue(address);
        }

        public string Address { get; private set; }

        public Email SetAddressValue(string address)
        {
            var checkEmailIsInvalid = !Regex.IsMatch(address, regexEmailPattern);
            if (checkEmailIsInvalid)
                throw new InvalidOperationException(nameof(SetAddressValue));

            Address = address;

            return this;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }

        public static Email Create(string address) => new(address);
    }
}