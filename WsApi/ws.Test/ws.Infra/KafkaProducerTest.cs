using Confluent.Kafka;
using Domain;
using Infra.Kafka;
using NodaTime;

namespace Test
{
    public class KafkaProducerTest
    {
        private readonly Mock<IProducer<string, Message>> ProducerMock;
        private readonly KafkaProducer Service;
        private readonly List<Confluent.Kafka.Message<string, Message>> MockMessageStore;
        private readonly Instant DefaultInstant = Instant.FromUtc(2022, 1, 1, 8, 27);
        private readonly Mock<IClock> ClockMock;

        public KafkaProducerTest()
        {
            ClockMock = new Mock<IClock>();
            ClockMock.Setup(clock => clock.GetCurrentInstant()).Returns(DefaultInstant);
            this.ProducerMock = new Mock<IProducer<string, Message>>();
            this.Service = new KafkaProducer(ProducerMock.Object, "t0picn@me", ClockMock.Object);
            this.MockMessageStore = new List<Message<string, Message>>();
            MockProducerWithDefaultBehavior();
        }

        [Fact]
        public void Should_StoreMessages()
        {
            var message1 = new Message("senderId1", "targetId1", "body1");
            var message2 = new Message("senderId2", "targetId2", "body2");
            var message3 = new Message("senderId3", "targetId3", "body3");
            
            Service.Add(message1);
            Service.Add(message2);
            Service.Add(message3);

            MockMessageStore.Should().HaveCount(3);
            MockMessageStore[0].Value.Should().BeSameAs(message1);
            MockMessageStore[1].Value.Should().BeSameAs(message2);
            MockMessageStore[2].Value.Should().BeSameAs(message3);
        }

        [Fact]
        public void Should_SetCorrectTimestamp_InNewMessage()
        {
            Service.Add(new Message("senderId1", "targetId1", "body1"));
            MockMessageStore.Should().HaveCount(1);
            var expectedTimestamp = new Timestamp(DefaultInstant.ToUnixTimeMilliseconds(), TimestampType.CreateTime);
            MockMessageStore[0].Timestamp.Should().BeEquivalentTo(expectedTimestamp);
        }

        [Fact]
        public void Should_UseTargetId_AsKey()
        {
            var message = new Message("senderId1", "targetId1", "body1");
            Service.Add(message);
            MockMessageStore.Should().HaveCount(1);
            MockMessageStore[0].Key.Should().BeEquivalentTo(message.TargetId);
        }

        [Fact]
        public void ShouldThrow_IfConstructorHas_InvalidInput()
        {
            #pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new KafkaProducer(null, "t0picn@me", ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer(ProducerMock.Object, null, ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer(ProducerMock.Object, "", ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer(ProducerMock.Object, "  ", ClockMock.Object));
            Assert.Throws<ArgumentNullException>(() => new KafkaProducer(ProducerMock.Object, "t0picn@ame", null));
            #pragma warning restore CS8625
        }

        [Fact]
        public void ShouldThrow_IfAdd_HasInvalidInput()
        {
            #pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => Service.Add(null));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message(null, "targetId", "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("", "targetId", "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("  ", "targetId", "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", null, "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", "", "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", "   ", "body")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", "targetId", null)));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", "targetId", "")));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("senderId", "targetId", "   ")));
            #pragma warning restore CS8625
        }

        private void MockProducerWithDefaultBehavior()
        {
            ProducerMock.Setup(producer => producer.Produce(
                    It.IsAny<string>(), 
                    It.IsAny<Confluent.Kafka.Message<string, Message>>(), 
                    It.IsAny<Action<DeliveryReport<string, Message>>>()))
                .Callback<string, Confluent.Kafka.Message<string, Message>, Action<DeliveryReport<string, Message>>>(
                    (key, message, callback) => this.MockMessageStore.Add(message));
        }
    }
}