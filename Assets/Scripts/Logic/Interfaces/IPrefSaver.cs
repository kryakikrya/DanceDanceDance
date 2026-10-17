public interface IPrefSaver
{
    string PrefKey { get; }
    string GetSaveData();
}