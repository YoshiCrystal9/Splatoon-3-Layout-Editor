using Syroot.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class SplActor : MuObj
    {
        [ByamlMember] public string CalcPriority { get; set; }
        [ByamlMember] public string Category { get; set; }
        [ByamlMember] public string Fmdb { get; set; }
        [ByamlMember] public int InstanceHeapSize { get; set; }
        [ByamlMember] public bool IsCalcNodePushBack { get; set; }
        [ByamlMember] public bool IsFarActor { get; set; }
        [ByamlMember] public bool IsNotTurnToActor { get; set; }
        [ByamlMember] public Vector3F ModelAabbMax { get; set; }
        [ByamlMember] public Vector3F ModelAabbMin { get; set; }
        [ByamlMember] public string __RowId { get; set; }

    }
}
