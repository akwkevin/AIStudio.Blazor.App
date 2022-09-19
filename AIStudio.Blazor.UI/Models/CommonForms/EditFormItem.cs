using System.Reflection;

namespace AIStudio.Blazor.UI.Models.CommonForms
{
    public class EditFormItem : BaseControlItem
    {
        public static EditFormItem GetEditFormItem(PropertyInfo property)
        {
            EditFormItem editFormItem = new EditFormItem();
            if (GetControlItem(property, editFormItem) == false)
                return null;

            //var attribute = ColumnHeaderAttribute.GetPropertyAttribute(property);
            //if (attribute != null)
            //{
            //    editFormItem.IsReadOnly = attribute.IsReadOnly;
            //    editFormItem.Visibility = attribute.Visibility;
            //}
            //else
            //{
            //    editFormItem.Visibility = Visibility.Visible;
            //}

            //if (_userData.ReadOnlySource.Contains(property.Name))
            //{
            //    editFormItem.IsReadOnly = true;
            //}
   
            return editFormItem;
        }
    }
}
