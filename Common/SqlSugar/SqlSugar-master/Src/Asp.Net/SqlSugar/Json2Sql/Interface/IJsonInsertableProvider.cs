﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SqlSugar
{
    public interface IJsonInsertableProvider<T> : IJsonProvider<T>
    {
        // IJsonQueryableProvider<T> UpdateColumns(string tableName, string[] columns);
    }
}
