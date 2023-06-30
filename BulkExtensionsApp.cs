using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlateBulkEFCore
{
    public class BulkExtensionsApp
    {
        public static async Task Main()
        {
            // Sample data
            List<Person> persons = new List<Person>
            {
                new Person { Name = "John", Age = 25 },
                new Person { Name = "Jane", Age = 30 },
                new Person { Name = "Mark", Age = 40 },
                // Add more persons as needed
            };

            // Insert bulk data (async)
            await using (var context = new YourDbContext())
            {
                await context.BulkInsertAsync(persons);
            }

            // Update bulk data (async)
            await using (var context = new YourDbContext())
            {
                var personsToUpdate = await context.Persons.Where(p => p.Age > 30).ToListAsync();
                personsToUpdate.ForEach(p => p.Age += 1);
                await context.BulkUpdateAsync(personsToUpdate);
            }

            // Delete bulk data (async)
            await using (var context = new YourDbContext())
            {
                var personsToDelete = await context.Persons.Where(p => p.Age < 25).ToListAsync();
                await context.BulkDeleteAsync(personsToDelete);
            }

            // Batch delete (async)
            await using (var context = new YourDbContext())
            {
                await context.BatchDeleteAsync<Person>(p => p.Age >= 50);
            }

            // Insert bulk data (sync)
            using (var context = new YourDbContext())
            {
                context.BulkInsert(persons);
            }

            // Update bulk data (sync)
            using (var context = new YourDbContext())
            {
                var personsToUpdate = context.Persons.Where(p => p.Age > 30).ToList();
                personsToUpdate.ForEach(p => p.Age += 1);
                context.BulkUpdate(personsToUpdate);
            }

            // Delete bulk data (sync)
            using (var context = new YourDbContext())
            {
                var personsToDelete = context.Persons.Where(p => p.Age < 25).ToList();
                context.BulkDelete(personsToDelete);
            }

            // Batch delete (sync)
            using (var context = new YourDbContext())
            {
                context.BatchDelete<Person>(p => p.Age >= 50);
            }

            Console.WriteLine("Bulk operations completed successfully.");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class YourDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("YourDatabaseName");
        }

        public async Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            await Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public void BulkInsert<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().AddRange(entities);
            SaveChanges();
        }

        public async Task BulkUpdateAsync<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().UpdateRange(entities);
            await SaveChangesAsync();
        }

        public void BulkUpdate<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().UpdateRange(entities);
            SaveChanges();
        }

        public async Task BulkDeleteAsync<T>(IEnumerable<T> entities) where T : class
