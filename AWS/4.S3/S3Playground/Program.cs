using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

AmazonS3Client s3Client = new();

// Uploading data
await using FileStream inputStream = new(
    "/Users/faber/Repos/dotnet/AWS/4.S3/S3Playground/movies.csv",
    FileMode.Open,
    FileAccess.Read
);

PutObjectRequest putObjectRequest = new()
{
    BucketName = "faberaws", Key = "files/movies.csv", ContentType = "text/csv", InputStream = inputStream
};

await s3Client.PutObjectAsync(putObjectRequest);


// Downloading data
using MemoryStream memoryStream = new();

GetObjectRequest getObjectRequest = new() { BucketName = "faberaws", Key = "files/movies.csv" };

GetObjectResponse? response = await s3Client.GetObjectAsync(getObjectRequest);

response.ResponseStream.CopyTo(memoryStream);

string text = Encoding.Default.GetString(memoryStream.ToArray());

Console.Write(text);