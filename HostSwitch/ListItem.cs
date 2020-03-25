﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace HostSwitch
{
    public class ListItem
    {
        private string id = string.Empty;
        private string name = string.Empty;
        //可以根据自己的需求继续添加,如：private Int32 m_Index；

        public ListItem()
        { }
        public ListItem(string sid, string sname)
        {
            id = sid;
            name = sname;
        }
        public override string ToString()
        {
            return this.name;
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
