using Amazon.S3;
using Amazon.S3.Model;

namespace Customers.Api.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private readonly string    _bucketName = "faberaws";

    public CustomerImageService(IAmazonS3 s3)
    {
        _s3 = s3;
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        PutObjectRequest putObjectRequest = new()
        {
            BucketName  = _bucketName,
            Key         = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-originalname"] = file.FileName, ["x-amz-extension"] = Path.GetExtension(file.FileName)
            }
        };

        return await _s3.PutObjectAsync(putObjectRequest);
    }

    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        GetObjectRequest getObjectRequest = new() { BucketName = _bucketName, Key = $"images/{id}" };

        return await _s3.GetObjectAsync(getObjectRequest);
    }

    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        DeleteObjectRequest deleteObjectRequest = new() { BucketName = _bucketName, Key = $"images/{id}" };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}