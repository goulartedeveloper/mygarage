using System.Threading.Tasks;
using Garage.Domain.Messages;
using Garage.Infrastructure.Database;
using Rebus.Handlers;

namespace Garage.Domain.Handlers
{
    public abstract class BaseHandler<TMessage> : IHandleMessages<TMessage>
        where TMessage : IBaseMessage
    {
        protected GarageContext _garageContext;

        public BaseHandler(GarageContext garageContext)
        {
            _garageContext = garageContext;
        }

        public async Task Handle(TMessage message)
        {
            _garageContext.SetUserId(message.UserId);
            await ExecuteAsync(message);
        }

        public abstract Task ExecuteAsync(TMessage message);
    }
}
