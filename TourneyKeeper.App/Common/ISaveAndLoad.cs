namespace TourneyKeeper.Common
{
    public interface ISaveAndLoad
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        void DeleteText(string filename);
    }
}
