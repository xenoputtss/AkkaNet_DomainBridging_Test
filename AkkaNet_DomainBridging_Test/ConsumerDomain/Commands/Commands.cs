namespace AkkaNet_DomainBridging_Test.ConsumerDomain.Commands
{
    public class CreateConsumer : IAmAnAggregateMessage
    {
        public string UserName { get; private set; }
        public string Pin { get; private set; }

        public CreateConsumer(string userName, string pin, string aggregateId)
        {
            UserName = userName;
            Pin = pin;
            AggregateId = aggregateId;
        }

        private string AggregateId { get; }
        string IAmAnAggregateMessage.AggregateId => AggregateId;
    }

    public class AddFirstName : IAmAnAggregateMessage
    {
        public string FirstName { get; private set; }

        public AddFirstName(string firstName, string aggregateId)
        {
            FirstName = firstName;
            AggregateId = aggregateId;
        }

        private string AggregateId { get; }
        string IAmAnAggregateMessage.AggregateId => AggregateId;
    }

    public class AddLastName : IAmAnAggregateMessage
    {
        public string LastName { get; private set; }

        public AddLastName(string lastName, string aggregateId)
        {
            AggregateId = aggregateId;
            LastName = lastName;
        }

        private string AggregateId { get; }
        string IAmAnAggregateMessage.AggregateId => AggregateId;
    }

}
