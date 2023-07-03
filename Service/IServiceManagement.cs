namespace Pros.Service
{
    public interface IServiceManagement
    {
        void SendEmail();

        void UpdateDatabase();

        void GenerateMerchandise();

        void SyncData();
    }
}
