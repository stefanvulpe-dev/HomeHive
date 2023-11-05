using HomeHive.Domain.Entities;
using HomeHive.Infrastructure;
using HomeHive.Infrastructure.Repositories;

var user = User.Create("John", "Doe", "johndoe@gmail.com", "12345678", "12345678", "picture", new List<Estate>());
var userRepository = new UserRepository(new HomeHiveContext());
await userRepository.AddAsync(user.Value);