using Shared.Enums;

namespace EventBus.Message.Events
{
    public class OrderLogEvent : IntegrationBaseEvent
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
        public OperationType Operation { get; set; }
    }
}
