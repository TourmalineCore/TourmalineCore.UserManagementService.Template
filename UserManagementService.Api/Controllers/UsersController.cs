using Microsoft.AspNetCore.Mvc;
using UserManagementService.Api.Dto.Users;
using UserManagementService.Application.Users.Commands;
using UserManagementService.Application.Users.Queries;

namespace UserManagementService.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly GetUserListQueryHandler _getUserListQueryHandler;
        private readonly GetUserByEmailQueryHandler _getUserByIdQueryHandler;
        private readonly CreateUserCommandHandler _createUserCommandHandler;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly DeleteUserCommandHandler _deleteUserCommandHandler;
        private readonly AddRoleToUserCommandHandler _addRoleToUserCommandHandler;

        public UsersController(
            GetUserListQueryHandler getUserListQueryHandler,
            GetUserByEmailQueryHandler getUserByIdQueryHandler,
            CreateUserCommandHandler createUserCommandHandler,
            UpdateUserCommandHandler updateUserCommandHandler,
            DeleteUserCommandHandler deleteUserCommandHandler,
            AddRoleToUserCommandHandler addRoleToUserCommandHandler)
        {
            _getUserListQueryHandler = getUserListQueryHandler;
            _getUserByIdQueryHandler = getUserByIdQueryHandler;
            _createUserCommandHandler = createUserCommandHandler;
            _updateUserCommandHandler = updateUserCommandHandler;
            _deleteUserCommandHandler = deleteUserCommandHandler;
            _addRoleToUserCommandHandler = addRoleToUserCommandHandler;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<UserListItemDto>> FindAll([FromQuery] GetUserListQuery getUserListQuery)
        {
            var users = await _getUserListQueryHandler.Handle(getUserListQuery);

            return users.Select(x => new UserListItemDto(
                x.Id,
                x.Name,
                x.Surname,
                x.Email,
                x.RoleName
              )
          );
        }

        [HttpGet("find")]
        public async Task<UserDto> FindByEmail([FromQuery] GetUserByEmailQuery getUserByIdQuery)
        {
            var user = await _getUserByIdQueryHandler.Handle(getUserByIdQuery);

            return new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.RoleName,
                user.Privileges
                );
        }

        [HttpPost("create")]
        public async Task<long> Create([FromBody] CreateUserCommand createUserCommand)
        {
            return await _createUserCommandHandler.Handle(createUserCommand);
        }

        [HttpPut("update")]
        public Task Update([FromBody] UpdateUserCommand updateUserCommand)
        {
            return _updateUserCommandHandler.Handle(updateUserCommand);
        }

        [HttpDelete("delete")]
        public Task Delete([FromQuery] DeleteUserCommand deleteUserCommand)
        {
            return _deleteUserCommandHandler.Handle(deleteUserCommand);
        }

        [HttpPost("add-role")]
        public Task AddRole([FromBody] AddRoleToUserCommand addRoleToUserCommand)
        {
            return _addRoleToUserCommandHandler.Handle(addRoleToUserCommand);
        }
    }
}
