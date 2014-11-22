using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ViralTree.Utilities;
using ViralTree.World;

namespace ViralTree.Tiled
{
    //TODO: everything. But its atm mostly game depentend
    public class TiledReader
    {
        public List<EntityAttribs> entityAttributs = new List<EntityAttribs>();

        public List<Vector3i> tileIds = new List<Vector3i>();
        public String tileSetName = null;

        public int tileSetSizeX = 0;
        public int tileSetSizeY = 0;

        public int numTilesX    = 0;
        public int numTilesY    = 0;

        public int tileSizeX    = 0;
        public int tileSizeY    = 0;

        public int spatialSizeX = 0;
        public int spatialSizeY = 0;



        public TiledReader()
        {

        }


        [Conditional(Settings.Constants.DEBUG_CONDITIONAL_STRING)]
        public void printAll()
        {
            Console.WriteLine("TileSizeX = " + tileSizeX);
            Console.WriteLine("TileSizeY = " + tileSizeY);

            Console.WriteLine("TileSetSizeX = " + tileSetSizeX);
            Console.WriteLine("TileSetSizeY = " + tileSetSizeY);

            Console.WriteLine("NumTilesX = " + numTilesX);
            Console.WriteLine("NumTilesY = " + numTilesY);

            Console.WriteLine("SpatialSizeX = " + spatialSizeX);
            Console.WriteLine("SpatialSizeY = " + spatialSizeY);

            Console.WriteLine("TileSetName = " + tileSetName);

        }

        [Conditional(Settings.Constants.DEBUG_CONDITIONAL_STRING)]
        public void print(Object s)
        {
            Console.WriteLine();
            Console.WriteLine(s);

        }

        public void Load(String name)
        {
            XmlReader reader = XmlReader.Create(name);

            while (reader.Read())
            {
                if (reader.Name.Equals("map"))
                    break;

            }

            LoadMap(reader);

        }

        public void LoadMap(XmlReader reader)
        {
            //print(reader.Name);

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("width"))
                    numTilesX = int.Parse(reader.Value);

                else if (reader.Name.Equals("height"))
                    numTilesY = int.Parse(reader.Value);

                else if (reader.Name.Equals("tilewidth"))
                    tileSizeX = int.Parse(reader.Value);

                else if (reader.Name.Equals("tileheight"))
                    tileSizeY = int.Parse(reader.Value);
            }

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    // print(reader.Name);
                    if (reader.Name.Equals("properties"))
                        LoadMapProps(reader);

                    else if (reader.Name.Equals("tileset"))
                        LoadTileSet(reader);

                    else if (reader.Name.Equals("layer"))
                        LoadTileLayer(reader);

                    else if (reader.Name.Equals("objectgroup"))
                        LoadObjectGroup(reader);


                }

                // printCurrent(reader);
            }

            printAll();
        }

        private void LoadObjectGroup(XmlReader reader)
        {
            print("found a objectgroup");

            //objectgroup name:
            while (reader.MoveToNextAttribute())
            {
                //printCurrent(reader);
            }

            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("objectgroup")))
            {
                if (reader.Name.Equals("object"))
                {

                    reader.MoveToNextAttribute();


                    if (reader.Value.Equals("Spawner"))
                        LoadSpawner(reader);

                    else if (reader.Value.Equals("Collision"))
                        LoadCollision(reader);
                }


            }

            print("abort");
        }

        private void LoadSpawner(XmlReader reader)
        {

        }

        private void LoadCollision(XmlReader reader)
        {
            Vector2f start = new Vector2f(0, 0);
            List<Vector2f> vertices = new List<Vector2f>();
            Vector2f center = new Vector2f(0, 0);

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("x"))
                    start.X = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    start.Y = float.Parse(reader.Value, Settings.cultureProvide);
            }

            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("object")))
            {
                if (reader.Name.Equals("polygon"))
                {
                    reader.MoveToNextAttribute();
                    center = LoadPoints(reader, vertices, start);
                }
            }

            entityAttributs.Add(new EntityAttribs(EntityType.Collision, new ConvexCollider(MathUtil.ToArray<Vector2f>(vertices)), center));
        }

        private Vector2f LoadPoints(XmlReader reader, List<Vector2f> vertices, Vector2f start)
        {
            Vector2f center = new Vector2f();

       //     Console.WriteLine(start);

//            print("foundPoints");
            char[] splits = { ' ' };
            char[] subSplit = { ',' };
            String[] vectors = reader.Value.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < vectors.Length; i++)
            {
                String[] components = vectors[i].Split(subSplit);

                float tmpX = float.Parse(components[0], Settings.cultureProvide);
                float tmpY = float.Parse(components[1], Settings.cultureProvide);

                Vector2f tmpVertex = new Vector2f(tmpX, tmpY);
                vertices.Add(start + tmpVertex);

                center += vertices[i];

            //   Console.WriteLine(vertices[i]);
            }
            center /= vertices.Count;

        //    Console.WriteLine(center);
 

            return center;
        }

        private void LoadTileLayer(XmlReader reader)
        {
          //  print("found a layer");
            int tileCount = 0;
            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("layer")))
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name.Equals("gid"))
                    {
                        int id = int.Parse(reader.Value) - 1;
                        
                        int x = tileCount % numTilesX;
                        int y = tileCount / numTilesX;

                        tileIds.Add(new Vector3i(x, y, id));
                        tileCount++;
                    }

                }
            }
        }

        private void LoadTileSet(XmlReader reader)
        {
          //  print("found a tileSet");
            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("tileset")))
            {
                if (reader.Name.Equals("image"))
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name.Equals("source"))
                            this.tileSetName = reader.Value;

                        else if (reader.Name.Equals("width"))
                            this.tileSetSizeX = int.Parse(reader.Value);

                        else if (reader.Name.Equals("height"))
                            this.tileSetSizeY = int.Parse(reader.Value);
                    }
                }


            }
        }

        private void LoadMapProps(XmlReader reader)
        {
          //  print("found map props");
            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("properties")))
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Value.Equals("SpatialSizeX"))
                    {
                        reader.MoveToNextAttribute();
                        spatialSizeX = int.Parse(reader.Value);
                    }

                    else if (reader.Value.Equals("SpatialSizeY"))
                    {
                        reader.MoveToNextAttribute();
                        spatialSizeY = int.Parse(reader.Value);
                    }
                }
            }

        }

        private void printCurrent(XmlReader reader)
        {
            print(reader.Name + " = " + reader.Value);
        }



    }
}
