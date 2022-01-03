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
            _logger.LogInformation("Инициализация БД выполнена успешно");

        }

        private async Task InitializeProductAsync(CancellationToken Cancel)
        {
            if (_db.Sections.Any())
            {
                _logger.LogInformation("Инициализация тестовых данных не требуется");
                return;
            }

            _logger.LogInformation("Инициализация тестовых данных ...");

            _logger.LogInformation("Добавление секций в БД ...");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel);
                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _logger.LogInformation("Добавление брэндов в БД ...");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel);
                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _logger.LogInformation("Добавление продуктов в БД ...");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel);
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
