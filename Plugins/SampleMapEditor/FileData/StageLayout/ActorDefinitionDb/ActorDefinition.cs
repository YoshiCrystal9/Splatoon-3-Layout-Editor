using CafeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class ActorDefinition
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        [ByamlMember]
        [BindGUI("CalcPriority")]
        public string CalcPriority
        {
            get => calcpriority;
            set => SetField(ref calcpriority, value);
        }
        private string calcpriority;


        [ByamlMember]
        [BindGUI("Category")]
        public string Category
        {
            get => category;
            set => SetField(ref category, value);
        }
        private string category;


        [ByamlMember]
        [BindGUI("ClassName")]
        public string ClassName
        {
            get => classname;
            set => SetField(ref classname, value);
        }
        private string classname;


        [ByamlMember]
        [BindGUI("Fmdb")]
        public string Fmdb
        {
            get => fmdb;
            set => SetField(ref fmdb, value);
        }
        private string fmdb;


        [ByamlMember]
        [BindGUI("InstanceHeapSize")]
        public int InstanceHeapSize
        {
            get => instanceheapsize;
            set => SetField(ref instanceheapsize, value);
        }
        private int instanceheapsize;


        [ByamlMember]
        [BindGUI("IsCalcNodePushBack")]
        public bool IsCalcNodePushBack
        {
            get => issalcnodepushback;
            set => SetField(ref issalcnodepushback, value);
        }
        private bool issalcnodepushback;


        [ByamlMember]
        [BindGUI("IsFarActor")]
        public bool IsFarActor
        {
            get => isfaractor;
            set => SetField(ref isfaractor, value);
        }
        private bool isfaractor;


        [ByamlMember]
        [BindGUI("IsNotTurnToActor")]
        public bool IsNotTurnToActor
        {
            get => isnotturntoactor;
            set => SetField(ref isnotturntoactor, value);
        }
        private bool isnotturntoactor;

        [ByamlMember]
        [BindGUI("ModelAabbMax")]
        public ByamlVector3F ModelAabbMax
        {
            get => modelaabbmax;
            set => SetField(ref modelaabbmax, value);
        }
        private ByamlVector3F modelaabbmax;

        [ByamlMember]
        [BindGUI("ModelAabbMin")]
        public ByamlVector3F ModelAabbMin
        {
            get => modelaabbmin;
            set => SetField(ref modelaabbmin, value);
        }
        private ByamlVector3F modelaabbmin;

        [ByamlMember]
        [BindGUI("__RowId")]
        public string __RowId
        {
            get => __rowid;
            set => SetField(ref __rowid, value);
        }
        private string __rowid;



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
