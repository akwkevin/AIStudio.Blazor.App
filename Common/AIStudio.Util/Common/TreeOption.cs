using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 暂时未使用，先放在这里
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeOption<T> : SelectOption, IIdObject
    {

        /// <summary>
        /// 唯一标识Id
        /// </summary>
        public string Id { get; set; }

        ///// <summary>
        ///// 数据值
        ///// </summary>
        //public string Value { get; set; }

        ///// <summary>
        ///// 显示的内容
        ///// </summary>
        //public string Text { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 节点深度
        /// </summary>
        public int? Level { get; set; } = 1;

        /// <summary>
        /// 实体数据
        /// </summary>
        public T Entity { get; set; }


        public bool IsExpand { get; set; }

        /// <summary>
        /// 孩子节点
        /// </summary>
        public List<TreeOption<T>> Children { get; set; }
    }

    public class TreeOption : TreeOption<object>
    {

    }

    public class TreeOptionHelper
    {
        public static TreeOption<T> GetTreeModel<T>(IEnumerable<TreeOption<T>> trees, string id)
        {
            TreeOption<T> treemodel = null;
            if (trees != null)
            {
                foreach (var tree in trees)
                {
                    if (tree.Id == id)
                    {
                        treemodel = tree;
                        break;
                    }
                    else if (tree.Children != null)
                    {
                        treemodel = GetTreeModel(tree.Children, id);
                    }
                }
            }
            return treemodel;
        }

        public static List<TreeOption<T>> GetTreeToList<T>(IEnumerable<TreeOption<T>> trees)
        {
            List<TreeOption<T>> treemodels = new List<TreeOption<T>>();
            if (trees != null)
            {
                foreach (var tree in trees)
                {
                    treemodels.Add(tree);

                    if (tree.Children != null)
                    {
                        treemodels.AddRange(GetTreeToList<T>(tree.Children));
                    }
                }
            }
            return treemodels;
        }

        #region 外部接口

        /// <summary>
        /// 建造树结构
        /// </summary>
        /// <param name="allNodes">所有的节点</param>
        /// <returns></returns>
        public static List<TreeOption<T>> BuildTree<T>(List<TreeOption<T>> allNodes)
        {
            List<TreeOption<T>> resData = new List<TreeOption<T>>();
            var rootNodes = allNodes.Where(x => x.ParentId == "0" || x.ParentId.IsNullOrEmpty()).ToList();
            resData = rootNodes;
            resData.ForEach(aRootNode =>
            {
                if (HaveChildren(allNodes, aRootNode.Id))
                    aRootNode.Children = _GetChildren(allNodes, aRootNode);
            });

            return resData;
        }

        /// <summary>
        /// 获取所有子节点
        /// 注：包括自己
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <param name="allNodes">所有节点</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="includeSelf">是否包括自己</param>
        /// <returns></returns>
        public static List<TreeOption<T>> GetChildren<T>(List<TreeOption<T>> allNodes, TreeOption<T> parentNode, bool includeSelf) 
        {
            List<TreeOption<T>> resList = new List<TreeOption<T>>();
            if (includeSelf)
                resList.Add(parentNode);
            _getChildren(allNodes, parentNode, resList);

            return resList;

            void _getChildren(List<TreeOption<T>> _allNodes, TreeOption<T> _parentNode, List<TreeOption<T>> _resNodes)
            {
                var children = _allNodes.Where(x => x.ParentId == _parentNode.Id).ToList();
                _resNodes.AddRange(children);
                children.ForEach(aChild =>
                {
                    _getChildren(_allNodes, aChild, _resNodes);
                });
            }
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <typeparam name="T">树模型（TreeModel或继承它的模型）</typeparam>
        /// <param name="nodes">所有节点列表</param>
        /// <param name="parentNode">父节点Id</param>
        /// <returns></returns>
        private static List<TreeOption<T>> _GetChildren<T>(List<TreeOption<T>> nodes, TreeOption<T> parentNode)
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            List<TreeOption<T>> resData = new List<TreeOption<T>>();
            var children = nodes.Where(x => x.ParentId == parentNode.Id).ToList();
            children.ForEach(aChildren =>
            {
                TreeOption<T> newNode = new TreeOption<T>();
                resData.Add(newNode);

                //赋值属性
                properties.Where(x => x.CanWrite).ForEach(aProperty =>
                {
                    var value = aProperty.GetValue(aChildren, null);
                    aProperty.SetValue(newNode, value);
                });
                //设置深度
                newNode.Level = parentNode.Level + 1;

                if (HaveChildren(nodes, aChildren.Id))
                    newNode.Children = _GetChildren(nodes, newNode);
            });

            return resData;
        }

        /// <summary>
        /// 判断当前节点是否有子节点
        /// </summary>
        /// <typeparam name="T">树模型</typeparam>
        /// <param name="nodes">所有节点</param>
        /// <param name="nodeId">当前节点Id</param>
        /// <returns></returns>
        private static bool HaveChildren<T>(List<TreeOption<T>> nodes, string nodeId) 
        {
            return nodes.Exists(x => x.ParentId == nodeId);
        }

        #endregion
    }
}
