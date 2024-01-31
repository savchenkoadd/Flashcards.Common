using Flashcards.Common.DTO.Identity;
using Flashcards.Common.RepositoryContracts;
using Flashcards.Common.ServiceContracts;
using FlashCards.Common.Helpers;

namespace Flashcards.Common.Services
{
	public class UserService : IUserService
	{
		private readonly IApiRepository _apiRepository;
		private bool _isLoggedIn;

        public UserService(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
			_isLoggedIn = false;
        }

        public async Task Login(LoginDTO? loginDTO)
		{
			await ValidationHelper.ValidateObjects(loginDTO);

			await _apiRepository.Login(loginDTO!);
			_isLoggedIn = true;
		}

		public async Task Logout()
		{
			if (!_isLoggedIn)
			{
				throw new InvalidOperationException("Unable to logout. You must be logged in to logout.");
			}

			await _apiRepository.Logout();
			_isLoggedIn = false;
		}

		public async Task Register(RegisterDTO? registerDTO)
		{
			await ValidationHelper.ValidateObjects(registerDTO);

			await _apiRepository.Register(registerDTO!);
			_isLoggedIn = true;
		}
	}
}
