using Contract;
using MessageHandler.Runtime.StreamProcessing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker;
using Xunit;

namespace ComponentTests
{
    public class WhileGeneratingSensorValueChangedEvents
    {
        [Fact]
        public async Task GivenWellknownSensor_WhenGeneratingEvent_ThenShouldDispatchSensorValueChanged()
        {
            // given
            var dispatcherMock = new Mock<IDispatchMessages>();

            dispatcherMock.Setup(dispatcher => dispatcher.Dispatch(It.IsAny<IEnumerable<SensorValueChanged>>(), true));

            //when
            var generator = new ReportDataDownloadedViaWifi(dispatcherMock.Object);

            await generator.Generate();

            //then          
            dispatcherMock.Verify(dispatcher => dispatcher.Dispatch(It.Is<IEnumerable<SensorValueChanged>>(evts =>
                        evts.Any(evt => evt.Unit =="bytes")
            ), true), Times.Once());
        }
    }
}
