using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.ProductDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _genericRepo;
        private readonly IGenericRepository<ProductImage> _imageGenericRepo;
        private readonly IGenericRepository<SubCategory> _subCategoryGenericRepo;
        private readonly IGenericRepository<ProductionCompany> _companyGenericRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<AddProductDTO> _addProductValidator;
        private readonly IValidator<UpdateProductDTO> _updateProductValidator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IGenericRepository<Product> genericRepo, IMapper mapper,
            IValidator<UpdateProductDTO> updateProductValidator,
            IValidator<AddProductDTO> addProductValidator,
            IWebHostEnvironment webHostEnvironment, IGenericRepository<ProductImage> imageGenericRepo)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _updateProductValidator = updateProductValidator;
            _addProductValidator = addProductValidator;
            _webHostEnvironment = webHostEnvironment;
            _imageGenericRepo = imageGenericRepo;
        }

        public async Task<GenericResponse<bool>> Add(AddProductDTO productDTO)
        {
            var response = new GenericResponse<bool>();
            var validator = await _addProductValidator.ValidateAsync(productDTO);

            if (validator.IsValid)
            {
                var subCategory = await _subCategoryGenericRepo.GetById(productDTO.SubCategoryId);
                var company = await _companyGenericRepo.GetById(productDTO.ProductionCompanyId);
                if (subCategory is not null && company is not null)
                {
                    var entity = _mapper.Map<Product>(productDTO);

                    await _genericRepo.Add(entity);
                    await _genericRepo.Commit();

                    response.Success(true);
                    return response;
                }
                response.Error(400, "Subcategory or company do not exist!");
                return response;
            }

            response.Error(400, "Invalid information added!");
            return response;

        }

        public async Task<GenericResponse<bool>> AddImage(AddImageDTO imageDTO)
        {
            var response = new GenericResponse<bool>();

            var fileName = string.Concat(Guid.NewGuid(), imageDTO.Image.FileName);
            var imageFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages");
            var productImagePath = Path.Combine(imageFolderPath, imageDTO.ProductId.ToString());
            var fullPath = Path.Combine(productImagePath, fileName);

            if (!Directory.Exists(productImagePath))
            {
                Directory.CreateDirectory(productImagePath);
            }

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                imageDTO.Image.CopyTo(fs);
                fs.Close();
            }

            var image = new ProductImage()
            {
                ProductId = imageDTO.ProductId,
                ImagePath = fileName
            };
            await _imageGenericRepo.Add(image);
            await _imageGenericRepo.Commit();

            response.Success(true);
            return response;
        }

        public async Task<GenericResponse<bool>> DeleteProductImage(DeleteImageDTO imageDTO)
        {
            var response = new GenericResponse<bool>();
            var imageEntity = await _imageGenericRepo.GetByExpression(i => i.Id == imageDTO.Id && i.ProductId == imageDTO.ProductId)
                .FirstOrDefaultAsync();

            if (imageEntity is not null)
            {
                var fileName = imageEntity.ImagePath;
                var imageFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages");
                var productImagePath = Path.Combine(imageFolderPath, imageDTO.ProductId.ToString());
                var fullPath = Path.Combine(productImagePath, fileName);

                File.Delete(fullPath);
                _imageGenericRepo.Delete(imageDTO.Id);
                await _imageGenericRepo.Commit();

                response.Success(true);
                return response;
            }
            response.Error(400,"Image does not exist!");
            return response;
        }

        public async Task<GenericResponse<GetProductDTO>> GetProductById(int id)
        {
            var response = new GenericResponse<GetProductDTO>();

            var entity = await _genericRepo.GetByExpression(p => p.Id == id)
            .Include(p => p.ProductInformations)
            .Include(p => p.Images).FirstOrDefaultAsync();

            if (entity is not null)
            {
                var product = _mapper.Map<GetProductDTO>(entity);
                response.Success(product);
                return response;
            }
            response.Error(400, "Product does not exist!");
            return response;

        }

        public async Task<GenericResponse<List<string>>> GetProductImages(int id)
        {
            var response = new GenericResponse<List<string>>();
            var product = await _genericRepo.GetByExpression(p => p.Id == id).Include(p => p.Images).FirstOrDefaultAsync();
            List<string> base64List = new List<string>();

            if (product is not null)
            {
                if (product.Images is not null)
                {
                    foreach (var image in product.Images)
                    {
                        var fileName = image.ImagePath;
                        var imageFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages");
                        var productImagePath = Path.Combine(imageFolderPath, id.ToString());
                        var fullPath = Path.Combine(productImagePath, fileName);

                        byte[] fileData = default(byte[]);

                        using (var fs = new FileStream(fullPath, FileMode.Open))
                        {
                            fileData = new byte[fs.Length];
                            await fs.ReadAsync(fileData, 0, fileData.Length);
                            fs.Close();
                        }

                        base64List.Add(Convert.ToBase64String(fileData));
                    }

                    response.Success(base64List);
                    return response;
                }
                response.Error(400, "There are no images!");
                return response;
            }
            response.Error(400, "Product does not exist!");
            return response;
        }

        public async Task<GenericResponse<IEnumerable<GetProductDTO>>> GetProducts()
        {
            var response = new GenericResponse<IEnumerable<GetProductDTO>>();

            var entities = await _genericRepo.GetAll().Include(p => p.ProductInformations)
            .Include(p => p.Images).ToListAsync();

            if (entities is not null)
            {
                var products = _mapper.Map<IEnumerable<GetProductDTO>>(entities);

                response.Success(products);
                return response;
            }
            response.Error(400, "Products do not exist!");
            return response;

        }

        public async Task<GenericResponse<bool>> SoftDelete(int id)
        {
            var response = new GenericResponse<bool>();

            if (await _genericRepo.GetById(id) is not null)
            {
                var entity = await _genericRepo.GetById(id);
                entity.IsDeleted = true;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();

                response.Success(true);
                return response;
            }
            response.Error(400, "Product does not exist!");
            return response;

        }

        public async Task<GenericResponse<bool>> Update(UpdateProductDTO productDTO)
        {
            var response = new GenericResponse<bool>();
            var validator = await _updateProductValidator.ValidateAsync(productDTO);

            if (validator.IsValid)
            {
                if (await _genericRepo.GetById(productDTO.Id) is not null)
                {
                    var entity = await _genericRepo.GetById(productDTO.Id);
                    var product = _mapper.Map(productDTO, entity);
                    product.UpdatedDate = DateTime.Now;

                    _genericRepo.Update(product);
                    await _genericRepo.Commit();

                    response.Success(true);
                    return response;
                }

                response.Error(400, "Product does not exist!");
                return response;
            }
            response.Error(400, "Invalid information!");
            return response;

        }
    }
}
