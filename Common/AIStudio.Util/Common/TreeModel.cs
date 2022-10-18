using AIStudio.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 树模型（可以作为父类）
    /// </summary>
    public class TreeModel : SelectOption, IIdObject
    {
        /// <summary>
        /// 唯一标识Id
        /// </summary>
        public string Id { get; set; }

        ///// <summary>
        ///// 数据值
        ///// </summary>
        //public string Value { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 节点深度
        /// </summary>
        public int? Level { get; set; } = 1;

        ///// <summary>
        ///// 显示的内容
        ///// </summary>
        //public string Text { get; set; }

        /// <summary>
        /// 孩子节点
        /// </summary>
        //public List<TreeModel> children { get { return Children; } }

        /// <summary>
        /// 孩子节点
        /// </summary>
        public List<TreeModel> Children { get; set; }


        public bool IsExpand { get; set; }
    }

    public class TreeModel<T> : TreeModel
    {
        /// <summary>
        /// 孩子节点
        /// </summary>
        public new List<T> Children { get; set; } = new List<T>();

        /// <summary>
        /// 孩子节点
        /// </summary>
        //public new List<T> children { get { return Children; } }

    }

    public class TreeHelper
    {
        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="trees"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TreeModel GetTreeModel(IEnumerable<TreeModel> trees, string id)
        {
            TreeModel treemodel = null;
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

        /// <summary>
        /// 获取所有树的节点
        /// </summary>
        /// <param name="trees"></param>
        /// <returns></returns>

        public static List<TreeModel> GetTreeToList(IEnumerable<TreeModel> trees)
        {
            List<TreeModel> treemodels = new List<TreeModel>();
            if (trees != null)
            {
                foreach (var tree in trees)
                {
                    treemodels.Add(tree);

                    if (tree.Children != null)
                    {
                        treemodels.AddRange(GetTreeToList(tree.Children));
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
        public static List<T> BuildTree<T>(List<T> allNodes) where T : TreeModel, new()
        {
            List<T> resData = new List<T>();
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
        public static List<T> GetChildren<T>(List<T> allNodes, T parentNode, bool includeSelf) where T : TreeModel
        {
            List<T> resList = new List<T>();
            if (includeSelf)
                resList.Add(parentNode);
            _getChildren(allNodes, parentNode, resList);

            return resList;

            void _getChildren(List<T> _allNodes, T _parentNode, List<T> _resNodes)
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
        private static List<TreeModel> _GetChildren<T>(List<T> nodes, T parentNode) where T : TreeModel, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            List<TreeModel> resData = new List<TreeModel>();
            var children = nodes.Where(x => x.ParentId == parentNode.Id).ToList();
            children.ForEach(aChildren =>
            {
                T newNode = new T();
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
        private static bool HaveChildren<T>(List<T> nodes, string nodeId) where T : TreeModel, new()
        {
            return nodes.Exists(x => x.ParentId == nodeId);
        }

        #endregion

        #region 泛型类使用

        public static List<T> GetTreeToList<T>(IEnumerable<T> trees) where T : TreeModel<T>
        {
            List<T> treemodels = new List<T>();
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

        /// <summary>
        /// 建造树结构，继承TreeModel<T>的模型使用使用
        /// </summary>
        /// <param name="allNodes">所有的节点</param>
        /// <returns></returns>
        public static List<T> BuildGenericsTree<T>(List<T> allNodes) where T : TreeModel<T>, new()
        {
            List<T> resData = new List<T>();
            var rootNodes = allNodes.Where(x => x.ParentId == "0" || x.ParentId.IsNullOrEmpty()).ToList();
            resData = rootNodes;
            resData.ForEach(aRootNode =>
            {
                if (HaveChildren(allNodes, aRootNode.Id))
                    aRootNode.Children = _GetGenericsChildren(allNodes, aRootNode);
            });

            return resData;
        }      

        /// <summary>
        /// 获取所有子节点，继承TreeModel<T>的模型使用使用
        /// </summary>
        /// <typeparam name="T">树模型（TreeModel或继承它的模型）</typeparam>
        /// <param name="nodes">所有节点列表</param>
        /// <param name="parentNode">父节点Id</param>
        /// <returns></returns>
        private static List<T> _GetGenericsChildren<T>(List<T> nodes, T parentNode) where T : TreeModel<T>, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            List<T> resData = new List<T>();
            var children = nodes.Where(x => x.ParentId == parentNode.Id).ToList();
            children.ForEach(aChildren =>
            {
                T newNode = new T();
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
                    newNode.Children = _GetGenericsChildren(nodes, newNode);
            });

            return resData;
        }

        #endregion
    }
}
