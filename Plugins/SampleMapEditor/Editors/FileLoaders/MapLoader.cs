﻿using MapStudio.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor.LayoutEditor
{
    public class MapLoader
    {
        //Debugging
        private bool DEBUG_PROBES = false;

        public StageLayoutPlugin Plugin;

        /// <summary>
        /// The stage layout information, such as spawn points and object placement.
        /// </summary>
        public StageDefinition stageDefinition = new StageDefinition();


        // Models

        // N/A


        // Effects

        public static MapLoader Instance = null;


        public MapLoader(StageLayoutPlugin plugin)
        {
            Instance = this;
            Plugin = plugin;
        }


        /// <summary>
        /// Initiates an empty stage with no objects placed (Except for 2 Respawn Points)
        /// </summary>
        public void Init(StageLayoutPlugin plugin) // "bool isSwitch" and "string model_path" are unnecessary here
        {
            // We don't need as much stuff as the MK8 tool because we are only dealing with the stage layout

            stageDefinition.Objs = new List<MuElement>();
            
            // Add a RespawnPos
            MuObj obj0 = new MuObj()
            {
                Id = "obj0",
                //IsLinkDest = false,
                Layer = "Cmn",
                TeamCmp = 0,
                Name = "Obj_RespawnPos",
                Translate = new ByamlVector3F(-200f, 0f, -200f),
                Scale = new ByamlVector3F(1f, 1f, 1f),
                Rotate = new ByamlVector3F(0f, 0f, 0f),
                //Links = new List<LinkInfo> { },
            };
            stageDefinition.Objs.Add(obj0);

            // Add a second RespawnPos
            MuObj obj1 = new MuObj()
            {
                Id = "obj1",
                //IsLinkDest = false,
                Layer = "Cmn",
                TeamCmp = 1,
                Name = "Obj_RespawnPos",
                Translate = new ByamlVector3F(200f, 0f, 200f),
                Scale = new ByamlVector3F(1f, 1f, 1f),
                Rotate = new ByamlVector3F(0f, 0f, 0f),
                //Links = new List<LinkInfo> { },
            };
            stageDefinition.Objs.Add(obj1);


        }

        public void Load(StageLayoutPlugin plugin, string folder, string byamlFile, string workingDir)
        {
            Instance = this;
            Plugin = plugin;

            Console.WriteLine("~ Called MapLoader.Load(....); ~");
            Console.WriteLine("Loading file!!!");

            // Load Stage Layout File
            //LoadStageLayoutFromBYAML(byaml) // ~Example
            LoadStageLayoutFile(byamlFile);


        }




        private void LoadStageLayoutFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            ProcessLoading.Instance.Update(15, 100, "Loading Stage Layout Byml");

            stageDefinition = new StageDefinition(filePath);

            //
        }


    }
}
