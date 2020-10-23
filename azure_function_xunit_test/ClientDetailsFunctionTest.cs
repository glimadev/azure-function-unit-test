using azure_function;
using azure_function.Models;
using azure_function.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace azure_function_xunit_test
{
    public class ClientDetailsFunctionTest
    {
        [Fact]
        public async void GetSuccess()
        {
            //Arrange
            int clientId = 2;

            var clientFakeRepositoryMock = new Mock<IClientFakeRepository>();

            clientFakeRepositoryMock.Setup(x => x.GetClientDetails(It.IsAny<int>())).Returns(new ClientDetailModel
            {
                Id = clientId,
                Name = "Cliente 2"
            });

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>() {
                    { "clientId", new StringValues(clientId.ToString()) }
                })
            };

            //Act
            OkObjectResult result = (OkObjectResult)await ClientDetailsFunction.Run(request, clientFakeRepositoryMock.Object);

            //Assert
            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }

        [Fact]
        public async void PostSuccess()
        {
            //Arrange
            int clientId = 2;

            var clientFakeRepositoryMock = new Mock<IClientFakeRepository>();

            clientFakeRepositoryMock.Setup(x => x.GetClientDetails(It.IsAny<int>())).Returns(new ClientDetailModel
            {
                Id = clientId,
                Name = "Cliente 2"
            });

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(new { clientId }));
            writer.Flush();
            stream.Position = 0;

            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = stream;            

            //Act
            OkObjectResult result = (OkObjectResult)await ClientDetailsFunction.Run(request, clientFakeRepositoryMock.Object);

            //Assert
            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }

        [Fact]
        public async void NotFound()
        {
            //Arrange
            int clientId = 2;

            var clientFakeRepositoryMock = new Mock<IClientFakeRepository>();

            clientFakeRepositoryMock.Setup(x => x.GetClientDetails(It.IsAny<int>())).Returns(default(ClientDetailModel));

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>() {
                    { "clientId", new StringValues(clientId.ToString()) }
                })
            };

            //Act
            NotFoundResult result = (NotFoundResult)await ClientDetailsFunction.Run(request, clientFakeRepositoryMock.Object);

            //Assert
            Assert.Equal(result.StatusCode, StatusCodes.Status404NotFound);
        }
    }
}
