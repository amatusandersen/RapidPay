using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Repositories.Common;

namespace RapidPay.Infrastructure.Persistence.Repositories
{
    public class FeeRepository(RapidPayDbContext context) : BaseRepository<Fee>(context), IFeeRepository;
}
