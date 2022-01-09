using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(WebStoreDB db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
        {
            _logger.LogInformation("Инициализация БД ...");

            if (RemoveBefore)
            {
                await RemoveAsync(Cancel).ConfigureAwait(false);
            }

            var pend_migration = await _db.Database.GetPendingMigrationsAsync(Cancel); 
            if (pend_migration.Any())
            {
                _logger.LogInformation("Выполнение миграции БД ...");

                await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);
                _logger.LogInformation("Выполнение миграции БД успешно");

            }

            await InitializeProductAsync(Cancel).ConfigureAwait(false);
            await InitializeEmployeeAsync(Cancel).ConfigureAwait(false);

            _logger.LogInformation("Инициализация БД выполнена успешно");

        }

        private async Task InitializeEmployeeAsync(CancellationToken Cancel)
        {
            if (_db.Employees.Any())
            {
                _logger.LogInformation("Инициализация тестовых данных не требуется");
                return;
            }

            _logger.LogInformation("Инициализация тестовых данных ...");

            _logger.LogInformation("Добавление сотрудников в БД ...");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                TestData.Employees.ForEach(emp => emp.Id = 0);

                await _db.Employees.AddRangeAsync(TestData.Employees, Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.CommitTransactionAsync(Cancel);
            }
        }

        private async Task InitializeProductAsync(CancellationToken Cancel)
        {
            if (_db.Sections.Any())
            {
                _logger.LogInformation("Инициализация тестовых данных не требуется");
                return;
            }

            _logger.LogInformation("Инициализация тестовых данных ...");

            var sections_pool = TestData.Sections.ToDictionary(s => s.Id);
            var brands_pool = TestData.Brands.ToDictionary(b => b.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
            {
                child_section.Parent = sections_pool[(int)child_section.ParentId!];
            }

            foreach(var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brandId)
                {
                    product.Brand=brands_pool[brandId];
                }

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
            {
                brand.Id = 0;
            }

            await using(await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);

                await _db.SaveChangesAsync(Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _logger.LogInformation("Инициализация тестовых данных завершена");

        }

        public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
        {
            _logger.LogInformation("Удаление БД ...");
            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);
            if (result)
            {
                _logger.LogInformation("Удаление БД успешно");
            }
            else
                _logger.LogInformation("Удаление БД не требуется");

            return result;

        }
    }
}
