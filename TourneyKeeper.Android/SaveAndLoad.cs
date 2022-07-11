using System;
using Xamarin.Forms;
using System.IO;
using TourneyKeeper.Droid;
using TourneyKeeper.Common;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace TourneyKeeper.Droid
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public void DeleteText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.Delete(filePath);
        }
        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                return File.ReadAllText(filePath);
            }
            catch(FileNotFoundException)
            {
                //hvis den forsøger refresh inden vi er logget på, det er ok, da token ryger afsted når de logger ind
                return null;
            }
        }
    }
}