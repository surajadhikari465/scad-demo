﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core
{
    public static class Extensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
