using NetArchTest.Rules;
using TestTask.Api.AzureFunctions;
using TestTask.Application.Inerfaces;
using TestTask.Domain.Entites;
using TestTask.Infrastructure.Inerfaces.Services;

namespace ArchTests
{
    [TestClass]
    public class ArchTests
    {
        protected string _applicationAssemblyName = typeof(IWeatherService).Assembly.GetName().Name;
        protected string _domainAssemblyName = typeof(WeatherLog).Assembly.GetName().Name;
        protected string _infrastructureAssemblyName = typeof(IOpenWeatherService).Assembly.GetName().Name;
        protected string _apiAssemblyName = typeof(WeatherFunction).Assembly.GetName().Name;

        [TestMethod]
        public void Api_ShouldHave()
        {

            var shouldNotHaveDependecy = new[] { _domainAssemblyName, _infrastructureAssemblyName };

            var result = Types.InAssembly(typeof(WeatherFunction).Assembly)
             .Should()
             .HaveDependencyOn(_applicationAssemblyName)
             .And()
             .NotHaveDependencyOnAll(shouldNotHaveDependecy)
             .GetResult();

            Assert.AreEqual(true, result.IsSuccessful,
              $"Api should have dependency on: {_applicationAssemblyName} and not have depenendecy on  {string.Join(", ", shouldNotHaveDependecy)}");
        }

        [TestMethod]
        public void Domain_ShouldNoHave()
        {
            var shouldNotHaveDependecy = new[] { _infrastructureAssemblyName, _apiAssemblyName, _infrastructureAssemblyName };

            var result = Types.InAssembly(typeof(WeatherLog).Assembly)
             .Should()
             .NotHaveDependencyOnAll(shouldNotHaveDependecy)
             .GetResult();

            Assert.AreEqual(true, result.IsSuccessful,
              $"Domain should  not have depenendecy on {string.Join(", ", shouldNotHaveDependecy)}");
        }
    }
}