using Flashcards.Common.DTO.Flashcard;
using Flashcards.Common.DTO.Identity;
using Flashcards.Common.RepositoryContracts;
using System.Net.Http.Json;

namespace Flashcards.Common.Repositories
{
	public class ApiRepository : IApiRepository
	{
		private readonly HttpClient _httpClient;
		private readonly string _connectionString;
		private readonly string _loginEndpointPath;
		private readonly string _logoutEndpointPath;
		private readonly string _registerEndpointPath;
		private readonly string _syncEndpointPath;

		public ApiRepository(
				string connectionString,
				string loginEndpointPath = nameof(Login),
				string logoutEndpointPath = nameof(Logout),
				string registerEndpointPath = nameof(Register),
				string syncEndpointPath = nameof(SyncAndGetCards)
			)
        {
            _connectionString = connectionString;
			_loginEndpointPath = loginEndpointPath;
			_logoutEndpointPath = logoutEndpointPath;
			_registerEndpointPath = registerEndpointPath;
			_syncEndpointPath = syncEndpointPath;
			_httpClient = new HttpClient();
        }

        public async Task Login(LoginDTO loginDTO)
		{
			var result = await _httpClient.PostAsJsonAsync(_connectionString + _loginEndpointPath, loginDTO);

			result.EnsureSuccessStatusCode();
		}

		public async Task Logout()
		{
			var result = await _httpClient.GetAsync(_connectionString + _logoutEndpointPath);

			result.EnsureSuccessStatusCode();
		}

		public async Task Register(RegisterDTO registerDTO)
		{
			var result = await _httpClient.PostAsJsonAsync(_connectionString + _registerEndpointPath, registerDTO);

			result.EnsureSuccessStatusCode();
		}

		public async Task<IEnumerable<FlashcardApiResponse>?> SyncAndGetCards(IEnumerable<FlashcardApiRequest> flashcards)
		{
			var responseMessage = await _httpClient.PostAsJsonAsync(_connectionString + _syncEndpointPath, flashcards);

			responseMessage.EnsureSuccessStatusCode();

			var apiFlashcards = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FlashcardApiResponse>>();

			return apiFlashcards;
		}
	}
}
