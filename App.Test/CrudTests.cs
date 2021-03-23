using App.Core.Models;
using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WebApplication1.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace App.Test
{
    public class CrudTests
    {
        private class DummyDataDBInitializer
        {
            public DummyDataDBInitializer()
            {
            }

            public void Seed(ApplicationDbContext context)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BaseEntities.AddRange(
                    new BaseEntity() { Id = 1,  Name = "CSHARP", ActiveFlag = true },
                    new BaseEntity() { Id = 2,  Name = "JAva", ActiveFlag = true },
                    new BaseEntity() { Id = 3,  Name = "Js", ActiveFlag = true }
                );

                context.SaveChanges();
            }

            public ApplicationDbContext CreateContextForInMemory()
            {
                var option = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "BaseList").Options;

                var context = new ApplicationDbContext(option);
                if (context != null)
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                return context;
            }
        }
        public class XunitLogger<T> : ILogger<T>, IDisposable
        {
            private ITestOutputHelper _output;

            public XunitLogger(ITestOutputHelper output)
            {
                _output = output;
            }
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _output.WriteLine(state.ToString());
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return this;
            }

            public void Dispose()
            {
            }
        }

        private Repository<BaseEntity> _repo;
        private XunitLogger<TestController> _logger;
        public CrudTests(ITestOutputHelper output)
        {
            var context = new DummyDataDBInitializer().CreateContextForInMemory();
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            _repo = new Repository<BaseEntity>(context);
            _logger = new XunitLogger<TestController>(output);
        }

        [Fact]
        public async void GetById_Return_OkResult()
        {
            //Arrange  
            var controller = new TestController(_logger, _repo);
            var baseId = 2;

            //Act  
            var data = await controller.GetById(baseId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetPostById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new TestController(_logger, _repo);
            int id = 5;

            //Act  
            var data = await controller.GetById(id);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetPosts_Return_OkResult()
        {
            //Arrange  
            var controller = new TestController(_logger, _repo);

            //Act  
            var data = await controller.Get();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new TestController(_logger, _repo);
            var @base = new BaseEntity() { Id = 10, Name = "Test Title 3",  ActiveFlag = true };

            //Act  
            var data = await controller.Create(@base);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }
    }
}
