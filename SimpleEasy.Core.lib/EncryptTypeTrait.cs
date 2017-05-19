using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleEasy.Core.lib
{
    public static class EncryptTypeTrait<TEntity>
    {
        public readonly static bool HaveEncrypt = typeof(IEncryptInput).GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo());
    }
}
