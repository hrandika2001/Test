    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Xunit;

    namespace MyApiProject.Tests
    {
        public class Apitest : IDisposable
        {
            private readonly HttpClient _client;
            private string _createdObjectId = null!;

            public Apitest()
            {
                _client = new HttpClient
                {
                    BaseAddress = new Uri("https://api.restful-api.dev/")
                };
            }

            [Fact]
            public async Task GetAllObjects_ReturnsListOfObjects()
            {
                // Act
                var response = await _client.GetAsync("objects");

                // Assert
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotNull(content);
                Assert.NotEmpty(content);

                var objects = JsonSerializer.Deserialize<ApiObject[]>(content);
                Assert.NotNull(objects);
                Assert.NotEmpty(objects);
            }

            [Fact]
            public async Task AddObject_ReturnsCreatedObject()
            {
                // Arrange
                var newObject = new ApiObject
                {
                    Name = "Google Pixel 6 Pro",
                    Data = new DataObject
                    {
                        Color = "Cloudy White",
                        Capacity = "128 GB"
                    }
                };
                var content = new StringContent(JsonSerializer.Serialize(newObject), Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PostAsync("objects", content);

                // Assert
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                var createdObjectContent = await response.Content.ReadAsStringAsync();
                Assert.NotNull(createdObjectContent);

                var createdObject = JsonSerializer.Deserialize<ApiObject>(createdObjectContent);
                Assert.NotNull(createdObject);
                _createdObjectId = createdObject.Id;
                Assert.NotNull(_createdObjectId);
            }

            [Fact]
            public async Task GetObjectById_ReturnsObject()
            {
                await AddObject_ReturnsCreatedObject();

                // Act
                var response = await _client.GetAsync($"objects/{_createdObjectId}");

                // Assert
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotNull(content);

                var obj = JsonSerializer.Deserialize<ApiObject>(content);
                Assert.NotNull(obj);
                Assert.Equal(_createdObjectId, obj.Id);
                Assert.Equal("Google Pixel 6 Pro", obj.Name);

                // Verify the 'data' field, which can be null or contain additional properties
                Assert.NotNull(obj.Data);
                Assert.Equal("Cloudy White", obj.Data.Color);
                Assert.Equal("128 GB", obj.Data.Capacity);
            }

            [Fact]
            public async Task UpdateObject_ReturnsUpdatedObject()
            {
                await AddObject_ReturnsCreatedObject();

                // Arrange
                var updatedObject = new ApiObject
                {
                    Name = "Google Pixel 6 Pro Updated",
                    Data = new DataObject
                    {
                        Color = "Stormy Black",
                        Capacity = "256 GB"
                    }
                };
                var content = new StringContent(JsonSerializer.Serialize(updatedObject), Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PutAsync($"objects/{_createdObjectId}", content);

                // Assert
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                var updatedObjectContent = await response.Content.ReadAsStringAsync();
                Assert.NotNull(updatedObjectContent);

                var obj = JsonSerializer.Deserialize<ApiObject>(updatedObjectContent);
                Assert.NotNull(obj);
                Assert.Equal("Google Pixel 6 Pro Updated", obj.Name);
                Assert.NotNull(obj.Data);
                Assert.Equal("Stormy Black", obj.Data.Color);
                Assert.Equal("256 GB", obj.Data.Capacity);

            }

            [Fact]
            public async Task DeleteObject_ReturnsNoContent()
            {
                await AddObject_ReturnsCreatedObject();

                // Act
                var response = await _client.DeleteAsync($"objects/{_createdObjectId}");

                // Assert
                Assert.True(
                    response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.StatusCode == System.Net.HttpStatusCode.OK,
                    $"Expected status code 204 No Content or 200 OK, but got {(int)response.StatusCode} {response.StatusCode}."
                );
            }

            // Dispose method to clean up resources
            public void Dispose()
            {
                _client?.Dispose();
            }
        }
    }