using Amazon.Rekognition.Model;

namespace ReconhecimentoFacialAWS.Extensions;

public class AWSCompareFace
{
    public string Photo1 { get; set; }
    public string Photo2 { get; set; }
    public float Similarity { get; set; }
    public Gender Gender { get; set; }
    public AgeRange AgeRange { get; set; }
    public ImageQuality Quality { get; set; }
    public string Message { get; set; }
    public string NoMatches { get; set; }
    public bool Spoofing { get; set; }
}
