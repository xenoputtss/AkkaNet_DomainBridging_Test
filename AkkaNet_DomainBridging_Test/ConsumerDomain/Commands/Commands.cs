namespace AkkaNet_DomainBridging_Test.ConsumerDomain.Commands
{
    public class CreateConsumer
    {
        public string UserName { get; private set; }
        public string Pin { get; private set; }

        public CreateConsumer(string userName, string pin)
        {
            UserName = userName;
            Pin = pin;
        }
    }

    public class AddFirstName
    {
        public string FirstName { get; private set; }

        public AddFirstName(string firstName)
        {
            FirstName = firstName;
        }
    }

    public class AddLastName
    {
        public string LastName { get; private set; }

        public AddLastName(string lastName)
        {
            LastName = lastName;
        }
    }

}
