using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.Base
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }
        protected Entity(TId id) => Id = id;
    }
}
