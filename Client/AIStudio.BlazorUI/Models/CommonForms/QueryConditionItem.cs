﻿using System.Reflection;

namespace AIStudio.BlazorUI.Models.CommonForms
{
    public class QueryConditionItem : BaseControlItem
    {
        public static QueryConditionItem GetQueryConditionItem(PropertyInfo property)
        {      
            var queryConditionItem = new QueryConditionItem();
            if (GetControlItem(property, queryConditionItem) == false)
                return null;

            //var attribute = ColumnHeaderAttribute.GetPropertyAttribute(property);
            //if (attribute != null)
            //{
            //    queryConditionItem.Visibility = attribute.IsPin ? Visibility.Visible : Visibility.Collapsed;
            //}
            //else
            //{
            //    queryConditionItem.Visibility =  Visibility.Collapsed;
            //}
            return queryConditionItem;

        }       
    }
}
