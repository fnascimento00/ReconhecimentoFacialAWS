using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.Extensions.Options;

namespace ReconhecimentoFacialAWS.Extensions;

public interface ICameraService 
{
    Task<AWSCompareFace> CompareWithRekognition(string photo1, string photo2);
}

public class CameraService : ICameraService
{
    private readonly AWSSettings _awsSettings;

    public CameraService(IOptions<AWSSettings> optionsAwsSettings)
    {
        _awsSettings = optionsAwsSettings.Value;
    }

    public async Task<AWSCompareFace> CompareWithRekognition(string photo1, string photo2)
    {
        try
        {
            var _compareFaces = new AWSCompareFace
            {
                Photo1 = photo1,
                Photo2 = photo2,
                Spoofing = false
            };

            var credentials = new Amazon.Runtime.BasicAWSCredentials(_awsSettings.AccessKey, _awsSettings.SecretKey);
            var regionEndpoint = RegionEndpoint.GetBySystemName(_awsSettings.Region);
            var rekognitionClient = new AmazonRekognitionClient(credentials, regionEndpoint);

            photo1 = photo1
                .Replace("data:image/png;base64,", "")
                .Replace("data:image/jpg;base64,", "")
                .Replace("data:image/jpeg;base64,", "");

            byte[] imageBytes1 = Convert.FromBase64String(photo1);
            var image1 = new Amazon.Rekognition.Model.Image { Bytes = new MemoryStream(imageBytes1) };

            photo2 = photo2
                .Replace("data:image/png;base64,", "")
                .Replace("data:image/jpg;base64,", "")
                .Replace("data:image/jpeg;base64,", "");

            byte[] imageBytes2 = Convert.FromBase64String(photo2);
            var image2 = new Amazon.Rekognition.Model.Image { Bytes = new MemoryStream(imageBytes2) };

            float similarityThreshold = 90F;

            var compareFacesRequest = new CompareFacesRequest
            {
                SourceImage = image1,
                TargetImage = image2,
                SimilarityThreshold = similarityThreshold,
            };

            var compareFacesResponse = await rekognitionClient.CompareFacesAsync(compareFacesRequest);

            if (compareFacesResponse.FaceMatches.Count > 0)
            {
                compareFacesResponse.FaceMatches.ForEach(match =>
                {
                    ComparedFace face = match.Face;
                    BoundingBox position = face.BoundingBox;
                    _compareFaces.Similarity = match.Similarity;
                });

                var detectFacesRequest = new DetectFacesRequest
                {
                    Image = new Image
                    {
                        Bytes = new System.IO.MemoryStream(imageBytes1)
                    },
                    Attributes = new List<string> { "ALL" }
                };

                var detectFacesResponse = await rekognitionClient.DetectFacesAsync(detectFacesRequest);

                foreach (var faceDetail in detectFacesResponse.FaceDetails)
                {
                    _compareFaces.Gender = faceDetail.Gender;
                    _compareFaces.AgeRange = faceDetail.AgeRange;
                    _compareFaces.Quality = faceDetail.Quality;
                }
            }
            else
            {
                _compareFaces.NoMatches = "Sua face não corresponde com a foto armazenada.";
            }

            if (string.IsNullOrWhiteSpace(_compareFaces.NoMatches))
            {
                _compareFaces.Message = "Faces com " + _compareFaces.Similarity + "% de similaridade.";

                if (_compareFaces.Quality.Brightness < 70)
                {
                    _compareFaces.Message += " Brilho da Foto Capturada abaixo valor mínimo esperado de 70%.";
                }

                if (_compareFaces.Quality.Sharpness < 70)
                {
                    _compareFaces.Message += " Nitidez da Foto Capturada abaixo valor mínimo esperado de 70%.";
                }

                if (_compareFaces.Quality.Brightness < 70 || _compareFaces.Quality.Sharpness < 70)
                {
                    _compareFaces.Message += " Detecção suspeita, possível spoofing.";
                    _compareFaces.Spoofing = true;
                }
            }

            return _compareFaces;
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
}
