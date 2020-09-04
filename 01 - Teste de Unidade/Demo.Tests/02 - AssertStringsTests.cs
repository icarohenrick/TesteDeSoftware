using Xunit;

namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringTools_UnirNomes_RetornarNomeCompleto()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.Equal("Patrizia Mastrodonato", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveIgnorarCase()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.Equal("PATRIZIA MASTRODONATO", nomeCompleto, true);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveConterTrecho()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.Contains("trizia", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveComecarCom()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.StartsWith("Pat", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveAcabarCom()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.EndsWith("nato", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_ValidarExpressaoRegular()
        {
            //Arrange
            var sut = new StringTools();

            //Act
            var nomeCompleto = sut.Unir("Patrizia", "Mastrodonato");

            //Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
        }
    }
}
