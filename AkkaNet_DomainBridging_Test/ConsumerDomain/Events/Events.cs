namespace AkkaNet_DomainBridging_Test.ConsumerDomain.Events
{
    public class ConsumerCreated
    {
        public string UserName { get; private set; }
        public string Pin { get; private set; }

        public ConsumerCreated(string userName, string pin)
        {
            UserName = userName;
            Pin = pin;
        }
    }

    public class FirstNameSet
    {
        public string FirstName { get; private set; }

        public FirstNameSet(string firstName)
        {
            FirstName = firstName;
        }
    }

    public class LastNameSet
    {
        public string LastName { get; private set; }

        public LastNameSet(string lastName)
        {
            LastName = lastName;
        }
    }

}
