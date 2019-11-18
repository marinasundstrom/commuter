using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Xunit;

namespace Commuter.Tests
{
    public class StopPointViewModelFactoryTest
    {
        [Fact]
        public async Task CreateViewModel()
        {
            var model = new StopPoint
            {
                Name = "A"
            };


            var viewModelFactory = new StopPointViewModelFactory();
            var viewModel = await viewModelFactory.CreateViewModelAsync(model);

            Assert.Equal(model.Name, viewModel.Name);

            Assert.Empty(viewModel);
        }
    }
}
