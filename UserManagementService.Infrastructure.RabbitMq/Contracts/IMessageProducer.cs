namespace UserManagementService.Infrastructure.RabbitMq.Contracts
{
    public interface IMessageProducer
    {
        Task PublishTotallyDeadMessage(string message);

        Task PublishCreatedUserAsync(string message);
    }
}