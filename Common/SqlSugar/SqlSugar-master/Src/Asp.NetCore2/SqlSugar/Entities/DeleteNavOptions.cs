﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar 
{
    public class DeleteNavOptions
    {
        public bool ManyToManyIsDeleteA { get; set; }
        public bool ManyToManyIsDeleteB { get; set; }
    }   
    public class UpdateNavOptions
    {
        public bool ManyToManyIsUpdateA { get; set; }
        public bool ManyToManyIsUpdateB { get; set; }
        public bool OneToManyDeleteAll { get;  set; }
        public Expression RootFunc { get; set; }
        public Expression CurrentFunc { get; set; }
    }

    public class InsertNavOptions 
    {
        public bool OneToManyIfExistsNoInsert { get; set; }
    }
}
