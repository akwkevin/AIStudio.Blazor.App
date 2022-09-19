using AIStudio.Util.Common;
using System;
using System.Collections.Generic;

namespace AIStudio.Entity.DTO.Base_Manage
{
    public class Base_ActionTree : TreeModel<Base_ActionTree>
    {
        public ActionType Type { get; set; }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    if (!string.IsNullOrEmpty(_url))
                    {
                        string[] urls = _url.Split('/', StringSplitOptions.RemoveEmptyEntries);
                        if (urls.Length >= 2)
                        {
                            PageUrl = urls[1];
                        }
                    }
                }
            }
        }
        public string PageUrl { get; set; }
        public bool NeedAction { get; set; }
        public string ValueInfo { get; set; }
        public string TypeText { get => Type.ToString(); }
        public string NeedActionText { get => NeedAction ? "是" : "否"; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public List<string> PermissionValues { get; set; }

        public string Name { get { return Text; } }
        public string Key { get { return Name + Path; } }
        public string Path { get { return Url; } }
    }
}
