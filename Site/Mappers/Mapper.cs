using ReconhecimentoFacialAWS.Domains.Commands;
using ReconhecimentoFacialAWS.Extensions;
using ReconhecimentoFacialAWS.ViewModels;

namespace ReconhecimentoFacialAWS.Mappers;

public static class Mapper
{
    public static LoginUserCOM MapToCommand(LoginVM viewModel)
    {
        return new LoginUserCOM
        {
            Login = viewModel.Login,
            Password = viewModel.Password
        };
    }

    public static AddUserCOM MapToCommand(UserVM viewModel)
    {
        return new AddUserCOM
        {
            Login = viewModel.Login.ToLower(),
            Password = viewModel.Password.ToLower()
        };
    }

    public static AddPhotoUserCOM MapToCommand(string login, string photo)
    {
        return new AddPhotoUserCOM
        {
            Login = login,
            Photo = photo
        };
    }

    public static CompareFacesVM MapToView(AWSCompareFace aws)
    {
        return new CompareFacesVM
        { 
            Photo1 = aws.Photo1,
            Photo2 = aws.Photo2,
            Similarity = aws.Similarity,
            Gender = aws.Gender,
            AgeRange = aws.AgeRange,
            Quality = aws.Quality,
            Message = aws.Message,
            NoMatches = aws.NoMatches,
            Spoofing = aws.Spoofing
        };
    }
}
