using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ViralTree.Tiled
{
    //TODO: everything. But its atm mostly game depentend
    public class TiledReader
    {
        private const String TILED_MAP = "map";
        private const String TILED_TILESET = "tileset";
        private const String TILED_POLYGON = "points";
        private const String TILED_PROPERTIES = "properties";
        private const String TILED_SPATIAL_X = "SpatialSizeX";
        private const String TILED_SPATIAL_Y = "SpatialSizeY";


        public String tileSetName = null;

        public int numTilesX = 0;
        public int numTilesY = 0;

        public int tileSizeX = 0;
        public int tileSizeY = 0;

        public int spatialSizeX = 0;
        public int spatialSizeY = 0;



        Stack<String> lastElement = new Stack<string>();
        int numLayers = 0;

        public TiledReader()
        {

        }

        public void Clear()
        {

        }

        public void Load(String path)
        {
            XmlReader reader = XmlReader.Create(path);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    #region Stuff not needed:
                    case XmlNodeType.XmlDeclaration:
                        break;

                    case XmlNodeType.CDATA:
                        break;

                    case XmlNodeType.Whitespace:
                        break;

                    case XmlNodeType.Comment:
                        break;
                    #endregion

                    case XmlNodeType.Element:
                        #region EmptyElement

                        if (reader.IsEmptyElement)
                        {
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {

                                    if (lastElement.Peek().Equals(TILED_PROPERTIES))
                                    {

                                        if (reader.Value.Equals(TILED_SPATIAL_X))
                                        {
                                            reader.MoveToNextAttribute();
                                            spatialSizeX = int.Parse(reader.Value);
                                        }

                                        else if (reader.Value.Equals(TILED_SPATIAL_Y))
                                        {
                                            reader.MoveToNextAttribute();
                                            spatialSizeY = int.Parse(reader.Value);
                                        }

                                    }

                                    else if (lastElement.Peek().Equals(TILED_TILESET))
                                    {
                                        if (reader.Name.Equals("source") && tileSetName == null)
                                            tileSetName = reader.Value;
                                    }

                                    print(reader.Name + " = " + reader.Value);
                                }
                            }

                        }
                        #endregion

                        #region NotEmptyElement
                        else //everything else here
                        {
                            lastElement.Push(reader.Name);
                            print("pushed " + reader.Name);

                            // prüfen, ob der Knoten Attribute hat
                            if (reader.HasAttributes)
                            {
                                // Durch die Attribute navigieren
                                while (reader.MoveToNextAttribute())
                                {
                                    if (lastElement.Peek().Equals(TILED_MAP))
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

                                    print("elements: " + reader.Name + " = " + reader.Value);
                                }
                            }
                        }
                        #endregion
                        break;


                    case XmlNodeType.EndElement:
                        lastElement.Pop();
                        print("popped " + reader.Name);
                        break;

                    case XmlNodeType.Text:

                        //  layers[numLayers] = reader.Value;

                        break;


                }
            }
            // map.tileIds = map.convertTilesToIntArray();

            // map.printAllData();
            //return map;

            printAll();
        }

        [Conditional(Settings.Constants.DEBUG_CONDITIONAL_STRING)]
        public void printAll()
        {
            Console.WriteLine("TileSizeX = " + tileSizeX);
            Console.WriteLine("TileSizeY = " + tileSizeY);

            Console.WriteLine("NumTilesX = " + numTilesX);
            Console.WriteLine("NumTilesY = " + numTilesY);

            Console.WriteLine("SpatialSizeX = " + spatialSizeX);
            Console.WriteLine("SpatialSizeY = " + spatialSizeY);

            Console.WriteLine("TileSetName = " + tileSetName);

        }

        [Conditional(Settings.Constants.DEBUG_CONDITIONAL_STRING)]
        public void print(String s)
        {
            Console.WriteLine(s);

        }


    }
}
