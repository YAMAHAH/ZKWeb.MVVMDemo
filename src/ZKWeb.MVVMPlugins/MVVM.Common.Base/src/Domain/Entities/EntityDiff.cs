﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities
{
    public class EntityDiff<T, TKey> where T : IEntity
    {
        public EntityDiff(IEnumerable<T> existEntries, IEnumerable<T> nowEntries, Func<T, TKey> getKey)
        {
            this.existEntries = existEntries;
            this.nowEntries = nowEntries;
            this.getKey = getKey;
            this.Cala();
        }
        private IEnumerable<T> existEntries;
        private IEnumerable<T> nowEntries;
        private Func<T, TKey> getKey;

        public List<T> AddedEntities { get; set; } = new List<T>();
        public List<T> DeletedEntities { get; set; } = new List<T>();
        public List<T> ModifiedEntities { get; set; } = new List<T>();
        public bool HasValue
        {
            get
            {
                return (AddedEntities.Count > 0 || DeletedEntities.Count > 0 || ModifiedEntities.Count > 0);
            }
        }
        void Cala()
        {
            AddedEntities = nowEntries.Except(existEntries, getKey).ToList();
            DeletedEntities = existEntries.Except(nowEntries, getKey).ToList();
            ModifiedEntities = nowEntries.Except(AddedEntities, getKey).ToList();
        }
    }
}