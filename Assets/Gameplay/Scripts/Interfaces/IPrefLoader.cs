public interface IPrefLoader
{
    string PrefKey { get; }
    void LoadData(string data);
}