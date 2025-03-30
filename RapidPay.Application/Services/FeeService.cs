using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Infrastructure;

namespace RapidPay.Application.Services
{
    public class FeeService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly Random random = new();

        public decimal CurrentFee { get; private set; } = 0.05m;

        private async Task UpdateFeeAsync()
        {
            decimal randomMultiplier = (decimal)random.NextDouble() * 2m;

            CurrentFee *= randomMultiplier;

            var feeHistory = new Fee
            {
                Id = Guid.NewGuid(),
                Amount = CurrentFee,
                Timestamp = DateTime.UtcNow
            };

            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;

            await unitOfWork.FeeRepository.AddAsync(feeHistory);
            await unitOfWork.SaveChangesAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateFeeAsync();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
