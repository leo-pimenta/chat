using Confluent.Kafka;
using Domain;
using Infra.Kafka;
using NodaTime;

namespace Test
{
    public class KafkaProducerTest
    {
        private readonly Mock<IProducer<string, Message>> ProducerMock;
        private readonly KafkaProducer<Message> Service;
        private readonly List<Confluent.Kafka.Message<string, Message>> MockMessageStore;
        private readonly Instant DefaultInstant = Instant.FromUtc(2022, 1, 1, 8, 27);
        private readonly Mock<IClock> ClockMock;

        public KafkaProducerTest()
        {
            ClockMock = new Mock<IClock>();
            ClockMock.Setup(clock => clock.GetCurrentInstant()).Returns(DefaultInstant);
            this.ProducerMock = new Mock<IProducer<string, Message>>();
            this.Service = new KafkaProducer<Message>(ProducerMock.Object, "t0picn@me", ClockMock.Object);
            this.MockMessageStore = new List<Message<string, Message>>();
            MockProducerWithDefaultBehavior();
        }

        [Fact]
        public void Should_StoreMessages()
        {
            var message1 = new Message("senderId1", "targetId1", "body1");
            var message2 = new Message("senderId2", "targetId2", "body2");
            var message3 = new Message("senderId3", "targetId3", "body3");
            
            Service.Add(message1, message1.TargetId);
            Service.Add(message2, message2.TargetId);
            Service.Add(message3, message3.TargetId);

            MockMessageStore.Should().HaveCount(3);
            MockMessageStore[0].Value.Should().BeSameAs(message1);
            MockMessageStore[1].Value.Should().BeSameAs(message2);
            MockMessageStore[2].Value.Should().BeSameAs(message3);
        }

        [Fact]
        public void Should_SetCorrectTimestamp_InNewMessage()
        {
            const string key = "targetId1";
            Service.Add(new Message("senderId1", key, "body1"), key);
            MockMessageStore.Should().HaveCount(1);
            var expectedTimestamp = new Timestamp(DefaultInstant.ToUnixTimeMilliseconds(), TimestampType.CreateTime);
            MockMessageStore[0].Timestamp.Should().BeEquivalentTo(expectedTimestamp);
        }

        [Fact]
        public void Should_UseTargetId_AsKey()
        {
            const string key = "targetId1";
            var message = new Message("senderId1", "targetId1", "body1");
            Service.Add(message, key);
            MockMessageStore.Should().HaveCount(1);
            MockMessageStore[0].Key.Should().BeEquivalentTo(message.TargetId);
        }

        [Fact]
        public void ShouldThrow_IfConstructorHas_InvalidInput()
        {
            Assert.Throws<ArgumentNullException>(() => new KafkaProducer<Message>(null!, "t0picn@me", ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer<Message>(ProducerMock.Object, null!, ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer<Message>(ProducerMock.Object, "", ClockMock.Object));
            Assert.Throws<ArgumentException>(() => new KafkaProducer<Message>(ProducerMock.Object, "  ", ClockMock.Object));
            Assert.Throws<ArgumentNullException>(() => new KafkaProducer<Message>(ProducerMock.Object, "t0picn@ame", null!));
        }

        [Fact]
        public void ShouldThrow_IfAdd_HasInvalidInput()
        {
            Assert.Throws<ArgumentNullException>(() => Service.Add(null!, ""));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("a", "a", "a"), null!));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("a", "a", "a"), ""));
            Assert.Throws<ArgumentException>(() => Service.Add(new Message("a", "a", "a"), "  "));
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