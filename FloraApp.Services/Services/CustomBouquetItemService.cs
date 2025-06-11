using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Services.Database;
using FloraApp.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloraApp.Services.Services
{
    public class CustomBouquetItemService : ICustomBouquetItemService
    {
        private readonly FloraAppDbContext _context;
        private readonly IMapper _mapper;

        public CustomBouquetItemService(FloraAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomBouquetItemResponse>> GetByBouquetIdAsync(int bouquetId)
        {
            var items = await _context.CustomBouquetItems
                .Include(cbi => cbi.Product)
                .Where(cbi => cbi.CustomBouquetId == bouquetId)
                .ToListAsync();

            return items.Select(MapToResponse).ToList();
        }

        public async Task<CustomBouquetItemResponse> AddItemAsync(int bouquetId, CustomBouquetItemUpsertRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId)
                ?? throw new ArgumentException("Product not found");

            var item = new CustomBouquetItem
            {
                CustomBouquetId = bouquetId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = product.Price * request.Quantity
            };

            _context.CustomBouquetItems.Add(item);
            await _context.SaveChangesAsync();
            await UpdateBouquetPriceAsync(bouquetId);

            return await GetItemWithDetailsAsync(item.Id);
        }

        public async Task<CustomBouquetItemResponse> UpdateItemAsync(int itemId, CustomBouquetItemUpsertRequest request)
        {
            var item = await _context.CustomBouquetItems
                .Include(cbi => cbi.Product)
                .FirstOrDefaultAsync(cbi => cbi.Id == itemId)
                ?? throw new ArgumentException("Item not found");

            var product = await _context.Products.FindAsync(request.ProductId)
                ?? throw new ArgumentException("Product not found");

            item.ProductId = request.ProductId;
            item.Quantity = request.Quantity;
            item.Price = product.Price * request.Quantity;

            await _context.SaveChangesAsync();
            await UpdateBouquetPriceAsync(item.CustomBouquetId);

            return await GetItemWithDetailsAsync(item.Id);
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _context.CustomBouquetItems.FindAsync(itemId)
                ?? throw new ArgumentException("Item not found");

            var bouquetId = item.CustomBouquetId;
            _context.CustomBouquetItems.Remove(item);
            await _context.SaveChangesAsync();
            await UpdateBouquetPriceAsync(bouquetId);
        }

        public async Task UpdateBouquetPriceAsync(int bouquetId)
        {
            var bouquet = await _context.CustomBouquets.FindAsync(bouquetId)
                ?? throw new ArgumentException("Bouquet not found");

            var totalPrice = await _context.CustomBouquetItems
                .Where(cbi => cbi.CustomBouquetId == bouquetId)
                .SumAsync(cbi => cbi.Price);

            bouquet.Price = totalPrice;
            await _context.SaveChangesAsync();
        }

        private async Task<CustomBouquetItemResponse> GetItemWithDetailsAsync(int itemId)
        {
            var item = await _context.CustomBouquetItems
                .Include(cbi => cbi.Product)
                .FirstOrDefaultAsync(cbi => cbi.Id == itemId)
                ?? throw new ArgumentException("Item not found");

            return MapToResponse(item);
        }

        private CustomBouquetItemResponse MapToResponse(CustomBouquetItem item)
        {
            var response = _mapper.Map<CustomBouquetItemResponse>(item);
            response.ProductName = item.Product?.Name ?? string.Empty;
            response.ProductPrice = item.Product?.Price ?? 0;
            return response;
        }
    }
} 