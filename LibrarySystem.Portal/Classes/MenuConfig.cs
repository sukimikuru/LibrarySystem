using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.IO;
using LibrarySystem.Entities;


namespace LibrarySystem.Portal.Classes
{
    /// <summary>
    /// 菜单配置类，假如以后有角色或权限管理，也放里面
    /// </summary>
    public class MenuConfig
    {
        #region 单例定义
        private static object _locker = new object();
        private static MenuConfig _current = null;
        public static MenuConfig Current
        {
            get
            {
                lock (_locker)
                {
                    if (_current == null)
                    {
                        _current = new MenuConfig();
                    }
                    return _current;
                }
            }
        }
        #endregion

        private Dictionary<MenuEntity, List<MenuEntity>> _menuDict = new Dictionary<MenuEntity, List<MenuEntity>>();

        private MenuConfig()
        {
            InitFromXml();
        }

        /// <summary>
        /// 读配置文件
        /// </summary>
        private void InitFromXml()
        {
            XmlDocument xdoc = new XmlDocument();
            string configPath = ConfigurationManager.AppSettings["MenuConfigPath"];
            if (!string.IsNullOrEmpty(configPath))
            {
                if (configPath.StartsWith("~/") || configPath.StartsWith("/"))
                {
                    configPath = HttpContext.Current.Request.MapPath(configPath);
                }
                else if (configPath.StartsWith("\\"))
                {
                    configPath = AppDomain.CurrentDomain.BaseDirectory + configPath.Substring(1);
                }
            }
            else
            {
                configPath = AppDomain.CurrentDomain.BaseDirectory + "menu.config";
            }
            if (!File.Exists(configPath))
            {
                throw new Exception("MenuConfig的配置文件不存在或配置路径错误.");
            }
            xdoc.Load(configPath);

            foreach (XmlElement node in xdoc.SelectNodes("/root/mainMenu"))
            {
                //主菜单
                MenuEntity mainMenu = new MenuEntity();
                mainMenu.Key = node.Attributes["key"].Value;
                mainMenu.Title = node.Attributes["title"].Value;
                mainMenu.Icon = node.Attributes["icon"].Value;
                mainMenu.Url = node.Attributes["url"].Value;

                List<MenuEntity> subMenuList = new List<MenuEntity>();

                XmlNodeList subNodeList = node.SelectNodes("subMenu");
                if (subNodeList != null && subNodeList.Count > 0)
                {
                    //子菜单
                    foreach (XmlElement subNode in subNodeList)
                    {
                        MenuEntity subMenu = new MenuEntity();
                        subMenu.Key = subNode.Attributes["key"].Value;
                        subMenu.Title = subNode.Attributes["title"].Value;
                        subMenu.Icon = subNode.Attributes["icon"].Value;
                        subMenu.Url = subNode.Attributes["url"].Value;
                        subMenuList.Add(subMenu);
                    }
                }

                _menuDict.Add(mainMenu, subMenuList);
            }
        }

        public Dictionary<MenuEntity, List<MenuEntity>> getMenuDict()
        {
            return _menuDict;
        }

        public string GetMenuTitleByKey(string key)
        {
            string title = "";
            foreach (MenuEntity MainMenu in _menuDict.Keys)
            {
                if (MainMenu.Key == key)
                    title = MainMenu.Title;
                else
                {
                    foreach (MenuEntity subMenu in _menuDict[MainMenu])
                    {
                        if (subMenu.Key == key)
                            title = subMenu.Title;
                    }
                }
            }
            return title;
        }
    }
}