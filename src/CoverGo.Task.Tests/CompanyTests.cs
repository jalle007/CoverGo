using CoverGo.Task.Domain;

namespace CoverGo.Task.Tests
{
    public class CompanyTests
    {
        [Fact]
        public void Constructor_ShouldSetCompanyName()
        {
            // Arrange
            var expectedName = "Test Company";

            // Act
            var company = new Company(expectedName);

            // Assert
            Assert.Equal(expectedName, company.Name);
        }


        [Fact]
        public void Constructor_ShouldGenerateUniqueId()
        {
            // Arrange & Act
            var company1 = new Company("Test Company 1");
            var company2 = new Company("Test Company 2");

            // Assert
            Assert.NotEqual(company1.Id, company2.Id);
        }

    }
}
