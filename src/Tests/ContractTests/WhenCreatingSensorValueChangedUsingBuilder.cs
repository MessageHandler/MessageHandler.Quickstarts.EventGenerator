using Contract;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ContractTests
{
    public class WhenCreatingSensorValueChangedUsingBuilder
    {
        [Fact]
        public async Task ShouldAdhereToContract()
        {
            var evt = new SensorValueChangedBuilder()
                                        .WellknownSensor("D98204AA-5328-4078-8D66-354286514994")
                                        .Build();

            string csOutput = JsonSerializer.Serialize(evt);

            await File.WriteAllTextAsync(@"./.verification/D98204AA-5328-4078-8D66-354286514994/actual.sensorvaluechanged.event.cs.json", csOutput);

            // output provided by similar tests on the receiver side
            var receiverOutput = await File.ReadAllTextAsync(@"./.verification/D98204AA-5328-4078-8D66-354286514994/verified.sensorvaluechanged.event.cs.json");

            Assert.Equal(receiverOutput, csOutput);
        }
    }
}
