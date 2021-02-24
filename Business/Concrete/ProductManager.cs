using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    //Bir iş sınıfı başka bir iş sınıflarını new'leyemez.
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        //***Bir EntityManager kendisi hariç başka bir DAL'ı enjekte edemez.
        //***Fakat başka bir Service enjekte edebilir.

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        //[LogAspect] --> AOP: Bir metodun önünde, bir metodun sonunda, bir metod hata verdiğinde çalışan kod parçacıklarına denir. Yani Business'ın içerisine Business yazılır.
        //[Validate] --> AOP
        //[Remove Cache] --> AOP
        //[Transaction] --> AOP
        //[Performance] --> AOP
        //...

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //İş Kuralı-1: Bir kategoride en fazla 10 ürün olabilir.

            //Bu iş kuralını veya genel iş kurallarını buraya yazarsak katmanlı mimari yapsak bile kod spagettiye döner.
            //var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            //if (result >= 10)
            //{
            //    return new ErrorResult(Messages.ProductCountOfCategoryError);
            //}

            //İş Kuralı-2: Aynı isimde ürün eklenemez.
            //İş Kuralı-3: Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez.

            //İş Kuralı Motoru
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            //İş Kodları
            //Yetkisi var mı?
            //...

            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId));

            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        //İş kuralı parçacığı olduğu için ilgili sınıfa private yazılmalı.
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            //Arka planda çalışan kod: SELECT COUNT(*) FROM Products WHERE CategoryId = 1
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
