using Flashcards.Common.DTO.Flashcard;
using Flashcards.Common.DTO.Identity;

namespace Flashcards.Common.RepositoryContracts
{
	public interface IApiRepository
	{
		Task Register(RegisterDTO registerDTO);

		Task Login(LoginDTO loginDTO);

		Task Logout();

		Task<IEnumerable<FlashcardApiResponse>?> SyncAndGetCards(IEnumerable<FlashcardApiRequest> flashcards);
	}
}
