using Optsol.Domain.Entities;
using Optsol.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.Mock.Exceptions;
using System.Text.RegularExpressions;

namespace Optsol.Components.Repository.Infra.Mock.Core
{
    public class CreditCard : Entity, IEntityCreatable
    {
        const string regexCodePattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";

        public CreditCard()
        {
            CreatedDate = DateValue.Create().SetDateValueWithDateOfNow();
        }

        private void SetCardCode(int code)
        {
            var codeIsInvalid = Regex.IsMatch(code.ToString(), regexCodePattern);
            if (codeIsInvalid)
                throw new CreditCardException("Invalid code verification");

            Code = code;
        }

        public string Number { get; private set; }

        public int Code { get; private set; }

        public DateValue Validity { get; private set; }

        public DateValue CreatedDate { get; private set; }

        public static CreditCard Create(string number, int code, DateValue validity)
        {
            var newCreditCard = new CreditCard();
            
            newCreditCard.SetCardCode(code);
            
            newCreditCard.Number = number;
            newCreditCard.Validity = validity;

            return newCreditCard;
        }
    }
}
