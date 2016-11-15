using System;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace LibrarySystem.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    [Table(Name = "tbl_department")]
    public partial class DepartmentEntity : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private long _RowKey;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_RowKey", Name = "row_key", DbType = "bigint", AutoSync = AutoSync.Never, IsPrimaryKey = true)]
        [DebuggerNonUserCode()]
        public long RowKey
        {
            get { return this._RowKey; }
            set
            {
                if (((_RowKey == value)
                                     == false))
                {
                    this.OnRowKeyChanging(value);
                    this.SendPropertyChanging();
                    this._RowKey = value;
                    this.SendPropertyChanged("RowKey");
                    this.OnRowKeyChanged();
                }
            }
        }

        partial void OnRowKeyChanged();

        partial void OnRowKeyChanging(long value);


        private long _Parent;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Parent", Name = "parent", DbType = "bigint", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public long Parent
        {
            get { return this._Parent; }
            set
            {
                if (((_Parent == value)
                                     == false))
                {
                    this.OnParentChanging(value);
                    this.SendPropertyChanging();
                    this._Parent = value;
                    this.SendPropertyChanged("Parent");
                    this.OnParentChanged();
                }
            }
        }

        partial void OnParentChanged();

        partial void OnParentChanging(long value);


        private Int32 _Level;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Level", Name = "level", DbType = "int", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public Int32 Level
        {
            get { return this._Level; }
            set
            {
                if (((_Level == value)
                                     == false))
                {
                    this.OnLevelChanging(value);
                    this.SendPropertyChanging();
                    this._Level = value;
                    this.SendPropertyChanged("Level");
                    this.OnLevelChanged();
                }
            }
        }

        partial void OnLevelChanged();

        partial void OnLevelChanging(Int32 value);


        private String _Name;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Name", Name = "name", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public String Name
        {
            get { return this._Name; }
            set
            {
                if (((_Name == value)
                                     == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        partial void OnNameChanged();

        partial void OnNameChanging(String value);


        private String _Note;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Note", Name = "note", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public String Note
        {
            get { return this._Note; }
            set
            {
                if (((_Note == value)
                                     == false))
                {
                    this.OnNoteChanging(value);
                    this.SendPropertyChanging();
                    this._Note = value;
                    this.SendPropertyChanged("Note");
                    this.OnNoteChanged();
                }
            }
        }

        partial void OnNoteChanged();

        partial void OnNoteChanging(String value);


        private String _Sort;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Sort", Name = "sort", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public String Sort
        {
            get { return this._Sort; }
            set
            {
                if (((_Sort == value)
                                     == false))
                {
                    this.OnSortChanging(value);
                    this.SendPropertyChanging();
                    this._Sort = value;
                    this.SendPropertyChanged("Sort");
                    this.OnSortChanged();
                }
            }
        }

        partial void OnSortChanged();

        partial void OnSortChanging(String value);


        private long _Creator;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Creator", Name = "creator", DbType = "bigint", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public long Creator
        {
            get { return this._Creator; }
            set
            {
                if (((_Creator == value)
                                     == false))
                {
                    this.OnCreatorChanging(value);
                    this.SendPropertyChanging();
                    this._Creator = value;
                    this.SendPropertyChanged("Creator");
                    this.OnCreatorChanged();
                }
            }
        }

        partial void OnCreatorChanged();

        partial void OnCreatorChanging(long value);


        private DateTime _CreateTime;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_CreateTime", Name = "create_time", DbType = "datetime", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public DateTime CreateTime
        {
            get { return this._CreateTime; }
            set
            {
                if (((_CreateTime == value)
                                     == false))
                {
                    this.OnCreateTimeChanging(value);
                    this.SendPropertyChanging();
                    this._CreateTime = value;
                    this.SendPropertyChanged("CreateTime");
                    this.OnCreateTimeChanged();
                }
            }
        }

        partial void OnCreateTimeChanged();

        partial void OnCreateTimeChanging(DateTime value);


        private long _Editor;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Editor", Name = "editor", DbType = "bigint", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public long Editor
        {
            get { return this._Editor; }
            set
            {
                if (((_Editor == value)
                                     == false))
                {
                    this.OnEditorChanging(value);
                    this.SendPropertyChanging();
                    this._Editor = value;
                    this.SendPropertyChanged("Editor");
                    this.OnEditorChanged();
                }
            }
        }

        partial void OnEditorChanged();

        partial void OnEditorChanging(long value);


        private DateTime _UpdateTime;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_UpdateTime", Name = "update_time", DbType = "datetime", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public DateTime UpdateTime
        {
            get { return this._UpdateTime; }
            set
            {
                if (((_UpdateTime == value)
                                     == false))
                {
                    this.OnUpdateTimeChanging(value);
                    this.SendPropertyChanging();
                    this._UpdateTime = value;
                    this.SendPropertyChanged("UpdateTime");
                    this.OnUpdateTimeChanged();
                }
            }
        }

        partial void OnUpdateTimeChanged();

        partial void OnUpdateTimeChanging(DateTime value);


        private Boolean _Status;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Storage = "_Status", Name = "status", DbType = "bit", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
        [DebuggerNonUserCode()]
        public Boolean Status
        {
            get { return this._Status; }
            set
            {
                if (((_Status == value)
                                     == false))
                {
                    this.OnStatusChanging(value);
                    this.SendPropertyChanging();
                    this._Status = value;
                    this.SendPropertyChanged("Status");
                    this.OnStatusChanged();
                }
            }
        }

        partial void OnStatusChanged();

        partial void OnStatusChanging(Boolean value);


        partial void OnCreated();

        public DepartmentEntity()
        {
            this.OnCreated();
        }

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
