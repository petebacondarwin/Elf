using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Elf.Tests.Web.Mvc {
    public static class Mocks {
        public static Mock<HttpContextBase> MockupHttpContext(string url) {
            var mockHttpContext = new Mock<HttpContextBase>();

            // Mock the request
            var mockRequest = new Mock<HttpRequestBase>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);
            
            // Mock the response
            var mockResponse = new Mock<HttpResponseBase>();
            mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);
            mockResponse.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);
            
            return mockHttpContext;
        }

        public static Mock<IControllerFactory> MockupControllerFactory() {
            var mockContollerFactory = new Mock<IControllerFactory>();
            mockContollerFactory.Setup(x => x.CreateController(It.IsAny<RequestContext>(), It.IsAny<string>())).Returns(new PageController());
            mockContollerFactory.Setup(x=> x.GetControllerSessionBehavior(It.IsAny<RequestContext>(), It.IsAny<string>())).Returns(SessionStateBehavior.Default);
            return mockContollerFactory;
        }
    }
}
