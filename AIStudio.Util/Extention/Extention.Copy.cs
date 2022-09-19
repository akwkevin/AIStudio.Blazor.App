using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util
{
	public static partial class Extention
	{
		/// <summary>
		/// 复制对象, 仅复制对象相同属性的值. 浅拷贝.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static object CloneObject(this object source)
		{
			try
			{
				if (source == null) return null;
				Type objType = source.GetType();
				object destObj = objType.Assembly.CreateInstance(objType.FullName);
				PropertyInfo[] propsSource = objType.GetProperties();
				foreach (PropertyInfo infoSource in propsSource)
				{
					object value = infoSource.GetValue(source, null);
					infoSource.SetValue(destObj, value, null);
				}
				return destObj;
			}
			catch { return null; }
		}

		/// <summary>
		/// 复制对象, 仅复制对象相同属性的值. 浅拷贝.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T CloneObject<T>(this T source) where T : new()
		{
			try
			{
				if (source == null) return default(T);
				Type objType = source.GetType();
				T destObj = Activator.CreateInstance<T>();
				PropertyInfo[] propsSource = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo infoSource in propsSource)
				{
					object value = infoSource.GetValue(source, null);
					SetPropertyValue(destObj, infoSource, value);
				}
				return destObj;
			}
			catch { return default(T); }
		}

		/// <summary>
		/// 复制对象相同属性的值.
		/// </summary>
		public static void CopyProperties(this object source, object destObj)
		{
			try
			{
				if (source == null || destObj == null) return;

				PropertyInfo[] propsDest = destObj.GetType().GetProperties();
				foreach (PropertyInfo infoDest in propsDest)
				{
					object value = GetValueOfObject(source, infoDest.Name);
					//if (CanShallowCopyProperty(value))
					SetPropertyValue(destObj, infoDest, value);
				}
			}
			catch { }
		}

		/// <summary>
		/// Object to Object. 将一个对象转换为指定类型的对象.
		/// 注意: destination内的Property必需在source内存在.
		/// </summary>
		public static object CopyProperties(this object source, Type destination)
		{
			try
			{
				if (source == null) return null;
				object destObj = destination.Assembly.CreateInstance(destination.FullName);
				PropertyInfo[] propsDest = destObj.GetType().GetProperties();
				foreach (PropertyInfo infoDest in propsDest)
				{
					object value = GetValueOfObject(source, infoDest.Name);
					SetPropertyValue(destObj, infoDest, value);
				}
				return destObj;
			}
			catch { return null; }
		}

		/// <summary>
		/// 给对象的属性赋值
		/// </summary>
		/// <param name="instance">对象实例</param>
		/// <param name="prop">对象实例的属性信息</param>
		/// <param name="value">其他对象属性的值</param>
		public static void SetPropertyValue(object instance, PropertyInfo prop, object value)
		{
			try
			{
				if (prop == null) return;

				if (prop.PropertyType.IsArray)//数组类型,单独处理
				{
					if (value == DBNull.Value)//特殊处理DBNull类型
						prop.SetValue(instance, null, null);
					else
						prop.SetValue(instance, value, null);
				}
				else
				{
					if (value == null || String.IsNullOrWhiteSpace(value.ToString()))//空值
						value = prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null;//值类型
					else
						value = System.ComponentModel.TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(value.ToString());//创建对象

					prop.SetValue(instance, value, null);
				}
			}
			catch (Exception ex) //报错在此跟踪
			{
			}
		}

		/// <summary>
		/// 获取对象指定属性的值
		/// </summary>
		public static object GetValueOfObject(object obj, string property)
		{
			try
			{
				if (obj == null) return null;
				Type type = obj.GetType();
				PropertyInfo[] pinfo = type.GetProperties();
				foreach (PropertyInfo info in pinfo)
				{
					if (info.Name.ToUpper() == property.ToUpper())
						return info.GetValue(obj, null);
				}
				return null;
			}
			catch { return null; }
		}

	}
}