using My_Pos.DbContexts;
using My_Pos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Pos.Services
{
    public class ProductService
    {
        private readonly SqlServerDbContext _context;

        public ProductService(SqlServerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return new List<Products>();
            }
        }

        public async Task<List<Products>> GetAllProductsByDescriptAsync(string descript)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.ReportCatId == Convert.ToInt32(descript))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products by descript: {ex.Message}");
                return new List<Products>();
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Category.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<Category>();
            }
        }

        // ✅ New: Fetch all active Sizes
      
        }
    }

