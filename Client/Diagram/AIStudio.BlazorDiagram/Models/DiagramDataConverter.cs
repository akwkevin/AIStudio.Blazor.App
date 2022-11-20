using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AIStudio.BlazorDiagram.Models
{
    public class DiagramNodeConverter : DATACreationConverter<DiagramNode>
    {
        protected override DiagramNode Create(Type objectType, JObject jObject)
        {
            //第一种方法：判断属性值来确认是哪个派生类 
            if (FieldExists("Type", jObject, out string type))
            {
                if (type == "DatabaseDesignerTableNode")
                {
                    return new DatabaseDesignerTableNode();
                }
                else
                {
                    return new DiagramNode();
                }
            }
            else
            {
                return new DiagramNode();
            }

            //第二种方法：判断字段是否存在来确认是哪个派生类
        }

        private bool FieldExists(string fieldName, JObject jObject, out string entityName)
        {
            entityName = jObject[fieldName] == null ? "" : jObject[fieldName].ToString();
            return jObject[fieldName] != null;
        }
    }

    public class DiagramLinkConverter : DATACreationConverter<DiagramLink>
    {
        protected override DiagramLink Create(Type objectType, JObject jObject)
        {
            //第一种方法：判断属性值来确认是哪个派生类 
            if (FieldExists("Type", jObject, out string type))
            {
                if (type == "DatabaseDesignerTableLink")
                {
                    return new DatabaseDesignerTableLink();
                }
                else
                {
                    return new DiagramLink();
                }
            }
            else
            {
                return new DiagramLink();
            }

            //第二种方法：判断字段是否存在来确认是哪个派生类
        }

        private bool FieldExists(string fieldName, JObject jObject, out string entityName)
        {
            entityName = jObject[fieldName] == null ? "" : jObject[fieldName].ToString();
            return jObject[fieldName] != null;
        }
    }

    public abstract class DATACreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
