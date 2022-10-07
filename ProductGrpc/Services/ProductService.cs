﻿using Grpc.Core;
using ProductGrpc.Application.Includes;
using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductCrpcCommunicator.ProductCrpcCommunicatorBase
    {
        private readonly IProductApplication _productApplication;

        public ProductService(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        public override async Task<ProductCreatorResult> Create(ProductCreatorRequest request, ServerCallContext context)
        {
            var product = new Product(request.Name, request.Description, request.Price);
            var result = await _productApplication.SaveAsync(product);

            var response = new ProductCreatorResult
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                Status = (Int32)result.Status,
                CreatedAt = result.CreatedAt.ToString("dd/MM/yyyy")
            };

            return response;
        }
    }
}
