using System.Threading;
using System.Threading.Tasks;

namespace Mockingbird.Tests.Services
{
    public interface IInterfaceServiceArgument
    {
        Task<string> GetTextAsync(string text, CancellationToken cancellationToken);
    }
}