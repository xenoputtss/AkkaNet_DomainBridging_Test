namespace AkkaNet_DomainBridging_Test.LegacyDomain.Events
{

    public class UserNameAdded
    {
        public string UserName { get; private set; }
        public UserNameAdded(string userName)
        {
            UserName = userName;
        }
    }

    public class PinAdded
    {
        public string Pin { get; private set; }
        public PinAdded(string pin)
        {
            Pin = pin;
        }
    }

    public class FirstNameAdded
    {
        public string FirstName { get; private set; }

        public FirstNameAdded(string firstName)
        {
            FirstName = firstName;
        }
    }

    public class LastNameAdded
    {
        public string LastName { get; private set; }

        public LastNameAdded(string lastName)
        {
            LastName = lastName;
        }
    }

}
