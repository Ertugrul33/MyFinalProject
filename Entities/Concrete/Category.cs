using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    //Çıplak Class Kalmasın Standardı -> Eğer bir class inheritance(miras) ve interface(arayüz, şablon) almıyorsa ileride sıkıntı çıkacak demektir.)
    public class Category:IEntity //Category artık bir veri tabanı tablosudur.
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
