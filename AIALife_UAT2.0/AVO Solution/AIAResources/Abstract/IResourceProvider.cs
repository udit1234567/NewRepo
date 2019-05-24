﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAResources.Abstract
{
    public interface IResourceProvider
    {
        object GetResource(string name, string culture);
    }
}
