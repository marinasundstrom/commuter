using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Xunit;

namespace Commuter.Tests
{
    public class DeviationViewModelFactoryTest
    {
        [Fact]
        public async Task CreateViewModel()
        {
            var model = new Deviation
            {
                Header = "Foo",
                ShortText = "Foo1",
                Importance = 1,
                Influence = 3,
                Urgency = 2
            };

            var viewModelFactory = new DeviationViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            Assert.Equal(model.Header, viewModel.Header);
            Assert.Equal(model.ShortText, viewModel.ShortText);
            Assert.Equal(model.Importance, viewModel.Importance);
            Assert.Equal(model.Influence, viewModel.Influence);
            Assert.Equal(model.Urgency, viewModel.Urgency);
        }

        [Fact]
        public async Task UpdateViewModel()
        {
            var model = new Deviation
            {
                Header = "Foo",
                ShortText = "Foo1",
                Importance = 1,
                Influence = 3,
                Urgency = 2
            };

            var viewModelFactory = new DeviationViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            var model2 = new Deviation
            {
                Header = "Foo2",
                ShortText = "Foo4",
                Importance = 13,
                Influence = 53,
                Urgency = 37
            };

            await viewModelFactory.UpdateViewModelAsync(model2, viewModel);

            Assert.NotEqual(model.Header, viewModel.Header);
            Assert.Equal(model2.Header, viewModel.Header);

            Assert.NotEqual(model.ShortText, viewModel.ShortText);
            Assert.Equal(model2.ShortText, viewModel.ShortText);

            Assert.NotEqual(model.Importance, viewModel.Importance);
            Assert.Equal(model2.Importance, viewModel.Importance);

            Assert.NotEqual(model.Influence, viewModel.Influence);
            Assert.Equal(model2.Influence, viewModel.Influence);

            Assert.NotEqual(model.Urgency, viewModel.Urgency);
            Assert.Equal(model2.Urgency, viewModel.Urgency);
        }
    }
}
