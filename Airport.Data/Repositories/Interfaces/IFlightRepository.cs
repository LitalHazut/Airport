﻿using Airport.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Data.Repositories.Interfaces
{
    public interface IFlightRepository<T> where T : class
    { 
        IQueryable<T> GetAll();
        void Create(T entity);

    }
}
