using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class MuObj : MuElement, IByamlSerializable, IStageReferencable
    {
        [ByamlMember]
        [BindGUI("TeamCmp", Category = "Object Properties")]
        public string TeamCmp { get; set; }


        public MuObj() : base()
        {
            TeamCmp = "{Team: Alpha}";
        }

        // Copying Contstructor
        public MuObj(MuObj other) : base(other)
        {
            TeamCmp = other.TeamCmp;
        }

        public override MuObj Clone()
        {
            //MuObj cloned = (MuObj)base.Clone();
            //cloned.Team = this.Team;
            //return cloned;

            return new MuObj(this);
        }



        public override void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            base.DeserializeByaml(dictionary);
        }

        public override void SerializeByaml(IDictionary<string, object> dictionary)
        {
            base.SerializeByaml(dictionary);
        }

        public override void DeserializeReferences(StageDefinition stageDefinition)
        {
            base.DeserializeReferences(stageDefinition);
        }

        public override void SerializeReferences(StageDefinition stageDefinition)
        {
            base.SerializeReferences(stageDefinition);
        }
    }
}
