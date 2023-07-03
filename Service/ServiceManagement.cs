namespace Pros.Service
{
    public class ServiceManagement : IServiceManagement
    {
        public void GenerateMerchandise()
        {
            Console.WriteLine($"Generate Merchandise: Logn Running Task {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail()
        {
            Console.WriteLine($"SendEmail: Short Running Task {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

        }

        public void SyncData()
        {
            Console.WriteLine($"SyncData: Short Running Task {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

        }

        public void UpdateDatabase()
        {
            Console.WriteLine($"UpdateDatabase: Logn Running Task {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

        }
    }
}
