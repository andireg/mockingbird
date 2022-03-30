namespace Mockingbird.Tests.Services
{
    public class RefAndOutService
    {
        private readonly IRefService refService;
        private readonly IOutService outService;

        public RefAndOutService(IRefService refService, IOutService outService)
        {
            this.refService = refService;
            this.outService = outService;
        }

        public bool RefMethod(decimal number, ref string text) => refService.RefMethod(number, ref text);

        public bool OutMethod(decimal number, out string text) => outService.OutMethod(number, out text);
    }
}
