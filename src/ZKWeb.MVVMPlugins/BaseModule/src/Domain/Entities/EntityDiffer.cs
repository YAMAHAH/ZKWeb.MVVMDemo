using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Components.Extensions;

namespace InfrastructurePlugins.BaseModule.Domain.Entities
{
    public class EntityDiffer<T, TKey> where T : IEntity
    {
        public EntityDiffer(IEnumerable<T> existEntries, IEnumerable<T> nowEntries, Func<T, TKey> getKey)
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
