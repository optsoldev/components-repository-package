﻿using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace Optsol.Components.Repository.Infra.Mock.Core
{
    public class CreaditCard : Entity, IEntityCreatable
    {
        const string regexCodePattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";

        public CreaditCard()
        {
            CreateDate = DateValue.Create().SetDateValueWithDateOfNow();
        }

        private void SetCardCode(int code)
        {
            var codeIsInvalid = Regex.IsMatch(code.ToString(), regexCodePattern);
            if (codeIsInvalid)
                throw new CreaditCardException("Invalid code verification");

            Code = code;
        }

        public string Number { get; private set; }

        public int Code { get; private set; }

        public DateValue Validity { get; private set; }

        public DateValue CreateDate { get; private set; }

        public static CreaditCard Create(string number, int code, DateValue validity)
        {
            var newCreditCard = new CreaditCard();
            
            newCreditCard.SetCardCode(code);
            
            newCreditCard.Number = number;
            newCreditCard.Validity = validity;

            return newCreditCard;
        }
    }
}