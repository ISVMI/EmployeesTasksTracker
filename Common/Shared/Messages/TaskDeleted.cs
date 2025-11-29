using Shared.DTOs;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public record TaskDeleted(Guid DeletedTaskId, DateTime DeletedAt);
}
