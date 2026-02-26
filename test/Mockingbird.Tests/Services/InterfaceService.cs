using System.Threading;
using System.Threading.Tasks;

namespace Mockingbird.Tests.Services
{
    public class InterfaceService
    {
        private readonly IInterfaceServiceArgument argument;

        public InterfaceService(IInterfaceServiceArgument argument)
        {
            this.argument = argument;
        }

        public Task<string> GetTextAsync(string text, CancellationToken cancellationToken)
        {
            return argument.GetTextAsync(text, cancellationToken);
        }
    }
}