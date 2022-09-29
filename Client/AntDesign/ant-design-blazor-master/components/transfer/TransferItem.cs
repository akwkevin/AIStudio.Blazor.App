﻿using System.Collections.Generic;

namespace AntDesign
{
    public class TransferItem
    {
        public string Key { get; set; } = "";

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Disabled { get; set; }

        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

        public object this[string index]
        {
            get
            {
                return _properties[index];
            }
            set
            {
                _properties[index] = value;
            }
        }
    }
}
