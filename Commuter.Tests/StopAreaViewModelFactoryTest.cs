using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Xunit;

namespace Commuter.Tests
{
    public class StopAreaViewModelFactoryTest
    {
        [Fact]
        public async Task CreateViewModel()
        {
            var model = new StopAreaInternal
            {
                StopAreaId = 42,
                Name = "Neverland",
                Distance = 150,
                X = 13,
                Y = 45
            };

            var viewModelFactory = new StopAreaViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            Assert.Equal(model.StopAreaId, viewModel.StopAreaId);
            Assert.Equal(model.Name, viewModel.Name);
            Assert.Equal(model.Distance, viewModel.Distance);
            Assert.Equal(model.X, viewModel.X);
            Assert.Equal(model.Y, viewModel.Y);

            Assert.Empty(viewModel);
        }

        [Fact]
        public async Task UpdateViewModel()
        {
            var model = new StopAreaInternal
            {
                StopAreaId = 42,
                Name = "Neverland",
                Distance = 150,
                X = 13,
                Y = 45
            };

            var viewModelFactory = new StopAreaViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            var model2 = new StopAreaInternal
            {
                StopAreaId = 42,
                Name = "Neverlan 2",
                Distance = 370,
                X = 15,
                Y = 98
            };

            await viewModelFactory.UpdateViewModelAsync(model2, viewModel);

            Assert.Equal(model.StopAreaId, viewModel.StopAreaId);
            Assert.Equal(model2.StopAreaId, viewModel.StopAreaId);

            Assert.NotEqual(model.Name, viewModel.Name);
            Assert.Equal(model2.Name, viewModel.Name);

            Assert.NotEqual(model.Distance, viewModel.Distance);
            Assert.Equal(model2.Distance, viewModel.Distance);

            Assert.NotEqual(model.X, viewModel.X);
            Assert.Equal(model2.X, viewModel.X);

            Assert.NotEqual(model.Y, viewModel.Y);
            Assert.Equal(model2.Y, viewModel.Y);

            Assert.Empty(viewModel);
        }
    }
}
