using System;
using System.IO;
using Toolbox.Core;
using MapStudio.UI;
using OpenTK;
using GLFrameworkEngine;
using CafeLibrary;
using Toolbox.Core.IO;
using OpenTK.Input;
using ByamlExt.Byaml;
using System.Collections.Generic;
using CafeLibrary.Rendering;
using Syroot.Maths;
using Vector3 = OpenTK.Vector3;
using Vector2 = OpenTK.Vector2;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents a class used for loading files into the editor.
    /// IFileFormat determines what files to use. FileEditor is used to store all the editor information.
    /// </summary>
    public class EditorLoader : FileEditor, IFileFormat
    {
        /// <summary>
        /// The description of the file extension of the plugin.
        /// </summary>
        public string[] Description => new string[] { "Map Data" };

        /// <summary>
        /// The extension of the plugin. This should match whatever file you plan to open.
        /// </summary>
        public string[] Extension => new string[] { "*.byml" };

        /// <summary>
        /// Determines if the plugin can save or not.
        /// </summary>
        public bool CanSave { get; set; } = true;

        /// <summary>
        /// File info of the loaded file format.
        /// </summary>
        public File_Info FileInfo { get; set; }

        /// <summary>
        /// Determines when to use the map editor from a given file.
        /// You can check from file extension or check the data inside the file stream.
        /// The file stream is always decompressed if the given file has a supported ICompressionFormat like Yaz0.
        /// </summary>
        public bool Identify(File_Info fileInfo, Stream stream)
        {
            return fileInfo.Extension == ".byml";
        }


        public static string GetContentPath(string relativePath)
        {
            if (File.Exists($"{PluginConfig.S2ModPath}/{relativePath}")) return $"{PluginConfig.S2ModPath}/{relativePath}";
            if (File.Exists($"{PluginConfig.S2AocPath}/{relativePath}")) return $"{PluginConfig.S2AocPath}/{relativePath}";
            if (File.Exists($"{PluginConfig.S2GamePath}/{relativePath}")) return $"{PluginConfig.S2GamePath}/{relativePath}";
            return relativePath;


            /*//Update first then base package.
            //if (File.Exists($"{ModOutputPath}\\{relativePath}")) return $"{ModOutputPath}\\{relativePath}";
            if (File.Exists($"{UpdatePath}\\{relativePath}")) return $"{UpdatePath}\\{relativePath}";
            if (File.Exists($"{GamePath}\\{relativePath}")) return $"{GamePath}\\{relativePath}";

            //4 individual DLCs. Each directory is divided by content and permissive info.
            if (File.Exists($"{AOCPath}\\0013\\{relativePath}")) return $"{AOCPath}\\0013\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0015\\{relativePath}")) return $"{AOCPath}\\0015\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0017\\{relativePath}")) return $"{AOCPath}\\0017\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0019\\{relativePath}")) return $"{AOCPath}\\0019\\{relativePath}";

            return relativePath;*/
        }


        public class Actor
        {
            public string CalcPriority { get; set; }
            public string Category { get; set; }
            public string ClassName { get; set; }
            public string Fmdb { get; set; }
            public int InstanceHeapSize { get; set; }
            public bool IsCalcNodePushBack { get; set; }
            public bool IsFarActor { get; set; }
            public bool IsNotTurnToActor { get; set; }
            public Vector3 ModelAabbMax { get; set; }
            public Vector3 ModelAabbMin { get; set; }
            public string __RowId { get; set; }


            public Actor(dynamic actor)
            {
                CalcPriority = actor["CalcPriority"];
                Category = actor["Category"];
                ClassName = actor["ClassName"];
                Fmdb = actor["Fmdb"];
                InstanceHeapSize = actor["InstanceHeapSize"];
                IsCalcNodePushBack = actor["IsCalcNodePushBack"];
                IsFarActor = actor["IsFarActor"];
                //ModelAabbMax = actor["ModelAabbMax"];
                //ModelAabbMin = actor["ModelAabbMin"];
                __RowId = actor["__RowId"];
            }

            public Actor()
            {
                CalcPriority = "After";
                Category = "Obj";
                ClassName = "SplActor";
                Fmdb = "Work/Model/Weapon/Wsp_Shachihoko/output/Wsp_Shachihoko.fmdb";
                InstanceHeapSize = 213176;
                IsCalcNodePushBack = true;
                IsFarActor = false;
                //ModelAabbMax = (0.40589f, 1.57011f, 0.83881f);
                //ModelAabbMin = (-0.40585f, -0.00496f, -1.17535f);
                __RowId = "Gachihoko";
            }
        }

        public List<Actor> Actors { get; set; } = new List<Actor>();


        public List<dynamic> MapObjList { get; set; } = new List<dynamic>();

        public static Vector3 GetObjPos(dynamic obj)
        {
            var t = obj["Translate"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }
        public static Vector3 GetObjScale(dynamic obj)
        {
            var t = obj["Scale"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }
        public static Vector3 GetObjRotation(dynamic obj)
        {
            var t = obj["Rotate"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }


        private void ParseActorDb()
        {
            string mushPackPath = GetContentPath("RSDB/ActorInfo.Product.200.rstbl.byml");
            BymlFileData actorDbByml = new BymlFileData();
            actorDbByml = ByamlFile.LoadN(mushPackPath, true);

            if (actorDbByml == null) return;

            foreach (var node in actorDbByml.RootNode)
            {
                Actors.Add(new Actor(node));
                //Console.WriteLine(node["Name"]);
            }

            Console.WriteLine("Finished loading ActorDb.");
        }


        //
        public string GetModelPathFromName(string name)
        {
            Actor actor = Actors.Find(x => x.__RowId == name);
            if (actor == null) return null;
            if (actor.Fmdb == "") return null;
            return GetContentPath($"Model/{actor.__RowId}.bfres.zs");
        }

        public string GetModelPathFromObject(dynamic obj)
        {
            return GetModelPathFromName(obj["Name"]);
        }

        public Actor GetActorFromObj(dynamic obj)
        {
            string ucName = obj["Name"];
            return GetActorFromName(ucName);
        }

        public Actor GetActorFromName(string name)
        {
            Actor actor = Actors.Find(x => x.__RowId == name);
            return actor;
        }

        public BFRES model1 = new BFRES();


        /// <summary>
        /// Loads the given file data from a stream.
        /// </summary>
        public void Load(Stream stream)
        {
            Console.WriteLine(PluginConfig.S2GamePath);

            ParseActorDb();

            //SARC arc = (SARC)STFileLoader.OpenFileFormat(stream, FileInfo.FilePath);
            //SARC arc = new SARC();
            //arc.Load(stream);
            BymlFileData lytByml = ByamlFile.LoadN(stream, true);

            MapObjList.Clear();

            // Print out the name of each object that will be loaded
            foreach (var obj in lytByml.RootNode["Actors"])
            {
                Console.WriteLine(obj["Name"]);
                //float x = obj["Translate"]["X"];
                //float y = obj["Translate"]["Y"];
                //float z = obj["Translate"]["Z"];
                //Console.WriteLine($"  Pos: {x}, {y}, {z}");
                MapObjList.Add(obj);
            }

            Console.WriteLine("Actors list:");
            foreach (var obj in MapObjList)
            {
                Console.WriteLine($"  Actors name: {obj["Name"]}");
            }

            // (TESTING) Load the first object in the list
            int i = 0;
            string modelPath = GetModelPathFromObject(MapObjList[i]);
            Console.WriteLine($"Model Path: {modelPath}");


            /*SARC mdlFile = new SARC();
            byte[] tmp = YAZ0.Decompress(modelPath);
            using (var ms = new MemoryStream(tmp))
            {
                mdlFile.Load(ms);
            }*/

            //SARC mdlFile = new SARC();
            //mdlFile.Load(new MemoryStream(YAZ0.Decompress(modelPath)));
            //BfresRender r = new BfresRender(mdlFile.files.Find(x => x.FileName == "output.bfres").FileData, modelPath);

            
            //Console.WriteLine(mdlFile.files[0].FileName);
            //mdlFile.files.ForEach(f => Console.WriteLine(f.FileName));
            //model1 = (BFRES)mdlFile.files.Find(x => x.FileName == "output.bfres").OpenFile();


            //SARC modelFile = (SARC)STFileLoader.OpenFileFormat(modelPath);
            //modelFile.files.ForEach(f => Console.WriteLine(f));
            //BFRES res = (BFRES)modelFile.files[0].OpenFile();

            //model1 = res;

            //Console.WriteLine("!!!Break!!!");
            // Check if file is a Stage Layout Byml
            //BFRES a = new BFRES();
            //SARC arc = new SarcData(stream);

            //For this example I will show loading 3D objects into the scene
            MapScene scene = new MapScene();
            scene.Setup(this);
        }

        /// <summary>
        /// Saves the given file data to a stream.
        /// </summary>
        public void Save(Stream stream)
        {

        }

        //Extra overrides for FileEditor you can use for custom UI

        /// <summary>
        /// Draws the viewport menu bar usable for custom tools.
        /// </summary>
        public override void DrawViewportMenuBar()
        {

        }

        /// <summary>
        /// When an asset item from the asset windows gets dropped into the editor.
        /// You can configure your own asset category from the asset window and make custom asset items to drop into.
        /// </summary>
        public override void AssetViewportDrop(AssetItem item, Vector2 screenPosition)
        {
            //viewport context
            var context = GLContext.ActiveContext;

            //Screen coords can be converted into 3D space
            //By default it will spawn in the mouse position at a distance
            Vector3 position = context.ScreenToWorld(screenPosition.X, screenPosition.Y, 100);
            //Collision dropping can be used to drop these assets to the ground from CollisionCaster
            if (context.EnableDropToCollision)
            {
                Quaternion rot = Quaternion.Identity;
                CollisionDetection.SetObjectToCollision(context, context.CollisionCaster, screenPosition, ref position, ref rot);
            }
        }

        /// <summary>
        /// Checks for dropped files to use for the editor.
        /// If the value is true, the file will not be loaded as an editor if supported.
        /// </summary>
        public override bool OnFileDrop(string filePath)
        {
            return false;
        }
    }
}
