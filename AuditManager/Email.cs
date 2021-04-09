namespace AuditManager
{
    public class Email: ValueObject<Email>
    {
        public readonly string _value;

        private Email(string value)
        {
            _value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                return Result.Fail<Email>("Email should not be empty");

            email = email.Trim();
            if (email.Length > 256)
                return Result.Fail<Email>("Email is too long");

            if(!email.Contains('@'))
                return Result.Fail<Email>("Email is invalid");

            return Result.Ok(new Email(email));
        }

        protected override bool EqualsCore(Email email)
        {
            return email._value == _value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }

        public static implicit operator string(Email email) => email._value; 

        public static explicit operator Email(string email) => Create(email).Value;
    }
}
