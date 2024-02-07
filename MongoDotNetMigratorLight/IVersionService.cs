namespace MongoDotNetMigratorLight
{
    public interface IVersionService
    {
        Task<VersionInfo?> GetlatestVersion();
        Task InsertVersionAsync(string versionName);
        Task<bool> VersionExists(string name);
    }
}