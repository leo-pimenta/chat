using Domain;
using Infra.Repositories;

namespace Test
{
    public class WSConnectionInMemoryRepositoryTest
    {
        private readonly IWSConnectionRepository WsConnectionRepository;
        
        public WSConnectionInMemoryRepositoryTest()
        {
            this.WsConnectionRepository = new WSConnectionInMemoryRepository();
        }

        [Fact]
        public async Task Should_StoreConnectionsAdded()
        {
            var connection1 = new WSConnection("connId1", "userId1");
            var connection2 = new WSConnection("connId2", "userId2");
            var connection3 = new WSConnection("connId3", "userId3");

            await this.WsConnectionRepository.AddAsync(connection1);
            await this.WsConnectionRepository.AddAsync(connection2);
            await this.WsConnectionRepository.AddAsync(connection3);

            var retrievedConnection1 = await WsConnectionRepository
                .GetByConnectionId(connection1.Id);
            
            var retrievedConnection2 = await WsConnectionRepository
                .GetByConnectionId(connection2.Id);

            var retrievedConnection3 = await WsConnectionRepository
                .GetByConnectionId(connection3.Id);
            
            retrievedConnection1.Should().BeEquivalentTo(connection1);
            retrievedConnection2.Should().BeEquivalentTo(connection2);
            retrievedConnection3.Should().BeEquivalentTo(connection3);
        }

        [Fact]
        public async Task ShouldNot_StoreByReference()
        {
            var connection = new WSConnection("connId1", "userId1");
            await WsConnectionRepository.AddAsync(connection);
            var retrievedConnection = await WsConnectionRepository
                .GetByConnectionId(connection.Id);
            retrievedConnection.Should().NotBeSameAs(connection);
        }

        [Fact]
        public async Task ShouldThrow_ArgumentNullException_When_WSConnectionIsNull()
        {
            #pragma warning disable CS8625
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await WsConnectionRepository.AddAsync(null));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasNullUserId()
        {
            var connection = new WSConnection("connId1", null);
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasEmptyUserId()
        {
            var connection = new WSConnection("connId1", "");
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasWhitespacesUserId()
        {
            var connection = new WSConnection("connId1", "   ");
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasNullConnectionId()
        {
            var connection = new WSConnection(null, "userId1");
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasEmptyConnectionId()
        {
            var connection = new WSConnection(null, "");
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_WSConnectionHasWhitespacesConnectionId()
        {
            var connection = new WSConnection(null, "   ");
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connection));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_ConnectionIdHasBeenStoredAlready()
        {
            var connection = new WSConnection("connId", "userId");
            var connectionWithDupeConnectionId = new WSConnection("connId", "otherUserId");
            await WsConnectionRepository.AddAsync(connection);
            
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connectionWithDupeConnectionId));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_When_UserIdHasBeenStoredAlready()
        {
            var connection = new WSConnection("connId", "userId");
            var connectionWithDupeUserId = new WSConnection("anotherConnId", "userId");
            await WsConnectionRepository.AddAsync(connection);
            
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await WsConnectionRepository.AddAsync(connectionWithDupeUserId));
        }

        [Fact]
        public async Task ShouldDelete_StoresConnectionId()
        {
            var connection = new WSConnection("connId", "userId");
            await WsConnectionRepository.AddAsync(connection);
            (await WsConnectionRepository.GetByConnectionId(connection.Id)).Should().NotBeNull();

            await WsConnectionRepository.DeleteAsync(connection.Id);
            (await WsConnectionRepository.GetByConnectionId(connection.Id)).Should().BeNull();
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_WhenDeletingByNullConnectionId()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await WsConnectionRepository.DeleteAsync(null));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_WhenDeletingByEmptyConnectionId()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await WsConnectionRepository.DeleteAsync(""));
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_WhenDeletingByWhitespacesConnectionId()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await WsConnectionRepository.DeleteAsync("   "));
        }
    }
}