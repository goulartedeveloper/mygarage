using System.Threading.Tasks;
using Garage.Domain.Interfaces;
using Garage.Domain.Messages.Vehicle;
using Garage.Infrastructure.Database;
using Microsoft.Extensions.Logging;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleCreatedHandler : BaseHandler<VehicleCreatedMessage>
    {
        private readonly ILogger<VehicleCreatedHandler> _logger;
        private readonly GarageContext _context;
        private readonly IEmailService _emailService;

        public VehicleCreatedHandler(ILogger<VehicleCreatedHandler> logger, GarageContext context, IEmailService emailService)
            : base(context)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public override async Task ExecuteAsync(VehicleCreatedMessage message)
        {
            _logger.LogInformation("Message: VehicleCreatedMessage received.");
            var user = await _context.Users.FindAsync(message.UserId);

            var garage = await _context.Garages.FindAsync(message.GarageId);

            await _emailService.SendEmailAsync(
                user?.Email,
                $"Novo veículo cadastrado na garagem {garage?.Name}",
                $"<h1>Veículo {message.Plate} cadastrado com sucesso!</h1>");

            _logger.LogInformation("Message: VehicleCreatedMessage processed with success!");
        }
    }
}
