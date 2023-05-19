﻿using Core.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.DataAccess.Abstract
{
    public interface IAdminUserRepository : IEntityRepository<AdminUser>
    {
    }
}