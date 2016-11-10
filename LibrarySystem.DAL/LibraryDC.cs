using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using LibrarySystem.Entities;
using System.Data.SQLite;
using System.Reflection;
using System.Linq.Expressions;

namespace LibrarySystem.DAL
{
    public partial class LibraryDC : DataContext
    {
        #region Extensibility Method Declarations
        partial void OnCreated();
        #endregion

        public LibraryDC(string connectionString) :
            base(connectionString)
        {
            this.OnCreated();
        }

        public LibraryDC(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }

        public LibraryDC(IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }



        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> PageList<T>(int pageIndex, int pageSize, Func<T, bool> wh, params string[] ob) where T : class
        {
            List<T> result = new List<T>();
            try
            {
                List<T> AllList = this.GetTable<T>().ToList();
                //根据wh条件筛选
                if (wh != null)
                {
                    AllList = AllList.Where(wh).ToList();
                }
                //根据ob排序
                if (ob != null)
                {
                    foreach (string obItem in ob)
                    {
                        string obKey = obItem.Split(',')[0];
                        string direction = obItem.Split(',')[1];
                        switch (direction)
                        {
                            case "asc":
                                AllList = AllList.OrderBy(p => GetPropertyValue(p, obKey)).ToList();
                                break;
                            case "desc":
                                AllList = AllList.OrderByDescending(p => GetPropertyValue(p, obKey)).ToList();
                                break;
                        }

                    }
                }
                //取指定页码数据
                result = AllList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {

            }
            return result;

        }

        /// <summary>
        /// 取所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> AllList<T>() where T : class
        {
            return this.GetTable<T>().ToList();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addItem"></param>
        /// <returns></returns>
        public bool InsertEntity<T>(T addItem) where T : class
        {
            bool result = false;
            try
            {
                this.GetTable<T>().InsertOnSubmit(addItem);

                foreach (System.Data.Linq.ObjectChangeConflict occ in this.ChangeConflicts)
                {
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }
                this.SubmitChanges();

                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delItem"></param>
        /// <returns></returns>
        public bool DelEntity<T>(string row_key_str) where T : class
        {
            bool result = false;
            try
            {
                List<string> row_key_list = row_key_str.Split(',').ToList();

                //this.GetTable<T>().ToList() 必须先tolist，否则异常“GetProperty不能转换为sql”
                List<T> tables = this.GetTable<T>().ToList().Where(p => row_key_list.Contains(GetPropertyValue(p, "RowKey").ToString())).ToList();
                if (tables.Count() > 0)
                {
                    foreach (T delItem in tables)
                    {
                        this.GetTable<T>().DeleteOnSubmit(delItem);
                    }
                    this.SubmitChanges();
                    result = true;
                }


            }
            catch (Exception ex)
            { }
            return result;
        }

        /// <summary>
        /// 取单个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row_key"></param>
        /// <returns></returns>
        public T Single<T>(long row_key) where T : class
        {
            List<T> list = this.GetTable<T>().ToList().Where(p => GetPropertyValue(p, "RowKey").ToString() == row_key.ToString()).ToList();
            if (list.Count > 0)
                return list[0];
            else return null;
        }

        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetPropertyValue(object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }



    }

    public partial class LibraryDC
    {

        public LibraryDC(IDbConnection connection) :
            base(connection)
        {
            this.OnCreated();
        }
    }


}
