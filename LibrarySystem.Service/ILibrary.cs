using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using LibrarySystem.Entities;

namespace LibrarySystem.Service
{
    /// <summary>
    /// 和用户信息，登陆相关的服务
    /// </summary>
    [ServiceContract]
    public interface ILibrary
    {
        /// <summary>
        /// 登陆,成功返回用户key，否则返回0
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [OperationContract]
        bool LoginSys(string login_name, string pwd, out string msg, out UserEntity userInfo);

        /// <summary>
        /// 主动退出系统
        /// </summary>
        /// <param name="row_key"></param>
        /// <returns></returns>
        [OperationContract]
        bool LogOutSystem(long row_key);

        [OperationContract]
        int add(int x, int y);


        [OperationContract]
        List<ResEntity> ResPagerList(int pageIndex, int pageSize, Func<ResEntity, bool> wh, params string[] ob);
       

    }
}
