namespace Mockingbird.Tests.Services
{
    public class SubInterfaceUsedClass
    {
        private readonly ISubInterface subInterface;

        public SubInterfaceUsedClass(ISubInterface subInterface)
        {
            this.subInterface = subInterface;
        }

        public string Text
        {
            get => subInterface.Text; 
            set => subInterface.Text = value;
        }

        public string GetText() => subInterface.GetText();

        public decimal Number
        {
            get => subInterface.Number;
            set => subInterface.Number = value;
        }

        public decimal GetNumber() => subInterface.GetNumber();
    }
}
