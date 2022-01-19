using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<DbInitializer> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DbInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
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
            await InitializeIdentityAsync(Cancel).ConfigureAwait(false);

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

        private async Task InitializeIdentityAsync(CancellationToken Cancel)
        {
            _logger.LogInformation("Инициализация данных системы Identity...");

            var timer = Stopwatch.StartNew();

            async Task CheckRole(string roleName)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    _logger.LogInformation("Роль {0} существует в БД. {1} с.", roleName, timer.Elapsed.TotalSeconds);
                }
                else
                {
                    _logger.LogInformation("Роль {0} не существует в БД. {1} с.", roleName, timer.Elapsed.TotalSeconds);
                    
                    await _roleManager.CreateAsync(new Role { Name = roleName });
                    
                    _logger.LogInformation("Роль {0} создана в БД. {1} с.", roleName, timer.Elapsed.TotalSeconds);
                }
            }

            await CheckRole(Role.Administrotors);
            await CheckRole(Role.Users);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation("Пользователь {0} отсутствует в БД. Создаю... {1} с.", User.Administrator, timer.Elapsed.TotalSeconds);

                var admin = new User { UserName = User.Administrator };

                var create_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (create_result.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} создан в БД. Наделяю правами администратора. {1} с.", User.Administrator, timer.Elapsed.TotalSeconds);

                    await _userManager.AddToRoleAsync(admin, Role.Administrotors);
                    
                    _logger.LogInformation("Пользователь {0} готов к работе. {1} с.", User.Administrator, timer.Elapsed.TotalSeconds);
                }
                else
                {
                    var errors = create_result.Errors.Select(e => e.Description);
                    _logger.LogInformation("учётная запись администратора не создана. Ошибки: {0}", String.Join(',', errors));
                    throw new InvalidOperationException($"Невозможно создать пользователя {User.Administrator}, по причине {String.Join(',', errors)}");
                }
            }

            _logger.LogInformation("Инициализация данных системы Identity успешно завершена");

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
