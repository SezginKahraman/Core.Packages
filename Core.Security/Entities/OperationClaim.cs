﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Persistance.Repositories;

namespace Core.Security.Entities
{
    public class OperationClaim : Entity<int>
    {
        public string Name { get; set; }

        public OperationClaim()
        {
            Name = string.Empty;
        }

        public OperationClaim(string name)
        {
            Name = name;
        }

        public OperationClaim(int id, string name):base(id)
        {
            Name = name;
        }

        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
    }
}
