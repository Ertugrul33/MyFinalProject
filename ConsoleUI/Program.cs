using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    //SOLID
    //O -> Open Closed Principle
    class Program
    {
        static void Main(string[] args)
        {
            //Abstract -> Soyutlar (Interfaces, abstracts...) (Referans tutuculardır.)
            //Concrete -> Somutlar (İşi yapan sınıflar)

            ProductManager productManager = new ProductManager(new EfProductDal());
            foreach (var product in productManager.GetByUnitPrice(40,100))
            {
                Console.WriteLine(product.ProductName);
            }
        }
    }
}
