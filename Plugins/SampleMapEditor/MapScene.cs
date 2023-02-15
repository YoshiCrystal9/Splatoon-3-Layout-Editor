using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFrameworkEngine;
using OpenTK;
using Toolbox.Core.ViewModels;
using MapStudio.UI;

namespace SampleMapEditor
{
    internal class MapScene
    {
        public void Setup(EditorLoader loader)
        {
            //Prepare a collision caster for snapping objects onto
            SetupSceneCollision();
            //Add some objects to the scene
            SetupObjects(loader);
        }

        /// <summary>
        /// Adds objects to the scene.
        /// </summary>
        private void SetupObjects(EditorLoader loader)
        {
            NodeBase objFolder = new NodeBase("Objs");
            //objFolder.HasCheckBox = true;
            loader.Root.AddChild(objFolder);
            NodeBase railsFolder = new NodeBase("Rails");
            //railsFolder.HasCheckBox = true;
            loader.Root.AddChild(railsFolder);

            bool HasModel(string mpath, out SARC s)
            {
                s = new SARC();
                s.Load(new MemoryStream(YAZ0.Decompress(mpath)));
                foreach (var f in s.files) if (f.FileName == "output.bfres") return true;
                return false;
            }

            foreach (var mapObj in loader.MapObjList)
            {

                string modelPath = loader.GetModelPathFromObject(mapObj);

                if (File.Exists(modelPath) && HasModel(modelPath, out SARC s))
                {
                    BfresRender o = new BfresRender(s.files.Find(f => f.FileName == "output.bfres").FileData, modelPath);

                    o.Models.ForEach(model =>
                    {
                        bool state = true;
                        if (model.Name != loader.GetActorFromObj(mapObj).FmdbName)
                        {
                            state = false;
                            if (model.Name.StartsWith("Fld_") &&
                                model.Name.EndsWith("_DV")) //!(model.Name.EndsWith("_Map") || model.Name.EndsWith("_drcmap"))))
                            {
                                state = true;
                            }
                        }

                        model.IsVisible = state;
                        if (!state)
                        {
                            Console.WriteLine($"Hiding model: {model.Name}");
                        }
                    });

                    /*  UNUSED ON SPLATOON 3
                     * string fmdbname = loader.GetActorFromObj(mapObj).FmdbName;
                    if (fmdbname.StartsWith("Fld_Deli_"))
                    {
                        Console.WriteLine("Loading DeliTextures!");
                        var deliTex_sarc = new SARC();
                        string dtpath = EditorLoader.GetContentPath("Model/DeliTextures.Nin_NX_NVN.szs");
                        s.Load(new MemoryStream(YAZ0.Decompress(dtpath)));
                        BfresRender dt_bfres = new BfresRender(s.files.Find(f => f.FileName == "output.bfres").FileData, dtpath);
                        for (int i = 0; i < dt_bfres.Textures.Count; i++)
                        {
                            o.Textures.Add(dt_bfres.Textures.Keys.ElementAt(i), dt_bfres.Textures.Values.ElementAt(i));
                        }
                    }*/

                    objFolder.AddChild(o.UINode);
                    o.UINode.Header = mapObj["Name"];
                    o.UINode.Icon = IconManager.MESH_ICON.ToString();
                    o.Transform.Position = EditorLoader.GetObjPos(mapObj);
                    o.Transform.Scale = EditorLoader.GetObjScale(mapObj);
                    o.Transform.RotationEulerDegrees = EditorLoader.GetObjRotation(mapObj);
                    o.Transform.UpdateMatrix(true);
                    loader.AddRender(o);
                }
                else
                {
                    TransformableObject o = new TransformableObject(objFolder);
                    //CustomBoundingBoxRender o = new CustomBoundingBoxRender(objFolder);
                    o.UINode.Header = mapObj["Name"];
                    o.UINode.Icon = IconManager.MESH_ICON.ToString();
                    o.Transform.Position = EditorLoader.GetObjPos(mapObj);
                    o.Transform.Scale = EditorLoader.GetObjScale(mapObj);
                    o.Transform.RotationEulerDegrees = EditorLoader.GetObjRotation(mapObj);
                    //o.Color = new Vector4(0.5F, 0.5F, 0.5F, 0.5F);
                    o.Transform.UpdateMatrix(true);
                }
            }

            //A folder to represent in the outliner UI
            NodeBase folder = new NodeBase("Actors");
            //Allow toggling visibility for the folder
            folder.HasCheckBox = true;
            //Add it to the root of our loader
            //It is important you use "AddChild" so the parent is applied
            loader.Root.AddChild(folder);
            //Icons can be obtained from the icon manager constants
            //These also are all from font awesome and can be used directly
            folder.Icon = IconManager.MODEL_ICON.ToString();

            //These are default transform cubes
            //You give it the folder you want to parent in the tree or make it null to not be present.
            TransformableObject obj = new TransformableObject(folder);
            //Name
            obj.UINode.Header = "Object1";
            obj.UINode.Icon = IconManager.MESH_ICON.ToString();
            //Give it a transform in the scene
            obj.Transform.Position = new Vector3(0, 10, 0);
            obj.Transform.Scale = new Vector3(1, 1, 1);
            obj.Transform.RotationEulerDegrees = new Vector3(0, 0, 0);
            //You need to force update it. This is not updated per frame to save on performance
            obj.Transform.UpdateMatrix(true);

            //Lastly add your object to the scene
            loader.AddRender(obj);

            //Custom renderer
            CustomRender renderer = new CustomRender(folder);
            renderer.UINode.Icon = IconManager.MESH_ICON.ToString();
            renderer.UINode.Header = "Sphere";
            renderer.Transform.Position = new Vector3(-100, 0, 0);
            renderer.Transform.Scale = new Vector3(2.5f);
            renderer.Transform.UpdateMatrix(true);
            loader.AddRender(renderer);
        }

        /// <summary>
        /// Creates a big plane which you can drop objects onto.
        /// </summary>
        private void SetupSceneCollision()
        {
            var context = GLContext.ActiveContext;

            float size = 2000;
            float height = 0;

            //Make a big flat plane for placing spaces on.
            context.CollisionCaster.Clear();
            context.CollisionCaster.AddTri(
                new Vector3(-size, height, size),
                new Vector3(0, height, -(size * 2)),
                new Vector3(size * 2, height, 0));
            context.CollisionCaster.AddTri(
                new Vector3(-size, height, -size),
                new Vector3(size * 2, height, 0),
                new Vector3(size * 2, height, size * 2));
            context.CollisionCaster.UpdateCache();
        }
    }
}
