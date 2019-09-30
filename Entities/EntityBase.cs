using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
