﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ViralTree.Components;
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


        public Vector2f spawnPos;

        public int numKeys = 0;

        public ExitResponse exit;


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

//            printAll();
        }

        private void LoadObjectGroup(XmlReader reader)
        {
        //    print("found a objectgroup");

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

                    else if (reader.Value.Equals("PlayerSpawner"))
                        LoadPlayerSpawner(reader);

                    else if (reader.Value.Equals("Health"))
                        LoadHealth(reader);

                    else if (reader.Value.Equals("Key"))
                        LoadKey(reader);

                    else if (reader.Value.Equals("LeavePoint"))
                        LoadExit(reader);
                }


            }

           // print("abort");
        }

        private void LoadExit(XmlReader reader)
        {
            FloatRect rect = new FloatRect();

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("x"))
                    rect.Left = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    rect.Top = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("width"))
                    rect.Width = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("height"))
                    rect.Height = float.Parse(reader.Value, Settings.cultureProvide);
            }


            EntityAttribs attribs = new EntityAttribs(EntityType.LeavePoint, null, new Vector2f(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2));
            Vector2f[] vertices = {new Vector2f(rect.Left, rect.Top),
                                  new Vector2f(rect.Left, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top)};

            //Console.WriteLine(numKeys);
            attribs.AddAttribute(numKeys);

            attribs.collider = new ConvexCollider(vertices, true);
            entityAttributs.Add(attribs);
        
        }

        private void LoadKey(XmlReader reader)
        {
            numKeys++;

            FloatRect rect = new FloatRect();

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("x"))
                    rect.Left = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    rect.Top = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("width"))
                    rect.Width = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("height"))
                    rect.Height = float.Parse(reader.Value, Settings.cultureProvide);
            }

            EntityAttribs attribs = new EntityAttribs(EntityType.Key, null, Vec2f.Zero);

            attribs.pos = new Vector2f(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            Vector2f[] vertices = {new Vector2f(rect.Left, rect.Top),
                                  new Vector2f(rect.Left, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top)};

            double time = 0.0;

            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("properties")))
            {
                if (reader.Name.Equals("property"))
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name.Equals("value"))
                        {
 
                            time = double.Parse(reader.Value, Settings.cultureProvide);
                        }
                    }
                }
            }

            attribs.collider = new ConvexCollider(vertices, true);
            attribs.AddAttribute(exit);
            attribs.AddAttribute(time);


            entityAttributs.Add(attribs);
        }

        private void LoadHealth(XmlReader reader)
        {
            FloatRect rect = new FloatRect();
            float healthpoints = 0.0f;

            EntityAttribs attribs = new EntityAttribs(EntityType.Health, null, Vec2f.Zero);

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("x"))
                    rect.Left = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    rect.Top = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("width"))
                    rect.Width = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("height"))
                    rect.Height = float.Parse(reader.Value, Settings.cultureProvide);
            }

            attribs.pos = new Vector2f(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            Vector2f[] vertices = {new Vector2f(rect.Left, rect.Top),
                                  new Vector2f(rect.Left, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top)};

            attribs.collider = new ConvexCollider(vertices, true);
            reader.Read();
            reader.Read();
            if (reader.Name.Equals("properties"))
            {

                   printCurrent(reader);
                   reader.Read();
                   reader.Read();
                while (reader.MoveToNextAttribute())
                {
                    //printCurrent(reader);

                    if (reader.Value.Equals("healedHP"))
                    {
                        reader.MoveToNextAttribute();
                        healthpoints = float.Parse(reader.Value);
                    }
                }
            }

            attribs.AddAttribute(healthpoints);

            entityAttributs.Add(attribs);
        }

        private void LoadPlayerSpawner(XmlReader reader)
        {
            FloatRect rect = new FloatRect();

            while (reader.MoveToNextAttribute())
            {
                if(reader.Name.Equals("x"))
                    rect.Left = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    rect.Top = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("width"))
                    rect.Width = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("height"))
                    rect.Height = float.Parse(reader.Value, Settings.cultureProvide);
            }

            spawnPos.X = rect.Left;
            spawnPos.Y = rect.Top;
        }

        private void LoadSpawner(XmlReader reader)
        {
            FloatRect rect = new FloatRect();
            EntityType type = EntityType.None;
            double cooldown = 0;
            double startTime = 0;
            int numSpawns = 0;

            EntityAttribs attribs = new EntityAttribs(EntityType.Spawner, null, Vec2f.Zero);

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name.Equals("x"))
                    rect.Left = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("y"))
                    rect.Top = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("width"))
                    rect.Width = float.Parse(reader.Value, Settings.cultureProvide);

                else if (reader.Name.Equals("height"))
                    rect.Height = float.Parse(reader.Value, Settings.cultureProvide);
            }

            attribs.pos = new Vector2f(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            Vector2f[] vertices = {new Vector2f(rect.Left, rect.Top),
                                  new Vector2f(rect.Left, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height),
                                  new Vector2f(rect.Left + rect.Width, rect.Top)};

            attribs.collider = new ConvexCollider(vertices, true);


            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("object")))
            {
                if (reader.Name.Equals("property"))
                {

                 //   printCurrent(reader);
                        while (reader.MoveToNextAttribute())
                        {
                            //printCurrent(reader);

                            if (reader.Value.Equals("cooldown")){
                                reader.MoveToNextAttribute();
                                cooldown = double.Parse(reader.Value, Settings.cultureProvide);
                            }


                            else if (reader.Value.Equals("entityType"))
                            {
                                reader.MoveToNextAttribute();
                                type = (EntityType)Enum.Parse(typeof(EntityType), reader.Value, true);
                            }


                            else if (reader.Value.Equals("numSpawns"))
                            {
                                reader.MoveToNextAttribute();
                                numSpawns = int.Parse(reader.Value);
                            }


                            else if (reader.Value.Equals("startTime"))
                            {
                                reader.MoveToNextAttribute();
                                startTime = double.Parse(reader.Value, Settings.cultureProvide);
                            }
                             
                        }
                    
                }
            }

            EntityAttribs spawnAttribs = EntityAttribs.CreateAttrib(type);

            /*
            print(rect);
            print(cooldown);
            print(type);
            print(numSpawns);
            print(startTime);
            */

            attribs.AddAttribute(rect);
            attribs.AddAttribute(cooldown);
            attribs.AddAttribute(type);
            attribs.AddAttribute(numSpawns);
            attribs.AddAttribute(startTime);
            attribs.AddAttribute(spawnAttribs);

            entityAttributs.Add(attribs);
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

            entityAttributs.Add(new EntityAttribs(EntityType.Collision, new ConvexCollider(MathUtil.ToArray<Vector2f>(vertices), false), center));
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
