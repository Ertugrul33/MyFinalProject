using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    //Generic Constraint
    //Class: Burada class, referans tip olabilir demektir.
    //IEntity: IEntity olabilir veya IEntity implemente eden bir nesne olabilir.
    //new(): new'lenebilir olmalı.

    //Core hiçbir katmandan/sınıftan referans almaz.***
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
