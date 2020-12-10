using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TwilightSparkle.Firebase.Storage
{
    public class FirebaseStorageTask
    {
        private const int ProgressReportDelayMiliseconds = 500;

        private readonly Task<string> _uploadTask;
        private readonly Stream _stream;


        public string TargetUrl { get; private set; }

        public Progress<FirebaseStorageProgress> Progress { get; private set; }


        public FirebaseStorageTask(FirebaseStorageOptions options, string url, string downloadUrl, Stream stream, CancellationToken cancellationToken, string mimeType = null)
        {
            TargetUrl = url;
            _uploadTask = UploadFile(options, url, downloadUrl, stream, cancellationToken, mimeType);
            _stream = stream;
            Progress = new Progress<FirebaseStorageProgress>();

            Task.Factory.StartNew(ReportProgressLoop);
        }


        public TaskAwaiter<string> GetAwaiter()
        {
            return _uploadTask.GetAwaiter();
        }


        private async Task<string> UploadFile(FirebaseStorageOptions options, string url, string downloadUrl, Stream stream, CancellationToken cancellationToken, string mimeType = null)
        {
            var responseData = "N/A";

            try
            {
                using (var client = await options.CreateHttpClientAsync())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StreamContent(stream)
                    };

                    if (!string.IsNullOrEmpty(mimeType))
                    {
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                    }

                    var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);

                    return downloadUrl + data["downloadTokens"];
                }
            }
            catch (TaskCanceledException)
            {
                if (options.ThrowOnCancel)
                {
                    throw;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new FirebaseStorageException(url, responseData, ex);
            }
        }

        private async void ReportProgressLoop()
        {
            while (!_uploadTask.IsCompleted)
            {
                await Task.Delay(ProgressReportDelayMiliseconds);

                try
                {
                    OnReportProgress(new FirebaseStorageProgress(_stream.Position, _stream.Length));
                }
                catch (ObjectDisposedException)
                {
                    // there is no 100 % way to prevent ObjectDisposedException, there are bound to be concurrency issues.
                    return;
                }
            }
        }

        private void OnReportProgress(FirebaseStorageProgress progress)
        {
            (Progress as IProgress<FirebaseStorageProgress>).Report(progress);
        }
    }
}
