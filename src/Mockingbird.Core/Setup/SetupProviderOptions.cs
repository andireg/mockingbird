namespace Mockingbird.Setup
{
    public class SetupProviderOptions
    {
        public SetupProviderOptions(
            string setupFile, 
            Action<string>? logOutput = null)
        {
            SetupFile = setupFile;
            LogOutput = logOutput;
        }

        public string SetupFile { get; set; }

        public Action<string>? LogOutput { get; set; }
    }
}