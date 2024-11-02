using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SnakeGames
{
    public class Utils
    {

        static string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/" + Constant.FILE_SCORE;
        public static List<HighCoreModel> ReadFileHighScore()
        {
            try
            {
                List<HighCoreModel> models = new List<HighCoreModel>();
                CheckExistFileAndCreate();
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                foreach (var item in lines)
                {
                    string[] line = item.Split(' ');
                    if (line.Length <= 1)
                        continue;

                    models.Add(new HighCoreModel()
                    {
                        Id = int.Parse(line[0]),
                        Name = line[1],
                        Score = int.Parse(line[2])
                    });
                }
                return models;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CheckExistFileAndCreate()
        {
            if (!File.Exists(path))
                 File.Create(path);
        }

        public static void WriteFileHighScore(List<HighCoreModel> models)
        {
            var sb = new StringBuilder();
            using (StreamWriter streamWrite = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                int i = 1;
                foreach (var item in models)
                {
                    var line = i + " " + item.Name + " "+ item.Score;
                    sb.Append(line);
                    sb.Append(Environment.NewLine);
                    i++;
                }
                streamWrite.WriteLine(sb.ToString());
            }
        }
    }
}
