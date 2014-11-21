using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    /// <summary>
    /// A class that can load and hold IDisposables e.g. Textures, Images, Shader,....
    /// </summary>
    public class ContentManager
    {
        private string rootDir;
        private Dictionary<AssetKey, object> assetDictionary;

        /// <summary>
        /// The root directory of all Content files.
        /// </summary>
        public string RootDirectory
        {
            get { return rootDir; }
            set
            {
                if (assetDictionary.Count == 0)
                    rootDir = value;

                else
                    throw new Exception("ContentManager cant change the RootDirectory when it already has loaded content.");
            }
        }

        /// <summary>
        /// The number of loaded assets.
        /// </summary>
        public int Count
        {
            get { return assetDictionary.Count; }
        }

        /// <summary>
        /// Creates a ContentManager with a given root diretory.
        /// </summary>
        /// <param name="rootDirectory">The root directory, is standardly "Content"</param>
        public ContentManager(string rootDirectory = "Content")
        {
            this.assetDictionary = new Dictionary<AssetKey, object>();
            this.RootDirectory = rootDirectory;
        }

        /// <summary>
        /// Loads and returns an asset. If the asset was already loaded, if will just be returned instead of being reloaded before.
        /// </summary>
        /// <typeparam name="T">Must be a class and must be IDisposable, also must be supported by the ContentManager.</typeparam>
        /// <param name="path">Where the asset lays, also needs the file ending.</param>
        /// <returns>The loaded asset.</returns>
        public T Load<T>(string path) where T : class, IDisposable
        {
            return (T)Load(typeof(T), path);
        }

        /// <summary>
        /// Disposes all loaded assets.
        /// </summary>
        public void DisposeAll()
        {
            foreach (object o in assetDictionary)
            {
                IDisposable disposable = o as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }

            assetDictionary.Clear();
        }

        /// <summary>
        /// Tells if an asset is already loaded.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="path">The path and name of the asset.</param>
        /// <returns>True if the ContentManager knows this file, false otherwise.</returns>
        public bool Exists<T>(string path)
        {
            return assetDictionary.ContainsKey(new AssetKey(typeof(T), path));
        }

        /// <summary>
        /// Removes and diposes an asset from the ContentManager if it was loaded earlier.
        /// </summary>
        /// <typeparam name="T">The type of the file.</typeparam>
        /// <param name="path">The path and the name of the file.</param>
        /// <returns>True if the asset was loaded and now removed, false otherwise.</returns>
        public bool Remove<T>(string path) where T : class, IDisposable
        {

            AssetKey tmpKey = new AssetKey(typeof(T), path);
            T blah = (T)assetDictionary[tmpKey];
            blah.Dispose();
            return assetDictionary.Remove(tmpKey);
        }


        private object Load(Type type, string path)
        {
            AssetKey tmpKey = new AssetKey(type, path);
            object result = null;

            if (assetDictionary.TryGetValue(tmpKey, out result))
            {
                return result;
            }


            else
            {
                string combinedPath = string.Concat(RootDirectory, "/", path);

                if (type == typeof(Image))
                {
                    result = new Image(combinedPath);
                    assetDictionary.Add(tmpKey, result);
                }

                else if (type == typeof(Texture))
                {
                    result = new Texture(combinedPath);
                    assetDictionary.Add(tmpKey, result);
                }

                else if (type == typeof(SoundBuffer))
                {
                    result = new SoundBuffer(combinedPath);
                    assetDictionary.Add(tmpKey, result);
                }

                //TODO: maybe add functionality to load both vertex and pixelshader... or only one of them.
                else if (type == typeof(Shader))
                {
                    result = new Shader(null, path);
                    assetDictionary.Add(tmpKey, result);
                }

                else if (type == typeof(Font))
                {
                    result = new Font(combinedPath);
                    assetDictionary.Add(tmpKey, result);
                }

                //TODO: add other loadable resources

                else
                    throw new Exception("Unknown type: " + type + " for loading " + path);


                return result;
            }

        }
    }
}
