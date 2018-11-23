using System;
using System.Threading.Tasks;
using PowerArgs;

namespace SecretSanta
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var parsed = await Args.InvokeMainAsync<SantaArgs>(args);
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<SantaArgs>());
            }
        }
    }
}
