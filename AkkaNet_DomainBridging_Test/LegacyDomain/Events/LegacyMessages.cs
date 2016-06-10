namespace AkkaNet_DomainBridging_Test.LegacyDomain.Events
{

    public class UserNameAdded : IAmAnAggregateMessage
    {
        public string UserName { get; private set; }
        public UserNameAdded(string userName, string aggregateId)
        {
            UserName = userName;
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }
    }

    public class PinAdded : IAmAnAggregateMessage
    {
        public string Pin { get; private set; }
        public PinAdded(string pin, string aggregateId)
        {
            Pin = pin;
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }
    }

    public class FirstNameAdded : IAmAnAggregateMessage
    {
        public string FirstName { get; private set; }

        public FirstNameAdded(string firstName, string aggregateId)
        {
            FirstName = firstName;
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }
    }

    public class LastNameAdded : IAmAnAggregateMessage
    {
        public string LastName { get; private set; }

        public LastNameAdded(string lastName, string aggregateId)
        {
            LastName = lastName;
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }
    }
}
