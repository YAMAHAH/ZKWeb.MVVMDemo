﻿using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces
{
    public interface IHaveRootVersion<TPrimaryKey> : IEntity
    {
        TPrimaryKey RootVersionId { get; set; }

        IHaveRootVersion<TPrimaryKey> RootVersion { get; set; }

    }
}