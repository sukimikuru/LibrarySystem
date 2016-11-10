using System;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
namespace LibrarySystem.Entities
{
	/// <summary>
	/// 
	/// </summary>
  [Table(Name = "tbl_user")]
  public partial class UserEntity : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
  {

    private long _RowKey;
    /// <summary>
    /// 
    /// </summary>
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


    private long _DepartKey;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_DepartKey", Name = "depart_key", DbType = "bigint", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public long DepartKey 
    { 
	    get { return this._DepartKey; } 
	    set
	    {
       if (((_DepartKey == value)
                            == false))
                {
                    this.OnDepartKeyChanging(value);
                    this.SendPropertyChanging();
                    this._DepartKey = value;
                    this.SendPropertyChanged("DepartKey");
                    this.OnDepartKeyChanged();
                }
	    }
    }
    
       partial void OnDepartKeyChanged();

        partial void OnDepartKeyChanging(long value);


    private String _Role;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_Role", Name = "role", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String Role 
    { 
	    get { return this._Role; } 
	    set
	    {
       if (((_Role == value)
                            == false))
                {
                    this.OnRoleChanging(value);
                    this.SendPropertyChanging();
                    this._Role = value;
                    this.SendPropertyChanged("Role");
                    this.OnRoleChanged();
                }
	    }
    }
    
       partial void OnRoleChanged();

        partial void OnRoleChanging(String value);


    private String _LoginName;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_LoginName", Name = "login_name", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String LoginName 
    { 
	    get { return this._LoginName; } 
	    set
	    {
       if (((_LoginName == value)
                            == false))
                {
                    this.OnLoginNameChanging(value);
                    this.SendPropertyChanging();
                    this._LoginName = value;
                    this.SendPropertyChanged("LoginName");
                    this.OnLoginNameChanged();
                }
	    }
    }
    
       partial void OnLoginNameChanged();

        partial void OnLoginNameChanging(String value);


    private String _Pwd;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_Pwd", Name = "pwd", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String Pwd 
    { 
	    get { return this._Pwd; } 
	    set
	    {
       if (((_Pwd == value)
                            == false))
                {
                    this.OnPwdChanging(value);
                    this.SendPropertyChanging();
                    this._Pwd = value;
                    this.SendPropertyChanged("Pwd");
                    this.OnPwdChanged();
                }
	    }
    }
    
       partial void OnPwdChanged();

        partial void OnPwdChanging(String value);


    private String _Sex;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_Sex", Name = "sex", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String Sex 
    { 
	    get { return this._Sex; } 
	    set
	    {
       if (((_Sex == value)
                            == false))
                {
                    this.OnSexChanging(value);
                    this.SendPropertyChanging();
                    this._Sex = value;
                    this.SendPropertyChanged("Sex");
                    this.OnSexChanged();
                }
	    }
    }
    
       partial void OnSexChanged();

        partial void OnSexChanging(String value);


    private String _NickName;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_NickName", Name = "nick_name", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String NickName 
    { 
	    get { return this._NickName; } 
	    set
	    {
       if (((_NickName == value)
                            == false))
                {
                    this.OnNickNameChanging(value);
                    this.SendPropertyChanging();
                    this._NickName = value;
                    this.SendPropertyChanged("NickName");
                    this.OnNickNameChanged();
                }
	    }
    }
    
       partial void OnNickNameChanged();

        partial void OnNickNameChanging(String value);


    private String _HeadImg;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_HeadImg", Name = "head_img", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String HeadImg 
    { 
	    get { return this._HeadImg; } 
	    set
	    {
       if (((_HeadImg == value)
                            == false))
                {
                    this.OnHeadImgChanging(value);
                    this.SendPropertyChanging();
                    this._HeadImg = value;
                    this.SendPropertyChanged("HeadImg");
                    this.OnHeadImgChanged();
                }
	    }
    }
    
       partial void OnHeadImgChanged();

        partial void OnHeadImgChanging(String value);


    private String _Phone;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_Phone", Name = "phone", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String Phone 
    { 
	    get { return this._Phone; } 
	    set
	    {
       if (((_Phone == value)
                            == false))
                {
                    this.OnPhoneChanging(value);
                    this.SendPropertyChanging();
                    this._Phone = value;
                    this.SendPropertyChanged("Phone");
                    this.OnPhoneChanged();
                }
	    }
    }
    
       partial void OnPhoneChanged();

        partial void OnPhoneChanging(String value);


    private String _Email;
    /// <summary>
    /// 
    /// </summary>
    [Column(Storage = "_Email", Name = "email", DbType = "nvarchar", AutoSync = AutoSync.Never, UpdateCheck = UpdateCheck.Never)]
    [DebuggerNonUserCode()]
    public String Email 
    { 
	    get { return this._Email; } 
	    set
	    {
       if (((_Email == value)
                            == false))
                {
                    this.OnEmailChanging(value);
                    this.SendPropertyChanging();
                    this._Email = value;
                    this.SendPropertyChanged("Email");
                    this.OnEmailChanged();
                }
	    }
    }
    
       partial void OnEmailChanged();

        partial void OnEmailChanging(String value);


    private long _Creator;
    /// <summary>
    /// 
    /// </summary>
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
 
  public UserEntity()
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
