using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Orgainizer.Classes
{
    public static class DataManagment
    {
        private static List<string> _Media = new List<string>();
        private static Dictionary<string, List<Item>> _Content = new Dictionary<string, List<Item>>();
        private static bool isLoaded = false;

        /// <summary>
        /// Checks if data is filled
        /// </summary>
        public static bool Loaded
        {
            get
            {
                return isLoaded;
            }
        }

        public static List<string> Media
        {
            get
            {
                return _Media;
            }
        }

        public static Dictionary<string, List<Item>> Content
        {
            get
            {
                return _Content;
            }
        }

        /// <summary>
        /// Loads information
        /// </summary>
        public static void Load()
        {
            Load("items");
        }

        /// <summary>
        /// Loads information
        /// </summary>
        /// <param name="directory">Directory to load from.</param>
        public static void Load(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (File.Exists(directory + Path.DirectorySeparatorChar + "media.txt"))
            {
                string media = File.ReadAllText(directory + Path.DirectorySeparatorChar + "media.txt");
                if (media != "")
                {
                    string[] mediaItems = media.Split(',');
                    _Media.AddRange(mediaItems);
                }
            }
            else
            {
                FileStream f = File.Create(directory + Path.DirectorySeparatorChar + "media.txt");
                f.Close();
            }
            foreach (string s in _Media)
            {
                if (File.Exists(directory + Path.DirectorySeparatorChar + s + ".txt"))
                {
                    if (!_Content.ContainsKey(s))
                    {
                        _Content.Add(s, new List<Item>());
                    }
                    string media = File.ReadAllText(directory + Path.DirectorySeparatorChar + s + ".txt");
                    if (media != "")
                    {
                        string[] mediaItems = media.Split(',');
                        foreach (string mi in mediaItems)
                        {
                            if (mi.Contains("|"))
                            {
                                string[] mic = mi.Split('|');
                                Series ss = new Series();
                                ss.Name = mic[0];
                                ss.Season = Convert.ToInt32(mic[1]);
                                ss.Episode = Convert.ToInt32(mic[2]);
                                _Content[s].Add(ss);
                            }
                        }
                    }
                }
                else
                {
                    FileStream f = File.Create(directory + Path.DirectorySeparatorChar + s + ".txt");
                    f.Close();
                }
            }
        }

        /// <summary>
        /// Saves the information
        /// </summary>
        public static void Save()
        {
            Save("items");
        }

        /// <summary>
        /// Saves the information
        /// </summary>
        /// <param name="directory">Directory to save information to</param>
        public static void Save(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(directory + Path.DirectorySeparatorChar + "media.txt"))
            {
                FileStream s = File.Create(directory + Path.DirectorySeparatorChar + "media.txt");
                s.Close();
            }

            string toWrite = "";
            if (_Media.Count > 0) toWrite = _Media[0];
            for (int i = 1; i < _Media.Count; i++)
            {
                toWrite += ",";
                toWrite += Media[i];
            }
            File.WriteAllText(directory + Path.DirectorySeparatorChar + "media.txt", toWrite);
            foreach (string s in _Content.Keys)
            {
                if (_Content[s].Count > 0) toWrite = _Content[s][0].ToString();
                else toWrite = "";
                for (int i = 1; i < _Content[s].Count; i++)
                {
                    toWrite += ",";
                    toWrite += _Content[s][i].ToString();
                }
                File.WriteAllText(directory + Path.DirectorySeparatorChar + s + ".txt", toWrite);
            }
        }

        public static bool SaveSeries(string media, string name, int season, int episode)
        {
            try
            {
                if (!_Content.ContainsKey(media))
                {
                    _Content.Add(media, new List<Item>());
                }
                Series s = new Series();
                s.Name = name;
                s.Season = season;
                s.Episode = episode;
                _Content[media].Add(s);
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
