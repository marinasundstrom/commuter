using System;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Xunit;

namespace Commuter.Tests
{
    public class DepartureViewModelFactoryTest
    {
        [Fact]
        public async Task CreateViewModel()
        {
            var model = new Departure
            {
                RunNo = 300,
                LineType = "SkåneExpressen",
                No = 42,
                Name = "42 Test",
                Towards = "Nowhere",
                DepartureTime = DateTime.Now.AddMinutes(2)
            };


            var viewModelFactory = new DepartureViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            Assert.Equal(model.RunNo, viewModel.RunNo);
            Assert.Equal(model.LineType, viewModel.LineType);
            Assert.Equal(model.No, viewModel.No);
            Assert.Equal(model.Name, viewModel.Name);
            Assert.Equal(model.Towards, viewModel.Towards);
            Assert.Equal(model.DepartureTime, viewModel.Time);

            Assert.Empty(viewModel.Deviations);
        }

        [Fact]
        public async Task UpdateViewModel()
        {
            var model = new Departure
            {
                RunNo = 300,
                LineType = "SkåneExpressen",
                No = 42,
                Name = "42 Test",
                Towards = "Nowhere",
                DepartureTime = DateTime.Now.AddMinutes(2)
            };

            var viewModelFactory = new DepartureViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            var model2 = new Departure
            {
                RunNo = 300,
                LineType = "Stadsbuss",
                No = 43,
                Name = "43 Test",
                Towards = "Nowhere f",
                DepartureTime = DateTime.Now.AddMinutes(4)
            };

            await viewModelFactory.UpdateViewModelAsync(model2, viewModel);

            Assert.Equal(model.RunNo, viewModel.RunNo);
            Assert.Equal(model2.RunNo, viewModel.RunNo);

            Assert.NotEqual(model.LineType, viewModel.LineType);
            Assert.Equal(model2.LineType, viewModel.LineType);

            Assert.NotEqual(model.No, viewModel.No);
            Assert.Equal(model2.No, viewModel.No);

            Assert.NotEqual(model.Name, viewModel.Name);
            Assert.Equal(model2.Name, viewModel.Name);

            Assert.NotEqual(model.Towards, viewModel.Towards);
            Assert.Equal(model2.Towards, viewModel.Towards);

            Assert.NotEqual(model.DepartureTime, viewModel.Time);
            Assert.Equal(model2.DepartureTime, viewModel.Time);

            Assert.Empty(viewModel.Deviations);
        }
    }
}
