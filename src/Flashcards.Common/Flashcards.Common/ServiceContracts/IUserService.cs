using Flashcards.Common.DTO.Identity;

namespace Flashcards.Common.ServiceContracts
{
	public interface IUserService
	{
		Task Login(LoginDTO? loginDTO);

		Task Register(RegisterDTO? registerDTO);

		Task Logout();
	}
}
