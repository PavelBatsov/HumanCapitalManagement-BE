using HCM.Application.Services;
using HCM.Domain.Entities;
using HCM.Domain.Enums;
using HCM.Domain.Interfaces;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Manager;
using Moq;

namespace HCP.Tests.Services.Managers
{
    [TestClass]
    public class ManagerServiceTests
    {
        private Mock<IManagerRepository> managerRepositoryMock;
        private Mock<IUserHelper> userHelperMock;

        private ManagerService managerService;


        [TestInitialize]
        public void Setup()
        {
            managerRepositoryMock = new Mock<IManagerRepository>();
            userHelperMock = new Mock<IUserHelper>();

            managerService = new ManagerService(managerRepositoryMock.Object, userHelperMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldThrowException_WhenManagerAlreadyExists()
        {
            // Arrange
            var model = new ManagerModel
            { 
                Email = "existing@example.com" 
            };

            managerRepositoryMock
                .Setup(x => x.FindBy(x => x.Email == model.Email))
                .Returns(new[] { new ManagerEntity() }.AsQueryable());

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => managerService.CreateAsync(model));
            Assert.AreEqual(Strings.ManagerAlreadyExists, exception.Message);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddManager_WhenManagerDoesNotExist()
        {
            // Arrange
            var model = new ManagerModel
            {
                Email = "pavel@example.com",
                FirstName = "Pavel",
                LastName = "Batsov",
                PhoneNumber = "123",
                Address = "Some address",
                ManagerType = ManagerType.DevManager
            };

            managerRepositoryMock
                .Setup(x => x.FindBy(x => x.Email == model.Email))
                .Returns(Enumerable.Empty<ManagerEntity>().AsQueryable());

            userHelperMock
                .Setup(helper => helper.CurrentUserId()).Returns(Guid.NewGuid());

            // Act
            await managerService.CreateAsync(model);

            // Assert
            managerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ManagerEntity>()), Times.Once);
            managerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var model = new ManagerModel
            { 
                Id = Guid.NewGuid(),
                Email = "existing@example.com" 
            };

            var existingManager = new ManagerEntity
            { 
                Id = Guid.NewGuid(),
                Email = model.Email
            };

            managerRepositoryMock
                .Setup(repo => repo.GetAsync(model.Id))
                .ReturnsAsync(new ManagerEntity());

            managerRepositoryMock
                .Setup(repo => repo.FindBy(x => x.Email == model.Email && x.Id != model.Id))
                .Returns(new[] { existingManager }.AsQueryable());

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => managerService.UpdateAsync(model));
            Assert.AreEqual(string.Format(Strings.ManagerWithThatEmailExist, model.Email), exception.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateManager_WhenValid()
        {
            // Arrange
            var model = new ManagerModel
            {
                Id = Guid.NewGuid(),
                Email = "valid@example.com",
                FirstName = "Pavel",
                LastName = "Batsov",
                PhoneNumber = "123",
                Address = "Some address",
                ManagerType = ManagerType.DevManager
            };

            var managerEntity = new ManagerEntity
            { 
                Id = model.Id,
                Email = "old@example.com"
            };

            managerRepositoryMock
                .Setup(repo => repo.GetAsync(model.Id))
                .ReturnsAsync(managerEntity);

            managerRepositoryMock
                .Setup(repo => repo.FindBy(x => x.Email == model.Email && x.Id != model.Id))
                .Returns(Enumerable.Empty<ManagerEntity>().AsQueryable());

            userHelperMock
                .Setup(helper => helper.CurrentUserId()).Returns(Guid.NewGuid());

            // Act
            var result = await managerService.UpdateAsync(model);

            // Assert
            Assert.AreEqual(model.Email, result.Email);
            Assert.AreEqual(model.FirstName, result.FirstName);
            Assert.AreEqual(model.LastName, result.LastName);
            Assert.AreEqual(model.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(model.Address, result.Address);
            Assert.AreEqual(model.ManagerType, result.ManagerType);

            managerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldThrowException_WhenManagerNotFound()
        {
            // Arrange
            var managerId = Guid.NewGuid();

            managerRepositoryMock
                .Setup(repo => repo.GetAsync(managerId))
                .ReturnsAsync((ManagerEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => managerService.DeleteAsync(managerId));
            Assert.AreEqual(Strings.ManagerNotFound, exception.Message);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteManager_WhenManagerExists()
        {
            // Arrange
            var managerId = Guid.NewGuid();
            var managerEntity = new ManagerEntity
            { 
                Id = managerId 
            };

            managerRepositoryMock
                .Setup(repo => repo.GetAsync(managerId))
                .ReturnsAsync(managerEntity);

            managerRepositoryMock
                .Setup(repo => repo.Delete(managerEntity));

            managerRepositoryMock
                .Setup(repo => repo.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            await managerService.DeleteAsync(managerId);

            // Assert
            managerRepositoryMock.Verify(repo => repo.Delete(managerEntity), Times.Once);
            managerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnListOfManagers()
        {
            // Arrange
            var managers = new List<ManagerEntity>
            {
                new ManagerEntity 
                { 
                    Id = Guid.NewGuid(),
                    Email = "pavel@example.com",
                    FirstName = "Pavel",
                    LastName = "Batsov",
                    PhoneNumber = "123",
                    Address = "Some address",
                    ManagerType = ManagerType.DevManager 
                },
                new ManagerEntity 
                { 
                    Id = Guid.NewGuid(),
                    Email = "teddy@example.com",
                    FirstName = "Teddy",
                    LastName = "Georgieva",
                    PhoneNumber = "456",
                    Address = "Same address",
                    ManagerType = ManagerType.QAManager 
                }
            };

            managerRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(managers);

            // Act
            var result = await managerService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(managers.Count, result.Count());
            Assert.AreEqual(managers[0].Email, result.First().Email);
            Assert.AreEqual(managers[1].Email, result.Last().Email);
        }
    }
}
